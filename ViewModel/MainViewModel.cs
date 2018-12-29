using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;

namespace WpfApplication2
{
    public class MainViewModel: INotifyPropertyChanged
    {
      


        public MainView View { get; set; }
        public DelegateCommand AddGroupCommand { get; private set; }
        public DelegateCommand RemoveGroupCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }
        public DelegateCommand ExcuteCmdCommand { get; private set; }
        public DelegateCommand NextCommand { get; private set; }
        public DelegateCommand PreviousCommand { get; private set; }
        public DelegateCommand HelpCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand InstallSDCommand { get; private set; }
        public DelegateCommand AlwaysClickCommand { get; private set; }
        
        public void InstallSD()
        {

        }
        
        public void Save()
        {
            if (SelectGroup != null&& SelectGroup.Key!=Keys.None)
            {
                SelectGroup.Name = DialogManager.ShowModalInputExternal(this.View, "请命名", "", null);

                if (string.IsNullOrEmpty(SelectGroup.Name))
                {
                    Show("保存失败", this.View);
                    return;
                }
                try
                {
                    if (DataContext.AddOrUpdate(SelectGroup))
                    {
                        Show( "保存成功", this.View);
                    }
                    else
                    {
                        Show( "保存失败", this.View);
                    }
                }
                catch (Exception ex)
                { }
            }
        }
        public static void Show(string title, MetroWindow window)
        {
            MetroDialogSettings MetroDialogOptions = new MetroDialogSettings();
            MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "OK",
                NegativeButtonText = "Cancel",
                DefaultText = "",
                FirstAuxiliaryButtonText = "1",
                SecondAuxiliaryButtonText = "2",
                ColorScheme = MetroDialogColorScheme.Accented,
                AnimateShow = false,
                AnimateHide = true,
            };
            MessageDialogResult result = DialogManager.ShowModalMessageExternal(window, title, "", MessageDialogStyle.Affirmative, mySettings);
        }
        public void Load()
        {
            LoadView lv = new LoadView();
            lv.ShowDialog();
            if (lv.DialogResult == true)
            {
                var x = (lv.DataContext as LoadViewModel).Groups.Where(t => t.IsSelected == true).ToList();
                if (x != null && x.Count > 0)
                {
                    this.Groups = new ObservableCollection<Group>(x.Select(t => t.ThisGroup));
                    //this.SelectGroup = Groups[0];
                }
            }
        }
        ObservableCollection<Group> _Groups = new ObservableCollection<Group>();
        public ObservableCollection<Group> Groups
        {
            get { return _Groups; }
            set
            {
                _Groups = value; OnPropertyChanged("Groups");
                if (RemoveGroupCommand != null)
                {
                    RemoveGroupCommand.CanExecute(null);
                }
                if (EditCommand != null)
                {
                    EditCommand.CanExecute(null);
                }
                if (ExcuteCmdCommand != null)
                {
                    ExcuteCmdCommand.CanExecute(null);
                }
                if (SaveCommand != null)
                {
                    SaveCommand.CanExecute(null);
                }



            }
        }
        Group _SelectGroup = new Group();
        public Group SelectGroup
        {
            get { return _SelectGroup; }
            set { _SelectGroup = value; OnPropertyChanged("SelectGroup"); }
        }
        public void AddGroup()
        {
            Group temp = new Group();
            var tempList = Groups;
            tempList.Add(temp);
            Groups = tempList;
            //SelectGroup = temp;
        }
        public void RemoveGroup()
        {
            Groups.Remove(SelectGroup);
            Groups = Groups;
            if (Groups.Count != 0)
            {
                //SelectGroup = Groups[0];
            }
        }
        public void Edit()
        {
            if (SelectGroup == null|| SelectGroup.Key==Keys.None)
            { return; }

            if (SelectGroup.Commands == null)
            {
                SelectGroup.Commands = new ObservableCollection<JYCommand>();
            }
            EditView ev = new EditView(SelectGroup);
            ev.ShowDialog();
            SelectGroup.GetMyDescribe();
        }
        public void ExcuteCmd()
        {
            if (Groups != null && Groups.Count > 0)
            {
                this.View.ShowInTaskbar = false;
                this.View.Hide();

                //var list = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Group>>(Newtonsoft.Json.JsonConvert.SerializeObject(Groups));
                var list = Groups;
                if (IsCycle)
                {
                    list.All(t => t.Commands.All(x => { x.KeyValue.ExcuteTimes = 9999;return true; }));
                }
                RunForm rf = new RunForm(list, this.View, IsChooseDD);
                rf.Show();
                //this.View.Show();
            }
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
        public void Help()
        {
            HelpView hv = new HelpView();
            hv.ShowDialog();
        }
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        const int MOUSEEVENTF_LEFTDOWN = 0x0002; //模拟鼠标左键按下
        const int MOUSEEVENTF_LEFTUP = 0x0004; //模拟鼠标左键抬起
        public void AlwaysClick()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = System.Environment.CurrentDirectory + @"\AlwaysClick\AlwysClick.exe";
            p.Start();
        }
        public MainViewModel(MainView view)
        {
            View = view;
            AddGroupCommand = new DelegateCommand(AddGroup);
            RemoveGroupCommand = new DelegateCommand(RemoveGroup);
            EditCommand = new DelegateCommand(Edit, () => IsValid());
            ExcuteCmdCommand = new DelegateCommand(ExcuteCmd, () => IsValid());
            PreviousCommand = new DelegateCommand(Previous);
            NextCommand = new DelegateCommand(Next);
            HelpCommand = new DelegateCommand(Help);
            SaveCommand = new DelegateCommand(Save, ()=>IsValid());
            LoadCommand = new DelegateCommand(Load);
            AlwaysClickCommand = new DelegateCommand(AlwaysClick);
            InstallSDCommand = new DelegateCommand(InstallSD);
            S1 = new ReadMe() { IsSelected = true, ImagePath = @"./bin/Debug/ico/1.png" };
            S2 = new ReadMe() { IsSelected = false, ImagePath = @"./bin/Debug/ico/2.png" };
            S3 = new ReadMe() { IsSelected = false, ImagePath = @"./bin/Debug/ico/3.png" };
            S4 = new ReadMe() { IsSelected = false, ImagePath = @"./bin/Debug/ico/4.png" };
            Files = new ObservableCollection<ReadMe>() { S1, S2, S3, S4 };
            SelectedFile = Files[0];
            Files.All(T => { T.IsSelected = false; return true; });
            SelectedFile.IsSelected = true;
        }

        #region 说明

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

        public void KeyDownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (SelectGroup != null)
            {
                SelectGroup.Key =(Keys)EditViewModel.GetKeyFromStr(e.Key.ToString());
                (sender as System.Windows.Controls.TextBox).Text = SelectGroup.Key.ToString();
            }
        }
        private bool _IsShowHelp = false;
        public bool IsShowHelp
        {
            get { return _IsShowHelp; }
            set { _IsShowHelp = value; OnPropertyChanged("IsShowHelp"); }
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
        bool IsValid()
        {
            if (Groups == null|| Groups.Count==0)
            {
                return false;
            }
            return true;
        }

        #region 循环
        bool _IsCycle = false;
        public bool IsCycle
        {
            get { return _IsCycle; }
            set { _IsCycle = value; OnPropertyChanged("IsCycle"); }
        }
        bool _IsChooseDD = true;
        /// <summary>
        /// 选择DD键
        /// </summary>
        public bool IsChooseDD
        {
            get { return _IsChooseDD; }
            set { _IsChooseDD = value; OnPropertyChanged("IsChooseDD"); }
        }
        #endregion
    }
}
