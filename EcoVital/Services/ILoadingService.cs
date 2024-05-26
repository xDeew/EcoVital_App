namespace EcoVital.Services;

/// <summary>
/// Define una interfaz para un servicio de carga que muestra y oculta un indicador de carga.
/// </summary>
public interface ILoadingService
{
    /// <summary>
    /// Muestra el indicador de carga.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica de mostrar el indicador de carga.</returns>
    Task ShowLoading();

    /// <summary>
    /// Oculta el indicador de carga.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica de ocultar el indicador de carga.</returns>
    Task HideLoading();
}