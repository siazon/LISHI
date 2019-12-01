using System;
using System.Collections.Generic;
using System.Linq;
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
    /// QueryView.xaml 的交互逻辑
    /// </summary>
    public partial class QueryView : UserControl
    {
        public QueryView()
        {
            InitializeComponent();

            sdate.SelectedDate = DateTime.Now.Date;
            stime.SelectedTime = DateTime.Now.Date;
            edate.SelectedDate = DateTime.Now;
            etime.SelectedTime = DateTime.Now;
        }

        private void Search_OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
