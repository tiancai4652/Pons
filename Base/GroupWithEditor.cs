using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfApplication2
{
    public class GroupWithEditor: INotifyPropertyChanged
    {
        private Group _ThisGroup;
        public Group ThisGroup
        {
            get { return _ThisGroup; }
            set { _ThisGroup = value; OnPropertyChanged("ThisGroup"); }
        }
        private bool _IsEdited = false;
        public bool IsEdited
        {
            get { return _IsEdited; }
            set { _IsEdited = value; OnPropertyChanged("IsEdited"); }
        }
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; OnPropertyChanged("IsSelected"); }
        }
        public static Group ConVerToGroup(GroupWithEditor gwe)
        {
            return gwe.ThisGroup;
        }

        public GroupWithEditor(Group gp)
        {
            ThisGroup = gp;
            IsEdited = false;
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
