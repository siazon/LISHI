using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RESI.Sorter
{
   
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
        private string _rule_code;

        public string rule_code
        {
            get { return _rule_code; }
            set { _rule_code = value;
                OnPropertyChanged("rule_code");
            }
        }

        private string _rule_name;

        public string rule_name
        {
            get { return _rule_name; }
            set
            {
                _rule_name = value;
                OnPropertyChanged("rule_name");
            }
        }

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

        public string _door_name { get; set; }

        public string door_name
        {
            get { return _door_name; }
            set { _door_name = value;
                OnPropertyChanged("door_name");
            }
        }
        private string _case_name;

        public string case_name
        {
            get { return _case_name; }
            set { _case_name = value;
                OnPropertyChanged("case_name");
            }
        }
        private int _sort_time;

        public int sort_time
        {
            get { return _sort_time; }
            set { _sort_time = value;
                OnPropertyChanged("sort_time");
            }
        }
        private bool _active=true;

        public bool active
        {
            get { return _active; }
            set { _active = value;
                OnPropertyChanged("active");
            }
        }

    }
}
