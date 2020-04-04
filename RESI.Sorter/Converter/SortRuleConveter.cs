using RESI.Sorter.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RESI.Sorter
{
    public class SortRuleConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "缺省";
            }
            var temp = CacheData.Ins.SortDoors.FirstOrDefault(a => a.door_code == value.ToString());
            if (temp == null)
                return "缺省";
            else
                return temp.door_name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
