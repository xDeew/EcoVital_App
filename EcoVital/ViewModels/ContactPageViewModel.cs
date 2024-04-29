using System.Windows.Input;

namespace EcoVital.ViewModels;

using Microsoft.Maui.ApplicationModel;

public class ContactPageViewModel : BaseViewModel
{
    public Command OpenMapsCommand { get; }
    public Command CallCommand { get; }

    public ICommand SendEmailCommand { get; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string Message { get; set; }

    public ContactPageViewModel()
    {
        CallCommand = new Command(Execute);
        SendEmailCommand = new Command(SendEmail);
        OpenMapsCommand = new Command(OpenMaps);
    }

    async void Execute()
    {
        await OnButtonClicked();
    }

    private void PlacePhoneCall()
    {
        try
        {
            PhoneDialer.Open("1234567890");
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // La aplicación no admite la realización de llamadas telefónicas.
        }
        catch (Exception ex)
        {
            // Algun otro error ha ocurrido.
        }
    }

    public async Task OnButtonClicked()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Phone>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Phone>();
        }

        if (status == PermissionStatus.Granted)
        {
            PlacePhoneCall();
        }
        else if (status == PermissionStatus.Denied)
        {
            // No se puede realizar la llamada telefónica porque el usuario ha denegado el permiso.
        }
        else if (status == PermissionStatus.Disabled)
        {
            // No se puede realizar la llamada telefónica porque el permiso está deshabilitado.
        }
    }
    private void SendEmail()
    {
        try
        {
            var message = new EmailMessage
            {
                Subject = "Consulta desde la App",
                Body = $"{UserName}\n\n{Message}",
                To = { "info@ecovital.com" }
            };

            Email.ComposeAsync(message);
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Email no soportado en este dispositivo
        }
        catch (Exception ex)
        {
            // Otro error ha ocurrido.
        }
    }

    private void OpenMaps()
    {
        try
        {
            var location = new Location(37.422, -122.084); // Actualiza con la ubicación real
            var options = new MapLaunchOptions { Name = "123 Eco Street, Green City" };
            Map.OpenAsync(location, options);
        }
        catch (Exception ex)
        {
            // Error al abrir mapas
        }
    }
}