using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Navigation;

namespace WpfApplication2
{
    /// <summary>
    /// MainForm.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            //WpfApplication2.DataContext.GetAll();
            //Database.SetInitializer<DataContext>(null);
            InitializeComponent();
        }
    }

    public class ReadMe: INotifyPropertyChanged
    {
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; OnPropertyChanged("IsSelected"); }
        }

        public string ImagePath { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}