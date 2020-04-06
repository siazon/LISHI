using Microsoft.Win32;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RESI.Sorter
{
    public class ExcelHelper
    {
        #region<<单例>>

        private static ExcelHelper instance;

        public static ExcelHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExcelHelper();
                }
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// 创建Sheet
        /// </summary>
        /// <returns></returns>
        public ISheet CreateSheet(string tableName, IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet(tableName);//获取第一个工作表
            return sheet;
        }

        /// <summary>
        /// 创建Row
        /// </summary>
        /// <returns></returns>
        public IRow CreateRow(ISheet sheet, int index)
        {
            IRow row = sheet.CreateRow(index);//获取工作表第一行
            return row;
        }

        /// <summary>
        /// 设置列值
        /// </summary>
        public void SetCellValue(IRow row, int index, object value)
        {
            ICell cell = row.CreateCell(index);//在行中添加一列
            if (value != null)
            {
                cell.SetCellValue(value.ToString());//设置列的内容
            }
            else
            {
                cell.SetCellValue("");//设置列的内容
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        public bool SaveToFile(MemoryStream ms)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Excel 文件(*.xls)|*.xls|Excel 文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";

            if (saveFile.ShowDialog() == true)
            {
                string sFilePathName = saveFile.FileName;
                SaveToFile(ms, sFilePathName);
                return true;
            }
            return false;
        }

        public bool SaveToFilePath(MemoryStream ms,string FilePath)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Excel 文件(*.xls)|*.xls|Excel 文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";
            //saveFile.InitialDirectory = FilePath;
            saveFile.FileName = FilePath;
            if (saveFile.ShowDialog() == true)
            {
                string sFilePathName = saveFile.FileName;
                SaveToFile(ms, sFilePathName);
                return true;
            }
            return false;
        }
        public bool SaveFile(MemoryStream ms, string FilePath)
        {
            //FileInfo file = new FileInfo(FilePath);
            //file.Create();
            SaveToFile(ms, FilePath);
            return false;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="fileName"></param>
        public void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }

    }
    public class ExcelUtil
    {
        #region<<单例>>

        private static ExcelUtil instance;

        public static ExcelUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExcelUtil();
                }
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// 创建Sheet
        /// </summary>
        /// <returns></returns>
        public ISheet CreateSheet(string tableName, IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet(tableName);//获取第一个工作表
            return sheet;
        }

        /// <summary>
        /// 创建Row
        /// </summary>
        /// <returns></returns>
        public IRow CreateRow(ISheet sheet, int index)
        {
            IRow row = sheet.CreateRow(index);//获取工作表第一行
            return row;
        }

        /// <summary>
        /// 设置列值
        /// </summary>
        public void SetCellValue(IRow row, int index, object value)
        {
            ICell cell = row.CreateCell(index);//在行中添加一列
            if (value != null)
            {
                cell.SetCellValue(value.ToString());//设置列的内容
            }
            else
            {
                cell.SetCellValue("");//设置列的内容
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        public bool SaveToFile(MemoryStream ms)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Excel 文件(*.xls)|*.xls|Excel 文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";

            if (saveFile.ShowDialog() == true)
            {
                string sFilePathName = saveFile.FileName;
                SaveToFile(ms, sFilePathName);
                return true;
            }
            return false;
        }

        public bool SaveToFilePath(MemoryStream ms, string FilePath)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Excel 文件(*.xls)|*.xls|Excel 文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";
            //saveFile.InitialDirectory = FilePath;
            saveFile.FileName = FilePath;
            if (saveFile.ShowDialog() == true)
            {
                string sFilePathName = saveFile.FileName;
                SaveToFile(ms, sFilePathName);
                return true;
            }
            return false;
        }
        public bool SaveFile(MemoryStream ms, string FilePath)
        {
            //FileInfo file = new FileInfo(FilePath);
            //file.Create();
            SaveToFile(ms, FilePath);
            return false;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="fileName"></param>
        public void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }




        #region  wpf客户端 导出DataGrid数据到Excel

        /// <summary>
        /// CSV格式化
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>格式化数据</returns>
        private static string FormatCsvField(string data)
        {
            return String.Format("\"{0}\"", data.Replace("\"", "\"\"\"").Replace("\n", "").Replace("\r", ""));
        }



        /// <summary>
        /// 导出DataGrid数据到Excel
        /// </summary>
        /// <param name="withHeaders">是否需要表头</param>
        /// <param name="grid">DataGrid</param>
        /// <param name="dataBind"></param>
        /// <returns>Excel内容字符串</returns>
        public static string ExportDataGrid(bool withHeaders, System.Windows.Controls.DataGrid grid, bool dataBind, int columnSortType = 1000, string SortTypeValue = "", int columnSortLine = 1000, string SortLineValue = "")
        {
            try
            {
                var strBuilder = new System.Text.StringBuilder();
                var source = (grid.ItemsSource as System.Collections.IList);
                if (source == null) return "";
                var headers = new List<string>();
                List<string> bt = new List<string>();
                int i = 0;
                foreach (var hr in grid.Columns)
                {

                    //   DataGridTextColumn textcol = hr. as DataGridTextColumn;

                    if (i == columnSortType) { headers.Add(hr.Header.ToString() + SortTypeValue); }
                    else if (i == columnSortLine) { headers.Add(hr.Header.ToString() + SortLineValue); }
                    else
                    {
                        headers.Add(hr.Header.ToString());
                    }

                    if (hr is DataGridTextColumn)//列绑定数据
                    {
                        DataGridTextColumn textcol = hr as DataGridTextColumn;
                        if (textcol != null)
                            bt.Add((textcol.Binding as Binding).Path.Path.ToString());        //获取绑定源      

                    }
                    else if (hr is DataGridTemplateColumn)
                    {
                        if (hr.Header.Equals("操作"))
                            bt.Add("Id");
                    }
                    else
                    {

                    }
                    i++;
                }
                strBuilder.Append(String.Join(",", headers.ToArray())).Append("\r\n");
                foreach (var data in source)
                {
                    var csvRow = new List<string>();
                    foreach (var ab in bt)
                    {
                        string s = ReflectionUtil.GetProperty(data, ab).ToString();
                        if (s != null)
                        {
                            csvRow.Add(FormatCsvField(s));
                        }
                        else
                        {
                            csvRow.Add("\t");
                        }
                    }
                    strBuilder.Append(String.Join(",", csvRow.ToArray())).Append("\r\n");
                    // strBuilder.Append(String.Join(",", csvRow.ToArray())).Append("\t");
                }
                return strBuilder.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 导出DataGrid数据到Excel为CVS文件
        /// 使用utf8编码 中文是乱码 改用Unicode编码
        /// 
        /// </summary>
        /// <param name="withHeaders">是否带列头</param>
        /// <param name="grid">DataGrid</param>
        public static void ExportDataGridSaveAs(string fileName, DataGrid grid, bool withHeaders = true, int columnSortType = 1000, string SortTypeValue = "", int columnSortLine = 1000, string SortLineValue = "")
        {
            try
            {
                string data = ExportDataGrid(true, grid, true, columnSortType, SortTypeValue, columnSortLine, SortLineValue);
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    DefaultExt = "csv",
                    Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    FileName = fileName
                };
                if (sfd.ShowDialog() == true)
                {
                    using (Stream stream = sfd.OpenFile())
                    {
                        using (var writer = new StreamWriter(stream, System.Text.Encoding.Unicode))
                        {

                            data = data.Replace(",", "\t");
                            writer.Write(data);
                            writer.Close();
                        }
                        stream.Close();
                    }
                    MessageBox.Show("导出成功！");
                }


            }
            catch (Exception ex)
            {
            }
        }

        #endregion 导出DataGrid数据到Excel


    }

    public class ReflectionUtil
    {
        public static object GetProperty(object obj, string propertyName)
        {
            PropertyInfo info = obj.GetType().GetProperty(propertyName);
            if (info == null && propertyName.Split('.').Count() > 0)
            {
                object o = ReflectionUtil.GetProperty(obj, propertyName.Split('.')[0]);
                int index = propertyName.IndexOf('.');
                string end = propertyName.Substring(index + 1, propertyName.Length - index - 1);
                return ReflectionUtil.GetProperty(o, end);
            }
            object result = null;
            try
            {
                result = info.GetValue(obj, null);
            }
            catch (TargetException)
            {
                return "";
            }
            return result == null ? "" : result;
        }
    }
}
