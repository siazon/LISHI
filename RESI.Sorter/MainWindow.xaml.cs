using LH.Sorter.Util.LiteDB;
using RESI.Sorter.Model;
using Season.SVT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RESI.Sorter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
        public ContorllerItem[] Items { get; }
        private SocketManager TcpServer;
        Dictionary<string, string> dicTcp = new Dictionary<string, string>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            RunFlowDoc.Blocks.Add(Runparagraph);
            Connectlist.Document = RunFlowDoc;
            LiteDBHelper.Ins.InitDB("DoorDB", Environment.CurrentDirectory + "/");
            brush = FindResource("AccentColorBrush") as SolidColorBrush;
            TcpServer = new SocketManager(4000);
            TcpServer.OnReceiveMsg += TcpServer_OnReceiveMsg;
            TcpServer.OnConnected += TcpServer_OnConnected;
            TcpServer.OnDisConnected += TcpServer_OnDisConnected;
            TcpServer.Start();
            AddLine("分拣机构连接中...",1);
            Items = new[]
            {
                new ContorllerItem("主页",new MainView()),
                new ContorllerItem("查询",new QueryView()),
                new ContorllerItem("设置",new SettingMainView()),
            };
            Task.Run(() =>
            {
                //DateTime LastLoadTime = DateTime.Now;
                //try
                //{
                //    List<SortInfo> CC = LiteDBHelper.Ins.GetCollection<SortInfo>().Max(a=>a.time);// MySqlUitity.Ins.Query<code_city>("SELECT * from beteng ORDER BY time DESC LIMIT 1");
                //    DateTime Stime = LastLoadTime.AddDays(-1);
                //    if (CC.Count > 0)
                //    {
                //        Stime = CC[0].time;
                //    }
                //    GetList(Stime, LastLoadTime);
                //}
                //catch (Exception ex)
                //{
                //    Log.Error("Fetch List", ex);
                //}
                //while (true)
                //{
                //    try
                //    {
                //        Thread.Sleep(300000);
                //        DateTime now = DateTime.Now;
                //        bool succeed = GetList(LastLoadTime, now);
                //        if (succeed)
                //            LastLoadTime = now;

                //        if (DateTime.Now.Hour == 15)
                //        {
                //            LiteDBHelper.Ins.Delete<SortInfo>(new LiteDB.BsonValue() { });
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Log.Error("LoadTime", ex);
                //    }
                //}
            });
        }

        private void TcpServer_OnDisConnected(SocketManager.SocketInfo socketInfo)
        {
            string remote = socketInfo.socket.RemoteEndPoint.ToString();
            if (dicTcp.Values.Contains(remote))
                dicTcp.Remove(dicTcp.FirstOrDefault(a => a.Value == remote).Key);
        }

        private void TcpServer_OnConnected(SocketManager.SocketInfo socketInfo)
        {
            string remote = socketInfo.socket.RemoteEndPoint.ToString();
            if (remote == "192.168.3.11")
            {
                if (dicTcp.Keys.Contains("SCAN"))
                    dicTcp["SCAN"] = remote;
                else
                    dicTcp.Add("SCAN", remote);
            }
            AddLine("分拣机械连接成功",0);
        }

        private void TcpServer_OnReceiveMsg(SocketManager.SocketInfo socketInfo)
        {

        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ColorZone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                return;
            }
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                return;
            }
        }

        SolidColorBrush brush = new SolidColorBrush(Colors.White);
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ItemListBox.SelectedIndex = 0;
            lbMain.Background = new SolidColorBrush(Colors.White);
            lbMain.Foreground = new SolidColorBrush(Colors.Black);
            lbQeury.Background = brush;
            lbQeury.Foreground = new SolidColorBrush(Colors.White);
            lbSetting.Background = brush;
            lbSetting.Foreground = new SolidColorBrush(Colors.White);
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            ItemListBox.SelectedIndex = 1;
            lbMain.Background = brush;
            lbMain.Foreground = new SolidColorBrush(Colors.White);
            lbQeury.Background = new SolidColorBrush(Colors.White);
            lbQeury.Foreground = new SolidColorBrush(Colors.Black);
            lbSetting.Background = brush;
            lbSetting.Foreground = new SolidColorBrush(Colors.White);
        }
        private void Label_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            ItemListBox.SelectedIndex = 2;
            lbMain.Background = brush;
            lbMain.Foreground = new SolidColorBrush(Colors.White);
            lbQeury.Background = brush;
            lbQeury.Foreground = new SolidColorBrush(Colors.White);
            lbSetting.Background = new SolidColorBrush(Colors.White);
            lbSetting.Foreground = new SolidColorBrush(Colors.Black);
        }
        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void CloseWindow_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GetList()
        {

        }
        public FlowDocument RunFlowDoc = new FlowDocument();
        public Paragraph Runparagraph = new Paragraph();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type">0：兰色 1：黑色 2：红色</param>
        /// <param name="withTime"></param>
        /// <param name="isEnter"></param>
        public void AddLine(string content, int type, bool withTime = true, bool isEnter = true)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    string time = "";
                    if (withTime)
                        time = DateTime.Now.ToString("HH:mm:ss.fff") + ">>";
                    Run r = new Run(time + content);
                    r.FontSize = 18;
                    if (type == 0)
                        r.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
                    else if (type == 1)
                        r.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    else
                        r.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                    Runparagraph.Inlines.Add(r);
                    if (isEnter)
                        Runparagraph.Inlines.Add("\r");
                    Connectlist.ScrollToEnd();
                    int count = Runparagraph.Inlines.Count;
                    if (count > 200)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            Runparagraph.Inlines.Remove(Runparagraph.Inlines.ElementAt(0));
                        }
                    }
                });
            }
            catch (Exception ex)
            {
            }
        }
    }
}
