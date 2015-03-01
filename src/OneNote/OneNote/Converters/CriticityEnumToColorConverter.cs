using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace OneNote.Converters
{
    public class CriticityEnumToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var criticity = (CriticityEnum)value;
            switch (criticity)
            {
                case CriticityEnum.Low:
                    return "LightSkyBlue";
                case CriticityEnum.Medium:
                    return "Orange";
                case CriticityEnum.High:
                    return "Tomato";
            }

            return "White";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
