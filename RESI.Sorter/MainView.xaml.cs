﻿using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RESI.Sorter
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : UserControl, INotifyPropertyChanged
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
        private ObservableCollection<Record> _Records = new ObservableCollection<Record>();

        public ObservableCollection<Record> Records
        {
            get { return _Records; }
            set
            {
                _Records = value;
                OnPropertyChanged("Records");
            }
        }
        public MainView()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += MainView_Loaded; 
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
         
        }

   
    }
}
