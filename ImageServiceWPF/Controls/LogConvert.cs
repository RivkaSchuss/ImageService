using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceWPF.Controls
{
    class LogConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.Name != "Brush")
            {
                throw new InvalidOperationException("This needs to be converted to a brush.");
            }

            string type = value.ToString();
            if (type.Equals("INFO"))
            {
                return Brushes.LightGreen;
            }
            else if (type.Equals("WARNING"))
            {
                return Brushes.Yellow;
            }
            else if (type.Equals("FAIL"))
            {
                return Brushes.Coral;
            }
            else
            {
                return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
