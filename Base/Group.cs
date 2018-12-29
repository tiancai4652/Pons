using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WpfApplication2
{
    public class Group : INotifyPropertyChanged
    {
        public Group()
        {
            ID = Guid.NewGuid().ToString("N");
        }
        public string Name { get; set; }
        public string ID { get; set; }
        private Keys _Key = Keys.None;
        /// <summary>
        /// 键
        /// </summary>
        public Keys Key
        {
            get { return _Key; }
            set { _Key = value; OnPropertyChanged("Key"); }
        }

        private bool _IsEnabled = true;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; OnPropertyChanged("IsEnabled"); }
        }
        private ObservableCollection<JYCommand> _Commands = new ObservableCollection<JYCommand>();
        /// <summary>
        /// 启用的命令
        /// </summary>
        public virtual  ObservableCollection<JYCommand> Commands
        {
            get { return _Commands; }
            set { _Commands = value; OnPropertyChanged("Commands"); }
        }
        private string _Discribe = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public string Discribe
        {
            get { return _Discribe; }
            set { _Discribe = value; OnPropertyChanged("Discribe"); }
        }
        bool _IsChooseDD = true;
        /// <summary>
        /// 启用DD键
        /// </summary>
        public bool IsChooseDD
        {
            get { return _IsChooseDD; }
            set { _IsChooseDD = value; OnPropertyChanged("IsChooseDD"); }
        }

        /// <summary>
        /// 生成自己的描述信息
        /// </summary>
        public void GetMyDescribe()
        {
            if (Key != Keys.None && Commands != null)
            {
                Discribe = string.Empty;
                foreach (var item in Commands)
                {
                    Discribe += Environment.NewLine + item.KeyValue.PK.ToString() + ":" + Environment.NewLine;
                    for (int i = 0; i < item.KeyValue.Values.Count; i++)
                    {
                        Discribe += "——>" + item.KeyValue.Values[i];
                    }
                }
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
