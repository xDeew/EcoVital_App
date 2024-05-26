using System.Globalization;

namespace EcoVital.Converters;

/// <summary>
/// Convierte un valor booleano en un valor de opacidad.
/// </summary>
public class BoolToOpacityConverter : IValueConverter
{
    /// <summary>
    /// Convierte un valor booleano en un valor de opacidad. 
    /// Si el valor es verdadero, retorna 0.2; de lo contrario, retorna 1.
    /// </summary>
    /// <param name="value">El valor booleano que se va a convertir.</param>
    /// <param name="targetType">El tipo de la propiedad de destino. No se utiliza en esta conversión.</param>
    /// <param name="parameter">Parámetro opcional de la conversión. No se utiliza en esta conversión.</param>
    /// <param name="culture">La referencia cultural a usar en la conversión.</param>
    /// <returns>Retorna 0.2 si el valor es verdadero, de lo contrario, retorna 1.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && boolValue)
            return 0.2;

        return 1;
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