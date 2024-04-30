namespace EcoVital.Services;

public interface ILoadingService
{
    Task ShowLoading();
    Task HideLoading();
}