using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESI.Sorter.Model
{
    public class SortInfo
    {
        public string WH_ID { get; set; }
        public string Mach_Code { get; set; }
        public DateTime Start_Weight_Date { get; set; }
        public DateTime End_Weight_Date { get; set; }
        public string Carton_No { get; set; }
        public double Theory_Weight { get; set; }
        public double Carton_Weight { get; set; }
        public string Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public string So_Id { get; set; }
        public string External_Order_Id { get; set; }
        public string Barcode { get; set; }
        public string Sku_Udf1 { get; set; }
        public string Sku_Name2 { get; set; }
        public double Packing_Qty { get; set; }
        public double Unit_Price { get; set; }
        public double Total_Price { get; set; }
        public double Gross_Wgt { get; set; }
        public string Remark { get; set; }

    }
}

