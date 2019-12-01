using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : UserControl, INotifyPropertyChanged
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
        private ObservableCollection<Record> _Records = new ObservableCollection<Record>();

        public ObservableCollection<Record> Records
        {
            get { return _Records; }
            set
            {
                _Records = value;
                OnPropertyChanged("Records");
            }
        }
        public MainView()
        {
            InitializeComponent();
            this.DataContext = this;
            RunFlowDoc.Blocks.Add(Runparagraph);
            Connectlist.Document = RunFlowDoc;
            this.Loaded += MainView_Loaded; AddLine("服务器连接失败", 2, true, true);
            AddLine("PLC连接失败", 2, true, true);
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
         
        }

        public FlowDocument RunFlowDoc = new FlowDocument();
        public Paragraph Runparagraph = new Paragraph();
        private void AddLine(string test)
        {
            Connectlist.AppendText(test);
            Connectlist.AppendText("\r");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="type">0：兰色 1：黑色 2：红色</param>
        /// <param name="withTime"></param>
        /// <param name="isEnter"></param>
        public  void AddLine(string content, int type, bool withTime = false, bool isEnter = true)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    string time = "";
                    if (withTime)
                        time = DateTime.Now.ToLongString() + ">>";
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
