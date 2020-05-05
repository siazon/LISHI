using LH.Sorter.Util.LiteDB;
using RESI.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESI.Sorter.Common
{
    public class CacheData
    {
        private static CacheData _ins;
        private static object obj = new object();
        public static CacheData Ins
        {
            get
            {
                lock (obj)
                {
                    if (_ins == null)
                    {
                        _ins = new CacheData();
                    }
                }
                return _ins;
            }

        }
        public CacheData()
        {
            LiteDBHelper = new LiteDBHelper();
            LiteDBHelper.InitDB("SortDB", Environment.CurrentDirectory + "/DB/");
            RecordDB = new LiteDBHelper();
            RecordDB.InitDB("RecordDB", Environment.CurrentDirectory + "/DB/");
            SortRules.Clear();
            var temprule = LiteDBHelper.GetCollection<SortRule>().FindAll().ToList();
            SortRules.AddRange(temprule);
            Sys_Paras.datas = LiteDBHelper.GetCollection<Sys_Para>().FindAll().ToList();

        }
        public List<SortRule> SortRules = new List<SortRule>();
        public Sys_paras Sys_Paras { get; set; } = new Sys_paras();
        public LiteDBHelper LiteDBHelper { get; set; }
        public LiteDBHelper RecordDB { get; set; }
    }
}
