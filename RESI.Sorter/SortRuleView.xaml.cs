using LH.Sorter.Util.LiteDB;
using LiteDB;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using RESI.Sorter.Common;
using RESI.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
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
    public partial class SortRuleView : UserControl, INotifyPropertyChanged
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
        private bool isRuleAdd = false;
        public SortRuleView()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += SettingView_Loaded;
        }

        private void SettingView_Loaded(object sender, RoutedEventArgs e)
        {
            var para = CacheData.Ins.LiteDBHelper.GetCollection<Sys_Para>().FindOne(Query.EQ("para_key", "case_name"));
            if (para != null)
            {
                CaseName = para;
            }
            RefreshData();

            RefreshCaseName();
        }
        Sys_Para CaseName = null;

        private void btnActive_Click(object sender, RoutedEventArgs e)
        {
            var temp = cmbcase.SelectedItem.ToString();
            if (CaseName != null)
            {
                CaseName.para_value = temp;
                CacheData.Ins.LiteDBHelper.Update<Sys_Para>(CaseName);
                SortRules.ForEach(a => { if (a.case_name == CaseName.para_value) a.active = true; else a.active = false; });
                CacheData.Ins.LiteDBHelper.Update<SortRule>(SortRules.ToList());
            }
            RefreshData();
        }


        #region Rule

        private void btnRuleAdd_Click(object sender, RoutedEventArgs e)
        {
            isRuleAdd = true;
            txtRuleTitle.Text = "新增";
            chkRuleEdit.IsChecked = !chkRuleEdit.IsChecked;
            if (SortRule == null)
            {
                SortRule = new SortRule();
            }
            else
            {
                SortRule rule = SortRule;
                SortRule = new SortRule();
                SortRule.case_name = rule.case_name;
                SortRule.door_code = rule.door_code;
                sortRule.door_name = rule.door_name;

            }

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
            if (SortRule == null) return;
            CacheData.Ins.LiteDBHelper.Delete<SortRule>(SortRule.id);
            SortRules.Remove(SortRule);
            CacheData.Ins.SortRules.Remove(SortRule);
        }
        private void btnRuleSave_Click(object sender, RoutedEventArgs e)
        {
            if (isRuleAdd)
            {
                CacheData.Ins.LiteDBHelper.Insert(SortRule);
            }
            else
                CacheData.Ins.LiteDBHelper.Update(SortRule);
            chkRuleEdit.IsChecked = false;

            RefreshCaseName();
            var temp = cmbcase.SelectedItem.ToString();
            if (CaseName != null)
            {
                CaseName.para_value = temp;
                CacheData.Ins.LiteDBHelper.Update<Sys_Para>(CaseName);
                SortRules.ForEach(a => { if (a.case_name == CaseName.para_value) a.active = true; else a.active = false; });
                CacheData.Ins.LiteDBHelper.Update<SortRule>(SortRules.ToList());
            }
            RefreshData();
        }
        private void RefreshCaseName()
        {
            cmbcase.Items.Clear();
            var temp = SortRules.ToList().GroupBy(a => a.case_name);
            foreach (var item in temp)
            {
                cmbcase.Items.Add(item.Key);
            }
            foreach (var item in cmbcase.Items)
            {
                if (item.ToString() == CaseName.para_value)
                {
                    cmbcase.SelectedItem = item;
                }
            }
        }
        private void RefreshData()
        {
            SortRules.Clear();
            var temprule = CacheData.Ins.LiteDBHelper.GetCollection<SortRule>().Find(Query.EQ("case_name", CaseName.para_value)).ToList();
            CacheData.Ins.SortRules = (temprule);
            SortRules.AddRange(temprule);
            temprule = CacheData.Ins.LiteDBHelper.GetCollection<SortRule>().Find(Query.Not("case_name", CaseName.para_value)).ToList();
            SortRules.AddRange(temprule);
        }
        private void btnRuleCancel_Click(object sender, RoutedEventArgs e)
        {
            if (isRuleAdd)
                SortRules.Remove(SortRule);
            chkRuleEdit.IsChecked = false;
        }

        #endregion
        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            IWorkbook workbook = Export();
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            ExcelHelper.Instance.SaveToFilePath(stream, "分拣方案");
        }

        private void btnExcelIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "选择文件";
                openFileDialog.Filter = "所有文件|*.*";
                openFileDialog.FileName = string.Empty;
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.DefaultExt = "*.*";
                var result = openFileDialog.ShowDialog();
                if (result == false)
                {
                    return;
                }
                string fileName = openFileDialog.FileName;
                DataTable dt = ExcelToDataTable(fileName, "分拣方案模板", true);
                List<SortRule> SRs = new List<SortRule>();
                SortRule SR;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SR = new SortRule();
                    SR.case_name = dt.Rows[i][0].ToString();
                    SR.door_code = dt.Rows[i][1].ToString();
                    SR.door_name = dt.Rows[i][2].ToString();
                    SR.rule_code = dt.Rows[i][3].ToString();
                    SR.rule_name = dt.Rows[i][4].ToString();
                    SRs.Add(SR);
                }
                var temp = SRs.ToList().GroupBy(a => a.case_name);
                foreach (var item in temp)
                {
                    CacheData.Ins.LiteDBHelper.Delete<SortRule>(Query.EQ("case_name", item.Key));
                }
                int res = CacheData.Ins.LiteDBHelper.Insert(SRs);
                if (res > 0)
                {

                    SortRules.Clear();
                    var temprule = CacheData.Ins.SortRules = CacheData.Ins.LiteDBHelper.GetCollection<SortRule>().FindAll().ToList();
                    SortRules.AddRange(temprule);
                    cmbcase.Items.Clear();
                    var cases = SortRules.ToList().GroupBy(a => a.case_name);
                    foreach (var item in cases)
                    {
                        cmbcase.Items.Add(item.Key);
                    }
                    MessageBox.Show("导入成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败：导入前请确保EXCEL文件已经保存并关闭");

            }
        }
        public DataTable ExcelToDataTable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, FileAccess.Read);
                IWorkbook workbook = new HSSFWorkbook(fs);// WorkbookFactory.Create(fs);
                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号，即总的列数
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;//得到项标题后
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)//打印错误信息
            {
                MessageBox.Show("Exception: " + ex.Message);
                return null;
            }
        }


        public IWorkbook Export()
        {
            IWorkbook workbook = new HSSFWorkbook();
            try
            {
                var sheet = ExcelHelper.Instance.CreateSheet("分拣方案模板", workbook);
                var HeaderRow = ExcelHelper.Instance.CreateRow(sheet, 0);
                ExcelUtil.Instance.SetCellValue(HeaderRow, 0, "方案名称");
                ExcelUtil.Instance.SetCellValue(HeaderRow, 1, "分拣口代码");
                ExcelUtil.Instance.SetCellValue(HeaderRow, 2, "分拣口名称");
                ExcelUtil.Instance.SetCellValue(HeaderRow, 3, "站点代码");
                ExcelUtil.Instance.SetCellValue(HeaderRow, 4, "站点名称");
                List<SortRule> datalist = new List<SortRule>();
                if (SortRules == null || SortRules.Count == 0)
                {
                    SortRule sr = new SortRule()
                    {
                        case_name = "方案A",
                        door_code = "1",
                        door_name = "一号口",
                        rule_code = "a01",
                        rule_name = "广州"
                    };
                    datalist.Add(sr);
                    sr = new SortRule()
                    {
                        case_name = "方案A",
                        door_code = "1",
                        door_name = "一号口",
                        rule_code = "a02",
                        rule_name = "深圳"
                    };
                    datalist.Add(sr);
                    sr = new SortRule()
                    {
                        case_name = "方案A",
                        door_code = "2",
                        door_name = "二号口",
                        rule_code = "a01",
                        rule_name = "广州"
                    };
                    datalist.Add(sr);
                    sr = new SortRule()
                    {
                        case_name = "方案A",
                        door_code = "2",
                        door_name = "二号口",
                        rule_code = "a02",
                        rule_name = "深圳"
                    };
                    datalist.Add(sr);
                    sr = new SortRule()
                    {
                        case_name = "方案B",
                        door_code = "1",
                        door_name = "一号口",
                        rule_code = "a02",
                        rule_name = "深圳"
                    };
                    datalist.Add(sr);

                }
                else datalist = SortRules.ToList();
                if (datalist != null && datalist.Count() > 0)
                {
                    int index = 1;
                    foreach (var item in datalist)
                    {
                        var dataRow = ExcelHelper.Instance.CreateRow(sheet, index);
                        index++;
                        ExcelHelper.Instance.SetCellValue(dataRow, 0, item.case_name);
                        ExcelHelper.Instance.SetCellValue(dataRow, 1, item.door_code);
                        ExcelHelper.Instance.SetCellValue(dataRow, 2, item.door_name);
                        ExcelHelper.Instance.SetCellValue(dataRow, 3, item.rule_code);
                        ExcelHelper.Instance.SetCellValue(dataRow, 4, item.rule_name);

                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error("Excel", ex);
                //记录日志
            }
            return workbook;
        }
    }


}
