using LH.Sorter.Util.LiteDB;
using RESI.Sorter.Common;
using RESI.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var temo = CacheData.Ins.Sys_Paras["IsUpLoad"];
            SwIsUpLoad.IsChecked = temo == "1";
            txtBoxInfo.Text = CacheData.Ins.Sys_Paras["BoxInfo"];
            txtDoorSortingTime.Text = CacheData.Ins.Sys_Paras["SortingTime"];
            txtScale.Text = CacheData.Ins.Sys_Paras["ScaleIP"];
            txtScan.Text = CacheData.Ins.Sys_Paras["ScanIP"];
            txtSorter.Text = CacheData.Ins.Sys_Paras["c"];
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (txtBoxInfo.Text.Length == 0)
            {
                MessageBox.Show("箱重量为空");
                return;
            }
            string[] list = txtBoxInfo.Text.Split(';');
            if (list.Length == 0 || !txtBoxInfo.Text.Contains(";") || !txtBoxInfo.Text.Contains('='))
            {
                MessageBox.Show(txtBoxInfo.Tag + ":输入值错误");
                return;
            }
            list = txtDoorSortingTime.Text.Split(';');
            if (txtDoorSortingTime.Text.Length == 0 || list.Length == 0 || !txtDoorSortingTime.Text.Contains(";") || !txtDoorSortingTime.Text.Contains('='))
            {
                MessageBox.Show(txtDoorSortingTime.Tag + ":输入值错误");
                return;
            }
            if (!CheckAddressPort(txtScale.Text))
            {
                MessageBox.Show(txtScale.Tag + ":输入值错误");
                return;
            }
            if (!CheckAddressPort(txtScan.Text))
            {
                MessageBox.Show(txtScan.Tag + ":输入值错误");
                return;
            }
            if (!CheckAddressPort(txtSorter.Text))
            {
                MessageBox.Show(txtSorter.Tag + ":输入值错误");
                return;
            }
            var para = CacheData.Ins.Sys_Paras.datas.FirstOrDefault(a => a.para_key == "IsUpLoad");
            if (para != null)
            {
                para.para_value = SwIsUpLoad.IsChecked == true ? "1" : "0";
                CacheData.Ins.LiteDBHelper.Update(para);
            }
            para = CacheData.Ins.Sys_Paras.datas.FirstOrDefault(a => a.para_key == "BoxInfo");
            if (para != null)
            {
                para.para_value = txtBoxInfo.Text;
                CacheData.Ins.LiteDBHelper.Update(para);
            }
            para = CacheData.Ins.Sys_Paras.datas.FirstOrDefault(a => a.para_key == "SortingTime");
            if (para != null)
            {
                para.para_value = txtDoorSortingTime.Text;
                CacheData.Ins.LiteDBHelper.Update(para);
            }
            para = CacheData.Ins.Sys_Paras.datas.FirstOrDefault(a => a.para_key == "ScaleIP");
            if (para != null)
            {
                para.para_value = txtScale.Text;
                CacheData.Ins.LiteDBHelper.Update(para);
            }
            para = CacheData.Ins.Sys_Paras.datas.FirstOrDefault(a => a.para_key == "ScanIP");
            if (para != null)
            {
                para.para_value = txtScan.Text;
                CacheData.Ins.LiteDBHelper.Update(para);
            }
            para = CacheData.Ins.Sys_Paras.datas.FirstOrDefault(a => a.para_key == "SortIP");
            if (para != null)
            {
                para.para_value = txtSorter.Text;
                CacheData.Ins.LiteDBHelper.Update(para);
            }

        }
        private bool CheckAddressPort(string s)
        {
            bool isLegal = false;
            Regex regex = new Regex(@"^((2[0-4]\d|25[0-5]|[1]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[1]?\d\d?)\:([1-9]|[1-9][0-9]|[1-9][0-9][0-9]|[1-9][0-9][0-9][0-9]|[1-6][0-5][0-5][0-3][0-5])$");//CheckAddressPort
            Match match = regex.Match(s);
            //Match match = regex.Match("192.168.1.2:33333"); //可以测试其他ip和端口
            if (match.Success)
            {
                isLegal = true;
            }
            else
            {
                isLegal = false;
            }
            return isLegal;
        }
    }
}
