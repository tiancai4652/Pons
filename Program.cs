using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WpfApplication2
{
    public static class Program
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            SplashScreen splashScreen = new SplashScreen("..\\Resource\\qqq.png");
            splashScreen.Show(false);
            PonApp app = new PonApp();
            app.InitialAppResources();
            MainView mv = new MainView();//WPF项目的Application实例，用来启动WPF项目的
            MainViewModel mvm = new MainViewModel(mv);
            mv.DataContext = mvm;
            try
            {
                splashScreen.Close(new TimeSpan(0, 0, 0));
            }
            catch { }
            app.Run(mv);
        }




    }
}
