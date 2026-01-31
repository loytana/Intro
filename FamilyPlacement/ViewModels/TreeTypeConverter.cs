using FamilyPlacement.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace FamilyPlacement.ViewModels
{
    public class FurnitureTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TreeType type)
            {
                switch (type)
                {
                    case TreeType.Oak:
                        return "Дуб";
                    case TreeType.Pine:
                        return "Сосна";
                    case TreeType.Birch:
                        return "Берёза";
                    default:
                        return value != null ? value.ToString() : string.Empty;
                        
                }
            }
            return value != null ? value.ToString() : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                switch (str)
                {
                    case "Дуб":
                        return TreeType.Oak;
                    case "Сосна":
                        return TreeType.Pine;
                    case "Берёза":
                        return TreeType.Birch;
                    default:
                        throw new ArgumentException(string.Format("Неизвестный тип мебели: (0}", str));
                        
                }
            }
            throw new ArgumentException("Некорректное значение для конвертации");
        }
    }
}
