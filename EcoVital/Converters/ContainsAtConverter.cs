using System.Globalization;

namespace EcoVital.Converters;

/// <summary>
/// Convierte una cadena de texto en un valor booleano indicando si contiene el carácter '@'.
/// </summary>
public class ContainsAtConverter : IValueConverter
{
    /// <summary>
    /// Convierte una cadena de texto en un valor booleano.
    /// Si la cadena contiene el carácter '@', retorna <c>true</c>; de lo contrario, retorna <c>false</c>.
    /// </summary>
    /// <param name="value">La cadena de texto que se va a convertir.</param>
    /// <param name="targetType">El tipo de la propiedad de destino. No se utiliza en esta conversión.</param>
    /// <param name="parameter">Parámetro opcional de la conversión. No se utiliza en esta conversión.</param>
    /// <param name="culture">La referencia cultural a usar en la conversión.</param>
    /// <returns>Retorna <c>true</c> si la cadena contiene el carácter '@', de lo contrario, retorna <c>false</c>.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string email)
            return email.Contains("@");

        return false;
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