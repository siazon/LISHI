using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESI.Sorter.Model
{
    public class Sys_Para
    {

        public int id { get; set; }
        public string para_type { get; set; }
        public string para_key { get; set; }
        public string para_value { get; set; }
        public string controlType { get; set; }
    }
    public class Sys_paras
    {
        public List<Sys_Para> datas = new List<Sys_Para>();
        private string GetDay(string testDay)
        {

            for (int j = 0; j < datas.Count; j++)
            {
                if (datas[j].para_key == testDay)
                {
                    return datas[j].para_value;
                }
            }
            throw new System.ArgumentOutOfRangeException(testDay, "testDay must be in the form \"Sun\", \"Mon\", etc");
        }
        public string this[string index]   //索引器
        {
            get
            {
                return GetDay(index);
            }

        }
    }
}
