namespace EcoVital.Services;

/// <summary>
/// Implementa un servicio de carga que muestra y oculta un indicador de carga.
/// </summary>
public class LoadingService : ILoadingService
{
    bool _isLoadingShown;

    /// <summary>
    /// Muestra el indicador de carga.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica de mostrar el indicador de carga.</returns>
    public async Task ShowLoading()
    {
        if (_isLoadingShown) return;
        _isLoadingShown = true;
        await Application.Current.MainPage.Navigation.PushModalAsync(new LoadingPage(), true);
    }

    /// <summary>
    /// Oculta el indicador de carga.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica de ocultar el indicador de carga.</returns>
    public async Task HideLoading()
    {
        if (!_isLoadingShown) return;
        _isLoadingShown = false;
        await Application.Current.MainPage.Navigation.PopModalAsync(true);
    }
}