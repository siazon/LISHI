using LH.Sorter.Util.LiteDB;
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

            SortDoors.Clear();
            SortRules.Clear();
            var temp = LiteDBHelper.Ins.GetCollection<SortDoor>().FindAll().ToList();
            SortDoors.AddRange(temp);
            var temprule = LiteDBHelper.Ins.GetCollection<SortRule>().FindAll().ToList();
            SortRules.AddRange(temprule);
        }
        public List<SortDoor> SortDoors = new List<SortDoor>();
        public List<SortRule> SortRules = new List<SortRule>();
    }
}
