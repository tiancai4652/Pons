using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApplication2
{
    public class BoolColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Media.Brush ColorTrue = System.Windows.Media.Brushes.GreenYellow;
            System.Windows.Media.Brush ColorFalse = System.Windows.Media.Brushes.Green;
            bool? input = (bool)value;
            if (input == null || input != null && (bool)input == false)
            {
                return ColorFalse;
            }
            else
            {
                return ColorTrue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
