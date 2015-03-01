using OneNote.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OneNote.Converters
{
    public class TopicSubscribeStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var status = (TopicSubscribeStatus)value;
            switch (status)
            {
                case TopicSubscribeStatus.Subscribed:
                    return "Unsubscribe";
                case TopicSubscribeStatus.Unsubscribed:
                    return "Subscribe";
                default:
                    return "TODO !";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
