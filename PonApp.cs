using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WpfApplication2
{
    public class PonApp: Application
    {
        public void InitialAppResources()
        {
            this.Resources.MergedDictionaries.Clear();

            List<Uri> resourcesUris = new List<Uri>()
            {
            new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml"),
            new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml"),
            new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml"),
            new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml"),
            new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml"),
            };

            for (int rdIndex = 0; rdIndex < resourcesUris.Count; rdIndex++)
            {
                System.Windows.ResourceDictionary dict = new System.Windows.ResourceDictionary();
                dict.Source = resourcesUris[rdIndex];
                this.Resources.MergedDictionaries.Add(dict);
            }
        }
    }
}
