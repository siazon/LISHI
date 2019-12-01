using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RESI.Sorter
{
    public class HttpUtility
    {
        public HttpUtility()
        {

        }
        private static HttpUtility _instance = null;
        public static HttpUtility Ins
        {
            get { if (_instance == null) { _instance = new HttpUtility(); } return _instance; }
        }
        public Task<string> PostAsync(string postUrl, string paramData, bool hasHeaders = false)
        {
            return Task.Factory.StartNew(() =>
            {

                return Post(postUrl, paramData, hasHeaders);
            });
        }
        private static object obj = new object();
        public string Post(string postUrl, string paramData, bool hasHeaders = false)
        {
            HttpWebRequest webReq = null;
            HttpWebResponse response = null;
            Stream newStream = null;
            Stream resStream = null;
            StreamReader sr = null;

            try
            {
                string ret = string.Empty;
                lock (obj)
                {
                    Encoding dataEncode = System.Text.Encoding.GetEncoding("utf-8");
                    //将URL编码后的字符串转化为字节
                    byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                    webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                    ServicePointManager.DefaultConnectionLimit = 512;
                    ServicePointManager.Expect100Continue = false;
                    webReq.Proxy = null;
                    webReq.KeepAlive = false;
                    //Post请求方式
                    webReq.Method = "POST";
                    if (hasHeaders)
                    {
                        webReq.Headers.Add(HttpRequestHeader.Authorization, "2D12CFF31185C46693C3EA4F0A8647CF");
                        SetHeaderValue(webReq.Headers, "RegisterNo", "902");
                        SetHeaderValue(webReq.Headers, "Date", DateTime.Now.ToString("yyyy-MM-dd HH:m:ss"));
                    }
                    // 内容类型
                    webReq.ContentType = "application/json";
                    webReq.Timeout = 1500;
                    //设置请求的 ContentLength 
                    webReq.ContentLength = byteArray.Length; //获得请 求流
                    newStream = webReq.GetRequestStream();
                    //将请求参数写入流
                    newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                                                                    // 关闭请求流
                    newStream.Close();
                    newStream = null;
                    // 获得响应流
                    response = (HttpWebResponse)webReq.GetResponse();
                    resStream = response.GetResponseStream();
                    sr = new StreamReader(resStream, System.Text.Encoding.GetEncoding("utf-8"));
                    ret = sr.ReadToEnd();
                    sr.Close();
                    sr = null;
                    resStream.Close();
                    resStream = null;
                    response.Close();
                    response = null;
                    webReq.Abort();
                    webReq = null;

                }
                return ret;
            }
            catch (Exception ex)
            {
                if (newStream != null) { newStream.Close(); newStream = null; }
                if (resStream != null) { resStream.Close(); resStream = null; }

                if (response != null) { response.Close(); response = null; }

                if (webReq != null) { webReq.Abort(); webReq = null; }

                Log.Error("Http Err", ex);
                return "TIMEOUT";
            }

        }
        /// <summary>
        /// 添加自定义Header
        /// </summary>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
        public bool isConnet(string Host)
        {
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                options.DontFragment = true;

                PingReply reply = pingSender.Send(Host, 10000);
                if (reply.Status == IPStatus.Success || reply.Status == IPStatus.TimedOut)
                {
                    return true;
                }
                else
                {

                }
                {

                }
                return false;


                //Ping p1 = new Ping();

                //PingReply reply = p1.Send(Host, 10000); //发送主机名或Ip地址
                //if (reply.Status == IPStatus.Success)
                //{
                //    return true;
                //}
                //else if (reply.Status == IPStatus.TimedOut)
                //{
                //    Console.WriteLine("超时");
                //    return false;
                //}
                //else
                //{
                //    Console.WriteLine("失败");
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
