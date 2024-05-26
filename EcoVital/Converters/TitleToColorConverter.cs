using System.Globalization;

namespace EcoVital.Converters;

/// <summary>
/// Convierte un título en un color basado en su contenido.
/// </summary>
public class TitleToColorConverter : IValueConverter
{
    /// <summary>
    /// Convierte un título en un color.
    /// Si el título es "¿No es tu cuenta? Cierra sesión aquí", retorna <see cref="Colors.Red"/>;
    /// de lo contrario, retorna <see cref="Colors.Black"/>.
    /// </summary>
    /// <param name="value">El título que se va a convertir.</param>
    /// <param name="targetType">El tipo de la propiedad de destino. No se utiliza en esta conversión.</param>
    /// <param name="parameter">Parámetro opcional de la conversión. No se utiliza en esta conversión.</param>
    /// <param name="culture">La referencia cultural a usar en la conversión.</param>
    /// <returns>Retorna <see cref="Colors.Red"/> si el título coincide, de lo contrario, retorna <see cref="Colors.Black"/>.</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value?.ToString() == "¿No es tu cuenta? Cierra sesión aquí")
            return Colors.Red;

        return Colors.Black;
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