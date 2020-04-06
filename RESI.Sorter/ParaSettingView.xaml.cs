using LH.Sorter.Util.LiteDB;
using RESI.Sorter.Common;
using RESI.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// ParaSettingView.xaml 的交互逻辑
    /// </summary>
    public partial class ParaSettingView : UserControl
    {
        public ParaSettingView()
        {
            InitializeComponent();
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
           
            Sys_Para paras = new Sys_Para();
            paras.para_type = "para";
            paras.para_key = "上传重量信息开关";
            paras.para_value = "1";
            paras.controlType = "Switch";
            CacheData.Ins.LiteDBHelper.Insert(paras);

        }
    }
}
