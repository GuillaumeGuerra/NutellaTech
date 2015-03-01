using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace KanbanBoard.Converters
{
    public class PostItImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string color = (string)value;
            switch (color)
            {
                case "LightSteelBlue":
                    return "/KanbanBoard;component/Resources/postit_blue.png";
                case "LightGray":
                    return "/KanbanBoard;component/Resources/postit_grey.png";
                case "LightCoral":
                    return "/KanbanBoard;component/Resources/postit_orange.png";
                case "LightYellow":
                    return "/KanbanBoard;component/Resources/postit_yellow.png";
                default:
                    break;
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
