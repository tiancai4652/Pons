using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// RunForm.xaml 的交互逻辑
    /// 
    /// </summary>
    public partial class RunForm : MetroWindow, INotifyPropertyChanged
    {
        private static CDD dd;
        bool IsChooseDD = false;
        Thread TrExcute;
        Thread TrAbort;
        public Keys StopKey = Keys.Escape;
        public MainView MainV { get; set; }
        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        private void InitialTray()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = "程序开始运行";
            notifyIcon.Text = "BiuBiuBiu";
            notifyIcon.Icon = new System.Drawing.Icon(System.Windows.Forms.Application.StartupPath + "\\ico\\O.ico");
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(2000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
            //设置菜单项
            System.Windows.Forms.MenuItem menu1 = new System.Windows.Forms.MenuItem("菜单项1");
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("exit");
            exit.Click += new EventHandler(exit_Click);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
            //窗体状态改变时候触发
            this.StateChanged += new EventHandler(SysTray_StateChanged);
        }
        private void SysTray_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Visibility = Visibility.Visible;
            }
        }
        private void exit_Click(object sender, EventArgs e)
        {
                notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
        }
        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            notifyIcon.Dispose();
            this.Close();
            MainV.ShowInTaskbar = true;
            MainV.Visibility = Visibility.Visible; 
        }
        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            KeysToRegisterList.All(t => { Unregister(Handle, t); return true; });
        }
        public List<int> KeysToRegisterList { get; set; }
        public Group CurrentGroup { get; set; }
        public RunForm(ObservableCollection<Group> commands,MainView mainV,bool isChooseDD)
        {
            MainV = mainV;
            Groups = new ObservableCollection<Group>( commands.Where(t=>t.IsEnabled).ToList());
            InitializeComponent();
            this.DataContext = this;
            KeysToRegisterList = new List<int>();
            CurrentGroup = new Group();
            InitialTray();
            WindowState = WindowState.Minimized;
            IsChooseDD = isChooseDD;
            if (IsChooseDD)
            {
                dd = new CDD();
                string path = System.Windows.Forms.Application.StartupPath + "\\DD81200x64.64.dll";
                if (!LoadDllFile(path,this))
                {
                    return;
                }
            }
        }
        private static bool LoadDllFile(string dllfile, MetroWindow window)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(dllfile);
            if (!fi.Exists)
            {
                System.Windows.Forms.MessageBox.Show("文件不存在");
                return false;
            }
            int ret = dd.Load(dllfile);
            if (ret == -2) { System.Windows.Forms.MessageBox.Show("装载库时发生错误"); return false; }
            if (ret == -1) { System.Windows.Forms.MessageBox.Show("取函数地址时发生错误"); return false; }
            if (ret == 0) { /*System.Windows.Forms.MessageBox.Show("非增强模块");*/ }
            return true;
        }
        private ObservableCollection<Group> _Groups;
        public ObservableCollection<Group> Groups
        {
            get
            {
                return _Groups;
            }
            set
            {
                _Groups = value;
                OnPropertyChanged("Commands");
            }
        }
        private Group _SelectedCommand;
        public Group SelectedCommand
        {
            get
            {
                return _SelectedCommand;
            }
            set
            {
                _SelectedCommand = value;
                OnPropertyChanged("SelectedCommand");
            }
        }
        public IntPtr Handle { get; set; }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
          
        }
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0312)
            {
                int PressKeyValue = wParam.ToInt32();

                #region 
                if (PressKeyValue == (int)StopKey)
                {
                    if (TrExcute != null)
                    {
                        while (TrExcute.IsAlive)
                        {
                            TrExcute.Abort();
                            Thread.Sleep(100);
                        }
                    }
                }




                #endregion

                Group gp = null;
                gp = Groups.ToList().Find(t => (int)t.Key == PressKeyValue);
                if (gp != null)
                {
                    KeysToRegisterList.All(t => { Unregister(Handle, t); return true; });
                    for (int i = 0; i < Groups.Count; i++)
                    {
                        if (Groups[i].IsEnabled)
                        {
                            for (int j = 0; j < Groups[i].Commands.Count; j++)
                            {
                                Register(Handle, (int)Groups[i].Key, KeyModifiers.None, Groups[i].Key);
                            }
                        }
                    }
                    Register(Handle, (int)StopKey, KeyModifiers.None, StopKey);
                    for (int i = 0; i < gp.Commands.Count; i++)
                    {
                        Register(Handle, (int)gp.Commands[i].KeyValue.PK, KeyModifiers.None, gp.Commands[i].KeyValue.PK);
                    }
                    CurrentGroup = gp;
                }

       
                if (CurrentGroup.Commands != null && CurrentGroup.Commands.Count > 0)
                {
                    JYCommand cmd = null;
                    cmd = CurrentGroup.Commands.ToList().Find(t => (int)(t.KeyValue.PK) == PressKeyValue && t.IsRunning == false);
                    #region
                    List<Keys> InfulentKeys = new List<Keys>();
                 
                    if (cmd != null)
                    {
                        Unregister(Handle, (int)CurrentGroup.Key);
                        Unregister(Handle, (int)cmd.KeyValue.PK);

                        List<Keys> kkl = new List<Keys>();
                        foreach (var item in cmd.KeyValue.Values)
                        {
                            if (RegistorList.Contains(item))
                            {
                                Unregister(Handle, (int)item);
                                kkl.Add(item);
                            }
                        }
                        cmd.IsRunning = true;
                        if (TrExcute==null||TrExcute.IsAlive != true)
                        {
                            TrExcute = new Thread(() =>
                            {
                                ///执行命令
                                for (int j = 0; j < cmd.KeyValue.ExcuteTimes; j++)
                                {
                                    for (int i = 0; i < cmd.KeyValue.Values.Count; i++)
                                    {
                                        Press(cmd.KeyValue.Values[i], cmd.KeyValue.UpDownDelay);
                                        Thread.Sleep(cmd.KeyValue.KeysDelays);
                                    }
                                }
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                ///注册按的键
                                Register(Handle, (int)cmd.KeyValue.PK, KeyModifiers.None, cmd.KeyValue.PK);
                                    foreach (var item in kkl)
                                    {
                                        Register(Handle, (int)item, KeyModifiers.None, item);
                                    }
                                }));
                                Register(Handle, (int)CurrentGroup.Key, KeyModifiers.None, CurrentGroup.Key);
                            });
                            TrExcute.Start();
                        }
                        cmd.IsRunning = false;
                    }
                    #endregion
                }
            }
            return Handle;
        }

        #region
        [DllImport("user32.dll")]
        public static extern int MapVirtualKey(uint Ucode, uint uMapType);
        public void Press(Keys key, int delay)
        {
            if (!IsChooseDD)
            {
                keybd_event((byte)key, (byte)(MapVirtualKey((uint)key, 0)), 0, 0);
                Thread.Sleep(delay);
                keybd_event((byte)key, (byte)(MapVirtualKey((uint)key, 0)), 2, 0);
            }
            else
            {
                dd.key(KeyToDDKey.Dic[key], 1);
                Thread.Sleep(delay);
                dd.key(KeyToDDKey.Dic[key], 2);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedCommand = Groups[0];
            Handle = new WindowInteropHelper(this).Handle;
            for (int i = 0; i < Groups.Count; i++)
            {
                if (Groups[i].IsEnabled)
                {
                    for (int j = 0; j < Groups[i].Commands.Count; j++)
                    {
                        Register(Handle, (int)Groups[i].Key, KeyModifiers.None, Groups[i].Key);
                    }
                }
            }
            Register(Handle, (int)StopKey, KeyModifiers.None, StopKey);
            
        }
        List<Keys> RegistorList = new List<Keys>();
        public void Register(IntPtr hWnd,int id,KeyModifiers fsModifiers,Keys vk)
        {
            if (!RegistorList.Exists(t => (int)t == ((int)vk)))
            {
                RegistorList.Add(vk);
            }
            RegisterHotKey(hWnd, id, fsModifiers, vk);
        }
        public void Unregister(IntPtr hWnd, int id)
        {
            if (RegistorList.Exists(t => (int)t == (id)))
            {
                RegistorList.Remove((Keys)id);
            }
            UnregisterHotKey(hWnd, id);
        }
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd,int id,KeyModifiers fsModifiers,Keys vk);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd,int id);
        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }
        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(byte bVk,byte bScan,int dwFlags,int dwExtraInfo);
        #region INotifyPropertyChanged
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

        #endregion

        private void MetroWindow_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                notifyIcon.Dispose();
                this.Close();
                MainV.ShowInTaskbar = true;
                MainV.Visibility = Visibility.Visible;
            }
        }

        private void MetroWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                notifyIcon.Dispose();
                if (TrExcute.IsAlive)
                {
                    TrExcute.Abort();
                }
                this.Close();
                MainV.ShowInTaskbar = true;
                MainV.Visibility = Visibility.Visible;
            }
        }

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            Unregister(Handle, (int)StopKey);
        }
    }


    public class CDD
    {
        [DllImport("Kernel32")]
        private static extern System.IntPtr LoadLibrary(string dllfile);

        [DllImport("Kernel32")]
        private static extern System.IntPtr GetProcAddress(System.IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);


        public delegate int pDD_btn(int btn);
        public delegate int pDD_whl(int whl);
        public delegate int pDD_key(int ddcode, int flag);
        public delegate int pDD_mov(int x, int y);
        public delegate int pDD_movR(int dx, int dy);
        public delegate int pDD_str(string str);
        public delegate int pDD_todc(int vkcode);

        public pDD_btn btn;          // 鼠标点击
        public pDD_whl whl;          // 鼠标滚轮
        public pDD_mov mov;       // 鼠标绝对移动
        public pDD_movR movR;   // 鼠标相对移动
        public pDD_key key;          // 键盘按键
        public pDD_str str;            //  键盘字符
        public pDD_todc todc;      // 标准虚拟键码转DD码

        //增强版功能
        public delegate Int32 pDD_MouseMove(IntPtr hwnd, Int32 x, Int32 y);
        public delegate Int32 pDD_SnapPic(IntPtr hwnd, Int32 x, Int32 y, Int32 w, Int32 h);
        public delegate Int32 pDD_PickColor(IntPtr hwnd, Int32 x, Int32 y, Int32 mode);
        public delegate IntPtr pDD_GetActiveWindow();

        public pDD_MouseMove MouseMove;                           //鼠标移动
        public pDD_SnapPic SnapPic;                                         //  抓图
        public pDD_PickColor PickColor;                                    //取色
        public pDD_GetActiveWindow GetActiveWindow;          //取激活窗口句柄

        private System.IntPtr m_hinst;

        ~CDD()
        {
            if (!m_hinst.Equals(IntPtr.Zero))
            {
                bool b = FreeLibrary(m_hinst);
            }
        }


        public int Load(string dllfile)
        {
            m_hinst = LoadLibrary(dllfile);
            if (m_hinst.Equals(IntPtr.Zero))
            {
                return -2;
            }
            else
            {
                return GetDDfunAddress(m_hinst);
            }
        }

        //取函数地址返回值  -1：取通用函数地址错误 ，  0：仅取通用函数地址正确 ， 1：取通用函数和增强函数地址都正确
        private int GetDDfunAddress(IntPtr hinst)
        {
            IntPtr ptr;

            ptr = GetProcAddress(hinst, "DD_btn");
            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            btn = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_btn)) as pDD_btn;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_whl");
            whl = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_whl)) as pDD_whl;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_mov");
            mov = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_mov)) as pDD_mov;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_key");
            key = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_key)) as pDD_key;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_movR");
            movR = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_movR)) as pDD_movR;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_str");
            str = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_str)) as pDD_str;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_todc");
            todc = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_todc)) as pDD_todc;

            //下面四个函数，只有在增强版中才可用
            ptr = GetProcAddress(hinst, "DD_MouseMove"); //鼠标移动
            if (!ptr.Equals(IntPtr.Zero)) MouseMove = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_MouseMove)) as pDD_MouseMove;

            ptr = GetProcAddress(hinst, "DD_SnapPic");        //抓取图片
            if (!ptr.Equals(IntPtr.Zero)) SnapPic = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_SnapPic)) as pDD_SnapPic;

            ptr = GetProcAddress(hinst, "DD_PickColor");      //取色
            if (!ptr.Equals(IntPtr.Zero)) PickColor = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_PickColor)) as pDD_PickColor;

            ptr = GetProcAddress(hinst, "DD_GetActiveWindow");    //获取激活窗口句柄
            if (!ptr.Equals(IntPtr.Zero)) GetActiveWindow = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_GetActiveWindow)) as pDD_GetActiveWindow;

            if (MouseMove == null || SnapPic == null || PickColor == null || GetActiveWindow == null)
            {
                return 0;
            }

            return 1;
        }
    }

    public class KeyToDDKey
    {
        static Dictionary<Keys, int>  _Dic = new Dictionary<Keys, int>();
        public static Dictionary<Keys, int> Dic
        {
            get
            {
                if (_Dic.Keys.Count == 0)
                {
                    _Dic.Add(Keys.Escape, 100);
                    _Dic.Add(Keys.F1, 101);
                    _Dic.Add(Keys.F2, 102);
                    _Dic.Add(Keys.F3, 103);
                    _Dic.Add(Keys.F4, 104);
                    _Dic.Add(Keys.F5, 105);
                    _Dic.Add(Keys.F6, 106);
                    _Dic.Add(Keys.F7, 107);
                    _Dic.Add(Keys.F8, 108);
                    _Dic.Add(Keys.F9, 109);
                    _Dic.Add(Keys.F10, 110);
                    _Dic.Add(Keys.F11, 111);
                    _Dic.Add(Keys.F12, 112);

                    _Dic.Add(Keys.D1, 201);
                    _Dic.Add(Keys.D2, 202);
                    _Dic.Add(Keys.D3, 203);
                    _Dic.Add(Keys.D4, 204);
                    _Dic.Add(Keys.D5, 205);
                    _Dic.Add(Keys.D6, 206);
                    _Dic.Add(Keys.D7, 207);
                    _Dic.Add(Keys.D8, 208);
                    _Dic.Add(Keys.D9, 209);
                    _Dic.Add(Keys.D0, 210);

                    _Dic.Add(Keys.Tab, 300);
                    _Dic.Add(Keys.Q, 301);
                    _Dic.Add(Keys.W, 302);
                    _Dic.Add(Keys.E, 303);
                    _Dic.Add(Keys.R, 304);
                    _Dic.Add(Keys.T, 305);
                    _Dic.Add(Keys.Y, 306);
                    _Dic.Add(Keys.U, 307);
                    _Dic.Add(Keys.I, 308);
                    _Dic.Add(Keys.O, 309);
                    _Dic.Add(Keys.P, 310);

                    _Dic.Add(Keys.A, 401);
                    _Dic.Add(Keys.S, 402);
                    _Dic.Add(Keys.D, 403);
                    _Dic.Add(Keys.F, 404);
                    _Dic.Add(Keys.G, 405);
                    _Dic.Add(Keys.H, 406);
                    _Dic.Add(Keys.J, 407);
                    _Dic.Add(Keys.K, 408);
                    _Dic.Add(Keys.L, 409);

                    _Dic.Add(Keys.Z, 501);
                    _Dic.Add(Keys.X, 502);
                    _Dic.Add(Keys.C, 503);
                    _Dic.Add(Keys.V, 504);
                    _Dic.Add(Keys.B, 505);
                    _Dic.Add(Keys.N, 506);
                    _Dic.Add(Keys.M, 507);

                }

                return _Dic;
            }
        }











    }
}
