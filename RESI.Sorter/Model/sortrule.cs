using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RESI.Sorter
{
    public class SortDoor
    {
        public int id { get; set; }
        public string door_code { get; set; }
        public string door_name { get; set; }
        public int door_status { get; set; }
        public double sort_time { get; set; }
        public SortRule sortRule { get; set; }
    }
    public class SortRule : INotifyPropertyChanged
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
        public int id { get; set; }
        public string rule_code { get; set; }
        public string rule_name { get; set; }
        private string _door_code;

        public string door_code
        {
            get { return _door_code; }
            set
            {
                _door_code = value;
                OnPropertyChanged("door_code");
            }
        }

        public string door_name { get; set; }
    }
}
