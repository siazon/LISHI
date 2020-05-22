using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESI.Sorter.Model
{
    public class BaseResult
    {
        public string code { get; set; }
        public string msg { get; set; }
        public List<ResultData> date { get; set; }
    }
    public class ResultData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Carton_No { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Customer_Id { get; set; }
        /// <summary>
        /// 商務文化快線(美孚)
        /// </summary>
        public string Customer_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Theory_Weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DetailItem> Detail { get; set; }
    }
    public class DetailItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string So_Id { get; set; }
        /// <summary>
        /// 称重测试
        /// </summary>
        public string External_Order_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Sku_Udf1 { get; set; }
        /// <summary>
        /// LEDERER BSP005BL 護照套(Blue)
        /// </summary>
        public string Sku_Name2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Packing_Qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Unit_Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Total_Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Gross_Wgt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
    }
}
