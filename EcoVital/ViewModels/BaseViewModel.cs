using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel base que proporciona propiedades comunes para todos los ViewModels.
/// </summary>
public partial class BaseViewModel : ObservableObject
{
    /// <summary>
    /// Indica si el ViewModel está ocupado realizando una operación.
    /// </summary>
    [ObservableProperty]
    public bool _isBusy;

    /// <summary>
    /// Título del ViewModel.
    /// </summary>
    [ObservableProperty]
    public string _title;
}