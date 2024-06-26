using System.Windows.Input;
using EcoVital.Services;

namespace EcoVital.ViewModels;

/// <summary>
/// ViewModel para gestionar el cambio de contraseña de los usuarios.
/// </summary>
public class ChangePasswordViewModel : BaseViewModel
{
    readonly ILoginRepository _loginRepository;
    string _confirmPassword;
    string _newPassword;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ChangePasswordViewModel"/>.
    /// </summary>
    /// <param name="loginRepository">El repositorio de inicio de sesión.</param>
    public ChangePasswordViewModel(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }

    /// <summary>
    /// Obtiene o establece la nueva contraseña.
    /// </summary>
    public string NewPassword
    {
        get => _newPassword;
        set => SetProperty(ref _newPassword, value);
    }

    /// <summary>
    /// Obtiene o establece la confirmación de la nueva contraseña.
    /// </summary>
    public string ConfirmNewPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    /// <summary>
    /// Comando para cambiar la contraseña.
    /// </summary>
    public ICommand ChangePasswordCommand => new Command(Execute);

    /// <summary>
    /// Ejecuta el comando para cambiar la contraseña.
    /// </summary>
    async void Execute()
    {
        if (_newPassword != _confirmPassword)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
            return;
        }

        if (!IsValidPassword(_newPassword))
        {
            await Application.Current.MainPage.DisplayAlert("Error",
                "La contraseña debe tener al menos 6 caracteres, una letra mayúscula y un símbolo.", "OK");
            return;
        }

        if (_loginRepository == null) throw new Exception("_loginRepository is null");

        if (NewPassword == null) throw new Exception("NewPassword is null");

        var email = App.UserEmail;
        var userInfo = await _loginRepository.GetUserByEmail(email);
        App.UserInfo = userInfo;

        var result = await _loginRepository.ChangePassword(App.UserInfo.UserId, NewPassword);
        if (result)
        {
            await Application.Current.MainPage.DisplayAlert("Éxito", "La contraseña se ha cambiado correctamente", "OK");
            await Shell.Current.GoToAsync("LoginPage");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un error al cambiar la contraseña", "OK");
        }
    }

    /// <summary>
    /// Valida si la contraseña cumple con los requisitos.
    /// </summary>
    /// <param name="password">La contraseña a validar.</param>
    /// <returns><c>true</c> si la contraseña es válida; de lo contrario, <c>false</c>.</returns>
    bool IsValidPassword(string password)
    {
        if (password.Length < 6) return false;
        if (!password.Any(char.IsUpper)) return false;
        if (!password.Any(char.IsSymbol) && !password.Any(char.IsPunctuation)) return false;

        return true;
    }
}