using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESI.Sorter.Model
{
   public class Record
    {
        public int id { get; set; }
        public string barcode { get; set; }
        public string door { get; set; }
        public string status { get; set; }
        public long time { get; set; }
        public string timeStr { get; set; }

    }
}
