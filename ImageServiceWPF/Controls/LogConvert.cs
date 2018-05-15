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
    /// <summary>
    /// converter for the log values
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    class LogConvert : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <see langword="null" />, the valid null value is used.
        /// </returns>
        /// <exception cref="InvalidOperationException">This needs to be converted to a brush.</exception>
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

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns <see langword="null" />, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
