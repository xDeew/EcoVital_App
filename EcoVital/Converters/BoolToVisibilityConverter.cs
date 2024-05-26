using System.Globalization;

namespace EcoVital.Converters;

/// <summary>
/// Convierte un valor booleano en un valor de visibilidad.
/// </summary>
public class BoolToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Convierte un valor booleano en un valor de visibilidad.
    /// Si el valor es verdadero, retorna <see cref="Visibility.Visible"/>; de lo contrario, retorna <see cref="Visibility.Collapsed"/>.
    /// </summary>
    /// <param name="value">El valor booleano que se va a convertir.</param>
    /// <param name="targetType">El tipo de la propiedad de destino. No se utiliza en esta conversión.</param>
    /// <param name="parameter">Parámetro opcional de la conversión. No se utiliza en esta conversión.</param>
    /// <param name="culture">La referencia cultural a usar en la conversión.</param>
    /// <returns>Retorna <see cref="Visibility.Visible"/> si el valor es verdadero, de lo contrario, retorna <see cref="Visibility.Collapsed"/>.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? Visibility.Visible : Visibility.Collapsed;
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
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}