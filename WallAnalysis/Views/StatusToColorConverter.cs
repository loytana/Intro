using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WallAnalysis.Models;

namespace WallAnalysis.Views
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ValidationStatus status)
            {
                switch (status)
                {
                    case ValidationStatus.Normal:
                        return new SolidColorBrush(Colors.LightGreen);
                    case ValidationStatus.Exceeded:
                        return new SolidColorBrush(Colors.LightCoral);
                    case ValidationStatus.Error:
                        return new SolidColorBrush(Colors.LightYellow);
                    default:
                        return new SolidColorBrush(Colors.Transparent);
                }
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
