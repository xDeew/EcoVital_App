using System.Windows.Input;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para la página de contacto.
/// </summary>
public class ContactPageViewModel : BaseViewModel
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ContactPageViewModel"/>.
    /// </summary>
    public ContactPageViewModel()
    {
        CallCommand = new Command(Execute);
        SendEmailCommand = new Command(SendEmail);
        OpenMapsCommand = new Command(OpenMaps);
    }

    /// <summary>
    /// Comando para abrir mapas.
    /// </summary>
    public Command OpenMapsCommand { get; }

    /// <summary>
    /// Comando para realizar una llamada.
    /// </summary>
    public Command CallCommand { get; }

    /// <summary>
    /// Comando para enviar un correo electrónico.
    /// </summary>
    public ICommand SendEmailCommand { get; }

    /// <summary>
    /// Obtiene o establece el nombre del usuario.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Obtiene o establece el correo electrónico del usuario.
    /// </summary>
    public string UserEmail { get; set; }

    /// <summary>
    /// Obtiene o establece el mensaje del usuario.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Ejecuta el comando de llamada.
    /// </summary>
    async void Execute()
    {
        await OnButtonClicked();
    }

    /// <summary>
    /// Realiza una llamada telefónica.
    /// </summary>
    void PlacePhoneCall()
    {
        try
        {
            PhoneDialer.Open("1234567890");
        }
        catch (FeatureNotSupportedException)
        {
            // La aplicación no admite la realización de llamadas telefónicas.
        }
        catch (Exception)
        {
            // Algun otro error ha ocurrido.
        }
    }

    /// <summary>
    /// Maneja el evento de clic del botón para realizar una llamada.
    /// </summary>
    async Task OnButtonClicked()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Phone>();
        if (status != PermissionStatus.Granted) status = await Permissions.RequestAsync<Permissions.Phone>();

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

    /// <summary>
    /// Envía un correo electrónico.
    /// </summary>
    void SendEmail()
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
        catch (FeatureNotSupportedException)
        {
            // Email no soportado en este dispositivo
        }
        catch (Exception)
        {
            // Otro error ha ocurrido.
        }
    }

    /// <summary>
    /// Abre la ubicación en la aplicación de mapas.
    /// </summary>
    static void OpenMaps()
    {
        try
        {
            var location = new Location(37.422, -122.084); 
            var options = new MapLaunchOptions { Name = "123 Eco Street, Green City" };
            Map.OpenAsync(location, options);
        }
        catch (Exception)
        {
            // Error al abrir mapas
        }
    }
}