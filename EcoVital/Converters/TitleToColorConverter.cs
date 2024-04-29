namespace EcoVital.Converters;

using System;
using System.Globalization;
using Microsoft.Maui.Controls;

public class TitleToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value?.ToString() == "¿No es tu cuenta? Cierra sesión aquí")
            return Colors.Red;

        return Colors.Black;
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}