using System.Windows.Input;
using EcoVital.Models;
using EcoVital.Services;
using EcoVital.Views;

namespace EcoVital.ViewModels;

public class ChangePasswordViewModel : BaseViewModel
{
    private readonly ILoginRepository _loginRepository;
    private string _newPassword;
    private string _confirmPassword;


    public ChangePasswordViewModel(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }

    public string NewPassword
    {
        get => _newPassword;
        set => SetProperty(ref _newPassword, value);
    }

    public string ConfirmNewPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }


    public ICommand ChangePasswordCommand
    {
        get { return new Command(Execute); }
    }

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

        if (_loginRepository == null)
        {
            throw new Exception("_loginRepository is null");
        }

        if (NewPassword == null)
        {
            throw new Exception("NewPassword is null");
        }

        // Intentamos obtener el usuario por el email
        string email = App.UserEmail;
        UserInfo userInfo = await _loginRepository.GetUserByEmail(email);
        App.UserInfo = userInfo;

        bool result = await _loginRepository.ChangePassword(App.UserInfo.UserId, NewPassword);
        if (result)
        {
            await Application.Current.MainPage.DisplayAlert("Éxito", "La contraseña se ha cambiado correctamente",
                "OK");

            // Esto limpia la pila de navegación y nos lleva a la página de inicio de sesión
            // Limpiar la pila de navegacion conlleva a que no se pueda regresar a la página anterior
            // await Shell.Current.Navigation.PopToRootAsync();


            await Shell.Current.GoToAsync("///LoginPage");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Ha ocurrido un error al cambiar la contraseña",
                "OK");
        }
    }

    private bool IsValidPassword(string password)
    {
        // Verifica que la contraseña tenga al menos 6 caracteres
        if (password.Length < 6)
        {
            return false;
        }

        // Verifica que la contraseña tenga al menos una letra mayúscula
        if (!password.Any(char.IsUpper))
        {
            return false;
        }

        // Verifica que la contraseña tenga al menos un símbolo
        if (!password.Any(char.IsSymbol) && !password.Any(char.IsPunctuation))
        {
            return false;
        }

        return true;
    }
}