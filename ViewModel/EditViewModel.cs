using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;

namespace WpfApplication2
{
    public class EditViewModel : INotifyPropertyChanged
    {
        #region Command&&Method

        public Keys ThisKey { get; set; }
        public DelegateCommand AddJYCommand { get; private set; }
        public DelegateCommand RemoveJYCommand { get; private set; }
        public DelegateCommand AddMapKeyCommand { get; private set; }
        public DelegateCommand RemoveMapKeyCommand { get; private set; }
        public DelegateCommand ExcuteJYCommand { get; private set; }
        public DelegateCommand OKCommand { get; private set; }
        public DelegateCommand ReverseCommand { get; private set; }
        public DelegateCommand UpMapKeyCommand { get; private set; }
        public DelegateCommand DownMapKeyCommand { get; private set; }

        public void UpMapKey()
        {
            if (SelectJYComand != null && SelectJYComand.KeyValue.Values.Count > 1 && SelectMapKey != Keys.None)
            {
                Keys temp = SelectMapKey;
                int currentIndex = SelectJYComand.KeyValue.Values.ToList().IndexOf(temp);
                if (currentIndex != 0)
                {
                    SelectJYComand.KeyValue.Values.Remove(temp);
                    SelectJYComand.KeyValue.Values.Insert(currentIndex - 1, temp);
                }
                SelectMapKey = temp;
            }
        }
        public void DownMapKey()
        {
            if (SelectJYComand != null && SelectJYComand.KeyValue.Values.Count > 1 && SelectMapKey != Keys.None)
            {
                Keys temp = SelectMapKey;
                int currentIndex = SelectJYComand.KeyValue.Values.ToList().IndexOf(temp);
                if (currentIndex != SelectJYComand.KeyValue.Values.Count - 1)
                {
                    SelectJYComand.KeyValue.Values.Remove(temp);
                    SelectJYComand.KeyValue.Values.Insert(currentIndex + 1, temp);
                }
                SelectMapKey = temp;
            }
        }
        public void KeyDownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            PKTextValue = e.Key.ToString();
            IsChoosePKTextBox = true;
        }
        public void MapKeyDownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            MapTextValue = e.Key.ToString();
            IsChoosePKTextBox = false;
        }
        public void KeyEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (IsChoosePKTextBox)
            {
                PKTextValue = e.Key.ToString();
            }
            else
            {
                MapTextValue = e.Key.ToString();
            }
        }
        public void Reverse()
        {
            SelectJYComand = null;
            JYComandList.Clear();
        }
        public void AddJYC()
        {
            Keys? pk = GetKeyFromStr(PKTextValue);
            if (pk != null)
            {
                JYCommand jyc = new JYCommand();
                jyc.KeyValue = new Key_Values();
                jyc.KeyValue.PK = (Keys)pk;
                JYComandList.Add(jyc);
                SelectJYComand = jyc;
            }
        }
        public void RemoveJYC()
        {
            if (SelectJYComand != null && SelectJYComand.KeyValue != null)
            {
                if (JYComandList.Count(t => t.KeyValue.PK == SelectJYComand.KeyValue.PK) > 0)
                {
                    var jyc = JYComandList.First(t => t.KeyValue.PK == SelectJYComand.KeyValue.PK);
                    if (jyc != null)
                    {
                        JYComandList.Remove(jyc);
                        if (SelectJYComand == jyc)
                        {
                            SelectJYComand = JYComandList.Last();
                        }
                    }
                }
            }
        }
        public void AddMapKey()
        {
            Keys? mapk = GetKeyFromStr(MapTextValue);
            if (mapk != null && SelectJYComand != null)
            {
                SelectJYComand.KeyValue.Values.Add((Keys)mapk);
            }
        }
        public void RemoveMapKey()
        {
            try
            {
                if (SelectJYComand != null)
                {
                    SelectJYComand.KeyValue.Values.RemoveAt(SelectJYComand.KeyValue.Values.Count - 1);
                }
            }
            catch(Exception ex)
            { }
        }
        public void OK()
        {
            //if (JYComandList != null && JYComandList.Count > 0)
            //{
            //    JYComandList.All(t => {
            //        t.KeyValue.UpDownDelay = UpDownDelayMS;
            //        t.KeyValue.KeysDelays = KKDelayMS;
            //        return true;
            //    });
            //}
            this.View.Close();
        }
        public static Keys? GetKeyFromStr(string str)
        {
            if (!string.IsNullOrEmpty(str) && DicKeyStr.Keys.Contains(str))
            {
                return DicKeyStr[str];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Ctor

        public EditViewModel(EditView vm, ObservableCollection<JYCommand> cmdList, Keys key)
        {
            View = vm;
            ThisKey = key;
            AddJYCommand = new DelegateCommand(AddJYC);
            RemoveJYCommand = new DelegateCommand(RemoveJYC);
            AddMapKeyCommand = new DelegateCommand(AddMapKey);
            RemoveMapKeyCommand = new DelegateCommand(RemoveMapKey);
            OKCommand = new DelegateCommand(OK);
            ReverseCommand = new DelegateCommand(Reverse);
            UpMapKeyCommand = new DelegateCommand(UpMapKey);
            DownMapKeyCommand = new DelegateCommand(DownMapKey);
            IsChoosePKTextBox = true;
            JYComandList = cmdList;
            if (JYComandList.Count != 0)
            {
                SelectJYComand = JYComandList[0];
            }
        }
        #endregion

        #region Property

        public bool IsChoosePKTextBox { get; set; }
        public EditView View { get; set; }
  
        private string _PKTextValue;
        /// <summary>
        /// 主键Text绑定值
        /// </summary>
        public string PKTextValue
        {
            get { return _PKTextValue; }
            set { _PKTextValue = value; OnPropertyChanged("PKTextValue"); }
        }

        private string _MapTextValue;
        /// <summary>
        /// 映射键Text绑定值
        /// </summary>
        public string MapTextValue
        {
            get { return _MapTextValue; }
            set { _MapTextValue = value; OnPropertyChanged("MapTextValue"); }
        }

        private ObservableCollection<JYCommand> _JYComandList = new ObservableCollection<JYCommand>();
        /// <summary>
        /// 该窗口所绑定的主键-映射键集合
        /// </summary>
        public ObservableCollection<JYCommand> JYComandList
        {
            get { return _JYComandList; }
            set { _JYComandList = value; OnPropertyChanged("JYComandList"); }
        }

        private JYCommand _SelectJYComand = new JYCommand();
        /// <summary>
        /// PKDataGrid选中选中项
        /// </summary>
        public JYCommand SelectJYComand
        {
            get { return _SelectJYComand; }
            set { _SelectJYComand = value; OnPropertyChanged("SelectJYComand"); }
        }

        private Keys _SelectMapKey;
        /// <summary>
        /// MapKeyDataGrid选中项
        /// </summary>
        public Keys SelectMapKey
        {
            get { return _SelectMapKey; }
            set { _SelectMapKey = value; OnPropertyChanged("SelectMapKey"); }
        }

        private static Dictionary<string, Keys> _DicKeyStr;
        /// <summary>
        /// 键盘键值对集合
        /// </summary>
        public static Dictionary<string, Keys> DicKeyStr
        {
            get
            {
                if (_DicKeyStr != null && _DicKeyStr.Count > 0)
                {
                    return _DicKeyStr;
                }
                _DicKeyStr = new Dictionary<string, Keys>();
                foreach (var item in Enum.GetValues(typeof(Keys)))
                {
                    try
                    {
                        _DicKeyStr.Add(item.ToString(), (Keys)item);
                    }
                    catch
                    {
                        continue;
                    }
                }
                return _DicKeyStr;
            }
        }

        #endregion

        #region Event

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
