using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WpfApplication2
{
    public class LoadViewModel: INotifyPropertyChanged
    {
        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }

        public void Select()
        {
            List<GroupWithEditor> list = new List<GroupWithEditor>();
            list = Groups.Where(t => t.IsSelected==true).ToList();
            var array = list.ToArray();
            if (array != null && array.Length > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    var temp = array[i];
                    DataContext.AddOrUpdate(temp.ThisGroup);
                    array[i] = temp;
                }
                this.View.DialogResult = true;
                this.View.Close();
            }
        }
        public void Close()
        {
            this.View.DialogResult = false;
            this.View.Close();
        }
        public void Delete()
        {
            List<GroupWithEditor> list = new List<GroupWithEditor>();
            list = Groups.Where(t => t.IsSelected == true).ToList();
            var array = list.ToArray();
            if (array != null && array.Length > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    var temp = array[i];
                    if (DataContext.Delete(temp.ThisGroup.ID))
                    {
                        Groups = new ObservableCollection<GroupWithEditor>((DataContext.GetAll()).Select(t => new GroupWithEditor(t)));
                        MainViewModel.Show("删除成功!", this.View);
                    }
                    else
                    {
                        MainViewModel.Show("删除失败!", this.View);
                    }
                }
            }
        }
        public void KeyDownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var x=EditViewModel.GetKeyFromStr(e.Key.ToString());
            if (x != null)
            {
                SelectedGroup.ThisGroup.Key = (Keys)x;
            }
        }
        public LoadView View { get; set; }
        private ObservableCollection<GroupWithEditor> _Groups = new ObservableCollection<GroupWithEditor>();
        public ObservableCollection<GroupWithEditor> Groups
        {
            get { return _Groups; }
            set { _Groups = value; OnPropertyChanged("Groups"); }
        }
        GroupWithEditor _SelectedGroup;
        public GroupWithEditor SelectedGroup
        {
            get { return _SelectedGroup; }
            set { _SelectedGroup = value; OnPropertyChanged("SelectedGroup"); }
        }
        public LoadViewModel(LoadView view)
        {
            SelectCommand = new DelegateCommand(Select);
            CloseCommand = new DelegateCommand(Close);
            DeleteCommand = new DelegateCommand(Delete);
            View = view;
            Groups = new ObservableCollection<GroupWithEditor>((DataContext.GetAll()).Select(t => new GroupWithEditor(t)));
            if (Groups != null && Groups.Count > 0)
            {
                SelectedGroup = Groups[0];
            }
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
