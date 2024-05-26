using System.Globalization;

namespace EcoVital.Converters;

/// <summary>
/// Convierte un valor booleano en una opción de diseño (LayoutOptions).
/// </summary>
public class BoolToLayoutOptionsConverter : IValueConverter
{
    /// <summary>
    /// Convierte un valor booleano en una opción de diseño (LayoutOptions). 
    /// Si el valor es verdadero, retorna <see cref="LayoutOptions.End"/>; de lo contrario, retorna <see cref="LayoutOptions.Start"/>.
    /// </summary>
    /// <param name="value">El valor booleano que se va a convertir.</param>
    /// <param name="targetType">El tipo de la propiedad de destino. No se utiliza en esta conversión.</param>
    /// <param name="parameter">Parámetro opcional de la conversión. No se utiliza en esta conversión.</param>
    /// <param name="culture">La referencia cultural a usar en la conversión.</param>
    /// <returns>Retorna <see cref="LayoutOptions.End"/> si el valor es verdadero, de lo contrario, retorna <see cref="LayoutOptions.Start"/>.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return boolValue ? LayoutOptions.End : LayoutOptions.Start;

        return LayoutOptions.Start;
    }

    /// <summary>
    /// No implementado. Lanza una excepción si se llama.
    /// </summary>
    /// <param name="value">El valor a convertir de vuelta. No se utiliza.</param>
    /// <param name="targetType">El tipo de la propiedad de destino. No se utiliza.</param>
    /// <param name="parameter">Parámetro opcional de la conversión. No se utiliza.</param>
    /// <param name="culture">La referencia cultural a usar en la conversión. No se utiliza.</param>
    /// <returns>No retorna ningún valor.</returns>
    /// <exception cref="NotImplementedException">Siempre se lanza esta excepción, ya que el método no está implementado.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}