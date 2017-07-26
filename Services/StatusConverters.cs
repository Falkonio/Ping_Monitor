using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Ping_Monitor
{
    class StatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "Exclamation";
            else if (value.ToString() == "Success") return "Check";
            else if (value.ToString() == "TimedOut") return "HourglassOutline";
            else if (value.ToString() == "Error") return "Times";
            else if (value.ToString() == "TTLExpired") return "Refresh";
            else return "Exclamation";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "DarkRed";
            else if (value.ToString() == "Success") return "DarkGreen";
            else return "DarkRed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
