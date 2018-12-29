using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApplication2
{
    public class Key_Values: INotifyPropertyChanged
    {
        public Key_Values()
        {
            Values = new ObservableCollection<Keys>();
            ID = Guid.NewGuid().ToString("N");
        }
        public string ID { get; set; }
        public Keys PK { get; set; }
        private string _ValueDBStr = string.Empty;
        public string ValueDBStr
        {
            get
            {
                _ValueDBStr= GetStrFromValues();
                return _ValueDBStr;
            }
            set
            {
                _ValueDBStr = value;
                Values = GetValuesFromStr();
            }
        }
        private string GetStrFromValues()
        {
            if (Values == null || Values != null && Values.Count == 0)
            {
                return "";
            }
            else
            {
                string Result = "";
                for (int i = 0; i < Values.Count; i++)
                {
                    Result += (int)Values[i] + "|";
                }
                return Result;
            }
        }
        private ObservableCollection<Keys> GetValuesFromStr()
        {
            List<string> List = _ValueDBStr.Split('|').ToList();
            List.RemoveAll(t=>t.Equals(""));
            List<Keys> x = List.Select(t => int.Parse(t)).ToList().Select(f=>(Keys)f).ToList();
            return new ObservableCollection<Keys>(x);


        }      
        private ObservableCollection<Keys> _Values = new ObservableCollection<Keys>();
        public  ObservableCollection<Keys> Values
        {
            get { return _Values; }
            set { _Values = value; OnPropertyChanged("Values"); }
        }
        public static Keys GetKeyFromStr(string KeyStr)
        {
            foreach (var item in Enum.GetValues(typeof(Keys)))
            {
                if (item.ToString().Equals(KeyStr))
                {
                    return (Keys)item;
                }
            }
            return Keys.Enter;
        }
        int _KeysDelays=200;
        public int KeysDelays
        {
            get { return _KeysDelays; }
            set { _KeysDelays = value; OnPropertyChanged("KeysDelays"); }
        }
        int _UpDownDelay=80;
        public int UpDownDelay
        {
            get { return _UpDownDelay; }
            set { _UpDownDelay = value; OnPropertyChanged("UpDownDelay"); }
        }

        int _ExcuteTimes = 1;
        /// <summary>
        /// 执行次数
        /// </summary>
        public int ExcuteTimes
        {
            get { return _ExcuteTimes; }
            set { _ExcuteTimes = value; OnPropertyChanged("ExcuteTimes"); }
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
