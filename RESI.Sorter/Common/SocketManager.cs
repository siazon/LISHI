using RESI.Sorter;
using Season;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RESI.Sorter
{
    public class SocketManager
    {

        public Dictionary<string, SocketInfo> _listSocketInfo = null;
        Socket _socket = null;
        EndPoint _endPoint = null;
        bool _isListening = false;
        int BACKLOG = 10;

        public delegate void OnConnectedHandler(SocketInfo socketInfo);
        public event OnConnectedHandler OnConnected;
        public delegate void OnReceiveMsgHandler(SocketInfo socketInfo);
        public event OnReceiveMsgHandler OnReceiveMsg;
        public event OnReceiveMsgHandler OnDisConnected;

        public SocketManager(int port,string ip="0.0.0.0")
        {

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress _ip = IPAddress.Parse(ip);
            _endPoint = new IPEndPoint(_ip, port);
            _listSocketInfo = new Dictionary<string, SocketInfo>();
            Thread threadRec = new Thread(QueueSendMsgPro);
            threadRec.IsBackground = true;
            threadRec.Start();
            //Task.Run(() =>
            //{
            //    QueueSendMsgPro();
            //});
        }

        public void Start()
        {
            _socket.Bind(_endPoint); //绑定端口
            _socket.Listen(BACKLOG); //开启监听
            _isListening = true;
            Thread acceptServer = new Thread(AcceptWork); //开启新线程处理监听
            acceptServer.IsBackground = true;
            acceptServer.Start();
            //Task.Run(() => { AcceptWork(); });
        }

        public void AcceptWork()
        {
            try
            {
                while (_isListening)
                {
                    Socket acceptSocket = _socket.Accept();
                    if (acceptSocket != null && this.OnConnected != null)
                    {
                        SocketInfo sInfo = new SocketInfo();
                        sInfo.socket = acceptSocket;
                        string str = ((System.Net.IPEndPoint)acceptSocket.RemoteEndPoint).Address.ToString();
                        //192.168.3.10  192.168.3.101
                        string ss = str + ":";
                        //IPAddress remote_ip = ((System.Net.IPEndPoint)acceptSocket.RemoteEndPoint).Address;//获取远程连接IP 
                        //包含指定的IP 和端口 
                        //必须遍历字典的key
                        List<string> listTemp = new List<string>();
                        foreach (var listkey in _listSocketInfo.Keys)
                        {
                            //如果有重复的IP
                            if (listkey.Contains(ss))
                            {
                                //将重复的先记录下来
                                listTemp.Add(listkey);

                            }
                        }
                        //再删除重复的IP
                        foreach (string liststr in listTemp)
                        {
                            //如果是空的直接删除
                            if (_listSocketInfo[liststr].socket == null)
                            {
                                //删除原有的
                                _listSocketInfo.Remove(liststr);
                            }
                            //如果不是空的先释放资源再删除
                            else
                            {
                                //shutDownClient(liststr); 
                                //_listSocketInfo[acceptSocket.RemoteEndPoint.ToString()].socket.Close();//就是这里报错  资源未完全释放
                                //_listSocketInfo[liststr].socket.Shutdown(SocketShutdown.Both);
                                //_listSocketInfo[liststr].socket.Disconnect(true);
                                //_listSocketInfo[liststr].isConnected = false;
                                //_listSocketInfo[liststr].socket.Close();
                                ////是否需要将socket值为空
                                //_listSocketInfo[liststr].socket = null;
                                ////删除原有的
                                //_listSocketInfo.Remove(liststr);

                            }
                        }
                        //最后将新的添加进来
                        _listSocketInfo.Add(acceptSocket.RemoteEndPoint.ToString(), sInfo);

                        OnConnected(sInfo);
                        Thread socketConnectedThread = new Thread(newSocketReceive);
                        socketConnectedThread.IsBackground = true;
                        socketConnectedThread.Start(acceptSocket);
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception err)
            {
                Console.Write(err.Message + "异常5");
            }
        }

        public void newSocketReceive(object obj)
        {
            Socket socket = obj as Socket;
            SocketInfo sInfo = _listSocketInfo[socket.RemoteEndPoint.ToString()];
            sInfo.isConnected = true;
            while (sInfo.isConnected)
            {
                try
                {
                    if (sInfo.socket == null) return;
                    //这里向系统投递一个接收信息的请求，并为其指定ReceiveCallBack做为回调函数 
                    sInfo.socket.BeginReceive(sInfo.buffer, 0, sInfo.buffer.Length, SocketFlags.None, ReceiveCallBack, sInfo.socket.RemoteEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //return;
                }
                Thread.Sleep(20);
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                EndPoint ep = ar.AsyncState as IPEndPoint;
                SocketInfo info = _listSocketInfo[ep.ToString()];
                int readCount = 0;
                try
                {
                    if (info.socket == null) return;
                    readCount = info.socket.EndReceive(ar);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //return;
                }
                if (readCount > 0)
                {
                    //byte[] buffer = new byte[readCount];
                    //Buffer.BlockCopy(info.buffer, 0, buffer, 0, readCount);
                    if (readCount < info.buffer.Length)
                    {
                        byte[] newBuffer = new byte[readCount];
                        Buffer.BlockCopy(info.buffer, 0, newBuffer, 0, readCount);
                        info.msgBuffer = newBuffer;
                    }
                    else
                    {
                        info.msgBuffer = info.buffer;
                    }
                    string msgTip = Encoding.ASCII.GetString(info.msgBuffer);
                    if (msgTip == "\0\0\0faild")
                    {
                        info.isConnected = false;
                        if (this.OnDisConnected != null) OnDisConnected(info);
                        _listSocketInfo.Remove(info.socket.RemoteEndPoint.ToString());
                        info.socket.Close();
                        return;
                    }
                    if (OnReceiveMsg != null)
                        OnReceiveMsg(info);
                    //OnReceiveMsg(info.socket.RemoteEndPoint.ToString(),info.buffer);
                }
            }
            //新增的错误处理机制
            catch (Exception err)
            {
                Console.Write(err.Message);
            }
        }

        public void SendMsg(string text, string endPoint)
        {
            sendInfo info = new sendInfo() { socketIP = endPoint, sendData = Encoding.ASCII.GetBytes(text) };
            SendMsgQueue.Enqueue(info);
        }
        public int SendMsg(byte[] buff, string endPoint)
        {
            int i = 0;
            try
            {
                foreach (var item in _listSocketInfo.Keys)
                {
                    if (item.Contains(endPoint))
                    {
                        i = _listSocketInfo[item].socket.Send(buff);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(endPoint + "SendMsg:", ex);

                return 0;
            }
            return i;
            sendInfo info = new sendInfo() { socketIP = endPoint, sendData = buff };
            SendMsgQueue.Enqueue(info);
        }
        private Queue SendMsgQueue = Queue.Synchronized(new Queue());
        private void QueueSendMsgPro()
        {
            while (true)
            {
                try
                {
                    sendInfo info = SendMsgQueue.Count > 0 ? (sendInfo)SendMsgQueue.Dequeue() : null;
                    string remoteIP = "";
                    if (info != null)
                    {
                        Socket CurrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {
                            foreach (var item in _listSocketInfo.Keys)
                            {
                                if (item.Contains(info.socketIP))
                                {
                                    remoteIP = item;
                                    _listSocketInfo[item].socket.Send(info.sendData);
                                    CurrSocket = _listSocketInfo[item].socket;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //shutDownClient(remoteIP);

                            Log.Error(info.socketIP + "SendMsg:::", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("QueueSendMsgPro", ex);
                }
                //否则就发生死锁
                Thread.Sleep(5); //暂时定位200，防止下位机处理不过来。
            }
        }
        public void Stop()
        {
            // _isListening = false;
            foreach (SocketInfo s in _listSocketInfo.Values)
            {
                s.socket.Close();
            }
        }

        public void shutDownClient(string point)
        {
            try
            {
                if (_listSocketInfo.ContainsKey(point))
                {

                    lock (_listSocketInfo)
                    {
                        Socket socket = _listSocketInfo[point].socket;
                        _listSocketInfo[point].isConnected = false;
                        lock (_listSocketInfo)
                        {
                            if (this.OnDisConnected != null) OnDisConnected(_listSocketInfo[point]);
                            _listSocketInfo.Remove(point);
                            Log.Debug(point + "==========>>>主动断开连接");
                        }

                        socket.Shutdown(SocketShutdown.Both);

                        System.Threading.Thread.Sleep(10);
                        socket.Close();

                    }


                }
            }
            catch (Exception ex)
            {
                Log.Error("shutDownClientError:::" + point, ex);
            }

        }

        public class SocketInfo
        {
            public Socket socket = null;
            public byte[] buffer = null;
            public byte[] msgBuffer = null;
            public bool isConnected = false;

            public SocketInfo()
            {
                buffer = new byte[1024 * 4];
            }
        }
        public class sendInfo
        {
            public string socketIP { get; set; }
            public byte[] sendData { get; set; }
        }
    }
    public class HeartB
    {
        public string IP { get; set; }
        public int Beat { get; set; }
        public HeartB(string ip, int beat)
        {
            IP = ip;
            Beat = beat;
        }
    }



}
