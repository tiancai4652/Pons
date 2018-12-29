using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfApplication2
{
    public class HelpViewModel: INotifyPropertyChanged
    {
        public DelegateCommand NextCommand { get; private set; }
        public DelegateCommand PreviousCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public HelpViewModel(HelpView hv)
        {
            HV = hv;
        }
        public void Ini()
        {
            PreviousCommand = new DelegateCommand(Previous);
            NextCommand = new DelegateCommand(Next);
            CloseCommand = new DelegateCommand(Close);
            S1 = new ReadMe() { IsSelected = true, ImagePath = @"../bin/Debug/ico/1.png" };
            S2 = new ReadMe() { IsSelected = false, ImagePath = @"../bin/Debug/ico/2.png" };
            S3 = new ReadMe() { IsSelected = false, ImagePath = @"../bin/Debug/ico/3.png" };
            S4 = new ReadMe() { IsSelected = false, ImagePath = @"../bin/Debug/ico/4.png" };

            Files = new ObservableCollection<ReadMe>() { S1, S2, S3, S4 };
            SelectedFile = Files[0];
            Files.All(T => { T.IsSelected = false; return true; });
            SelectedFile.IsSelected = true;
        }
        public void Previous()
        {
            int index = Files.IndexOf(SelectedFile);
            if (index != 0)
            {
                SelectedFile = Files[index - 1];
                Files.All(T => { T.IsSelected = false; return true; });
                SelectedFile.IsSelected = true;
            }
        }
        public void Next()
        {
            int index = Files.IndexOf(SelectedFile);
            if (index != Files.Count - 1)
            {
                SelectedFile = Files[index + 1];
                Files.All(T => { T.IsSelected = false; return true; });
                SelectedFile.IsSelected = true;
            }
        }
        public void Close()
        {
            this.HV.Close();
        }

        #region 说明
        public HelpView HV { get; set; }
        private ReadMe _S1 = new ReadMe();
        public ReadMe S1
        {
            get { return _S1; }
            set { _S1 = value; OnPropertyChanged("S1"); }
        }
        private ReadMe _S2 = new ReadMe();
        public ReadMe S2
        {
            get { return _S2; }
            set { _S2 = value; OnPropertyChanged("S2"); }
        }
        private ReadMe _S3 = new ReadMe();
        public ReadMe S3
        {
            get { return _S3; }
            set { _S3 = value; OnPropertyChanged("S3"); }
        }
        private ReadMe _S4 = new ReadMe();
        public ReadMe S4
        {
            get { return _S4; }
            set { _S4 = value; OnPropertyChanged("S4"); }
        }
        private ObservableCollection<ReadMe> _Files = new ObservableCollection<ReadMe>() { };
        public ObservableCollection<ReadMe> Files
        {
            get { return _Files; }
            set { _Files = value; OnPropertyChanged("Files"); }
        }
        private ReadMe _SelectedFile = new ReadMe();
        public ReadMe SelectedFile
        {
            get { return _SelectedFile; }
            set { _SelectedFile = value; OnPropertyChanged("SelectedFile"); }
        }
        #endregion

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
