using LH.Sorter.Util.LiteDB;
using RESI.Sorter.Common;
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
    /// SettingView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingView : UserControl, INotifyPropertyChanged
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
        private ObservableCollection<SortDoor> _sortDoors = new ObservableCollection<SortDoor>();

        public ObservableCollection<SortDoor> SortDoors
        {
            get { return _sortDoors; }
            set
            {
                _sortDoors = value;
                OnPropertyChanged("SortDoors");
            }
        }
        private SortDoor sortDoor;

        public SortDoor SortDoor
        {
            get { return sortDoor; }
            set
            {
                sortDoor = value;
                OnPropertyChanged("SortDoor");
            }
        }
        private ObservableCollection<SortRule> _sortRules = new ObservableCollection<SortRule>();

        public ObservableCollection<SortRule> SortRules
        {
            get { return _sortRules; }
            set
            {
                _sortRules = value;
                OnPropertyChanged("SortRules");
            }
        }
        private SortRule sortRule;

        public SortRule SortRule
        {
            get { return sortRule; }
            set
            {
                sortRule = value;
                OnPropertyChanged("SortRule");
            }
        }
        private bool isDoorAdd = false;
        private bool isRuleAdd = false;
        public SettingView()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += SettingView_Loaded;
        }

        private void SettingView_Loaded(object sender, RoutedEventArgs e)
        {
            SortDoors.Clear();
            SortRules.Clear();
            var temp= CacheData.Ins.SortDoors = LiteDBHelper.Ins.GetCollection<SortDoor>().FindAll().ToList();
            SortDoors.AddRange(temp);
            var temprule=CacheData.Ins.SortRules = LiteDBHelper.Ins.GetCollection<SortRule>().FindAll().ToList();
            SortRules.AddRange(temprule);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnActive_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Mygrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sortDoor == null) return;
            SortRules.Clear();
            var temprule = CacheData.Ins.SortRules.FindAll(a=>a.door_id==sortDoor.door_id);
            SortRules.AddRange(temprule);
        }
        private void btnDoorAdd_Click(object sender, RoutedEventArgs e)
        {
            isDoorAdd = true;
            txtDoorTitle.Text = "新增";
            chkDoorEdit.IsChecked = !chkDoorEdit.IsChecked;
            SortDoor = new SortDoor();

        }

        private void btnDoorEdit_Click(object sender, RoutedEventArgs e)
        {
            if (SortDoor == null) return;
            isDoorAdd = false;
            txtDoorTitle.Text = "修改";
            chkDoorEdit.IsChecked = !chkDoorEdit.IsChecked;
        }

        private void btnDoorDel_Click(object sender, RoutedEventArgs e)
        {
            LiteDBHelper.Ins.Delete<SortDoor>(SortDoor.id);
            SortDoors.Remove(SortDoor);
        }
        private void btnDoorSave_Clike(object sender, RoutedEventArgs e)
        {
            if (isDoorAdd)
            { LiteDBHelper.Ins.Insert(SortDoor); SortDoors.Add(SortDoor); }
            else
                LiteDBHelper.Ins.Update(SortDoor);
            chkDoorEdit.IsChecked = false;
            SortDoors.Clear();
            var temp = CacheData.Ins.SortDoors = LiteDBHelper.Ins.GetCollection<SortDoor>().FindAll().ToList();
            SortDoors.AddRange(temp);
        }
        private void btnDoorCancel_click(object sender, RoutedEventArgs e)
        {
            if (isDoorAdd)
                SortDoors.Remove(SortDoor);
            chkDoorEdit.IsChecked = false;
        }
        private void btnRuleAdd_Click(object sender, RoutedEventArgs e)
        {
            isRuleAdd = true;
            txtRuleTitle.Text = "新增";
            chkRuleEdit.IsChecked = !chkRuleEdit.IsChecked;
            SortRule = new SortRule();
            if (sortDoor != null)
                SortRule.door_id = sortDoor.door_id;

        }

        private void btnRuleEdit_Click(object sender, RoutedEventArgs e)
        {
            if (SortRule == null) return;
            isRuleAdd = false;
            txtRuleTitle.Text = "修改";
            chkRuleEdit.IsChecked = !chkRuleEdit.IsChecked;
        }

        private void btnRuleDel_Click(object sender, RoutedEventArgs e)
        {
            LiteDBHelper.Ins.Delete<SortRule>(SortRule.id);
            SortRules.Remove(SortRule);
        }
        private void btnRuleSave_Click(object sender, RoutedEventArgs e)
        {
            if (isRuleAdd)
            {
                LiteDBHelper.Ins.Insert(SortRule);
                SortRules.Add(SortRule);
            }
            else
                LiteDBHelper.Ins.Update(SortRule);
            chkRuleEdit.IsChecked = false;
            SortRules.Clear();
            var temprule = CacheData.Ins.SortRules = LiteDBHelper.Ins.GetCollection<SortRule>().FindAll().ToList();
            SortRules.AddRange(temprule);
        }

        private void btnRuleCancel_Click(object sender, RoutedEventArgs e)
        {
            if (isRuleAdd)
                SortRules.Remove(SortRule);
            chkRuleEdit.IsChecked = false;
        }
        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExcelIn_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }


}
