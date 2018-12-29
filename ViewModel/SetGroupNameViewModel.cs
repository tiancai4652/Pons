using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfApplication2
{
    public class SetGroupNameViewModel: INotifyPropertyChanged
    {
        public DelegateCommand OKCommand { get; private set; }
        public SetGroupNameView View { get; set; }
        public SetGroupNameViewModel(SetGroupNameView view, Group thisGroup)
        {
            View = view;
            ThisGroup = thisGroup;
            OKCommand = new DelegateCommand(OK);
        }
        public void OK()
        {
            if (!string.IsNullOrEmpty(ThisGroup.Name))
            {
                this.View.Close();
            }
        }
        private Group _ThisGroup;
        public Group ThisGroup
        {
            get { return _ThisGroup; }
            set { _ThisGroup = value; OnPropertyChanged("ThisGroup"); }
        }
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
