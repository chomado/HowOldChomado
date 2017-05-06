using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HowOldChomado.Converters
{
    class BinaryToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var binary = (byte[])value;
            if (binary == null)
            {
                var isWindowsOs = Device.RuntimePlatform == Device.Windows || Device.RuntimePlatform == Device.WinPhone;

                string file = isWindowsOs 
                    ? "Assets/noimage.png" 
                    : "noimage.png";

                return ImageSource.FromFile(file);
            }
            return ImageSource.FromStream(() => new MemoryStream(binary));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
