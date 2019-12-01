using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESI.Sorter.Model
{
    
    public class BoxInfo
    {
        public string WH_ID { get; set; }
        public string Mach_Code { get; set; } = "LISHI_01";
        public DateTime Start_Weight_Date { get; set; }
        public DateTime End_Weight_Date { get; set; }
        public string Carton_No { get; set; }
        public decimal Theory_Weight { get; set; }
        public decimal Carton_Weight { get; set; }
        public string Customer_Id { get; set; }
        public string Customer_Name { get; set; }
    }
    public class WeightInfo
    {
        public string WH_ID { get; set; }
        public string Carton_No { get; set; }
        public decimal Carton_Weight { get; set; }
        public DateTime Weight_Date { get; set; }
    }
    public class SortInfo
    {
        public string Carton_No { get; set; }
        public string Customer_Id { get; set; }
        public string Customer_Name { get; set; }
        public decimal Theory_Weight { get; set; }
        public DateTime time { get; set; }
    }
}
