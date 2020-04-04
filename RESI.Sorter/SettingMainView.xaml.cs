using Season.SVT;
using System;
using System.Collections.Generic;
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
    /// SettingMainView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingMainView : UserControl, INotifyPropertyChanged
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
        public SettingMainView()
        {
            InitializeComponent();
            this.DataContext = this;
            Items = new[]
          {
                new ContorllerItem("设置",new SortRuleView()),
                new ContorllerItem("系统参数",new ParaSettingView()),
            };
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ColorZone_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
