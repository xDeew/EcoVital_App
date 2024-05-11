using System.Windows.Input;
using EcoVital.Models;
using EcoVital.Services;
using EcoVital.UserControl;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;

namespace EcoVital.ViewModels
{
    public partial class RegisterPageViewModel : BaseViewModel
    {
        [ObservableProperty] string _userName;
        [ObservableProperty] string _email;
        [ObservableProperty] string _password;
        [ObservableProperty] string _confirmPassword;
        [ObservableProperty] string _securityQuestion;
        [ObservableProperty] string _securityAnswer;
        readonly ILoadingService _loadingService = new LoadingService();


        readonly ILoginRepository _loginRepository = new LoginService();

        public ICommand GoBackCommand { get; set; }

        public RegisterPageViewModel()
        {
            GoBackCommand = new RelayCommand(Cancel);
        }

        void Cancel()
        {
            Shell.Current.GoToAsync("LoginPage");
        }

        [ICommand]
        public async Task Register()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    "Se requiere conexión a Internet para registrarse.", "OK");

                return;
            }


            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword)
               )
            {
                await App.Current.MainPage.DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
                await _loadingService.HideLoading();

                return;
            }

            if (string.IsNullOrEmpty(Email) || !Email.Contains("@"))
            {
                await App.Current.MainPage.DisplayAlert("Error",
                    "Por favor, proporciona un correo electrónico válido.", "OK");

                return;
            }

            if (!IsPasswordSecure(Password))
            {
                await App.Current.MainPage.DisplayAlert("Contraseña insegura",
                    "Tu contraseña debe tener al menos 6 caracteres, incluir al menos un carácter en mayúsculas y un símbolo.",
                    "OK");

                await _loadingService.HideLoading();

                return;
            }

            // Validación de coincidencia de contraseña
            if (Password != ConfirmPassword)
            {
                await App.Current.MainPage.DisplayAlert("Error",
                    "La contraseña y la confirmación de contraseña no coinciden.", "OK");

                await _loadingService.HideLoading();

                return; // Detiene la ejecución si las contraseñas no coinciden
            }

            // Verificar si el usuario ya existe
            bool userExists = await _loginRepository.UserExists(UserName.ToLower()) ||
                              await _loginRepository.UserExists(Email.ToLower());

            if (userExists)
            {
                await App.Current.MainPage.DisplayAlert("Error",
                    "Un usuario con el mismo nombre de usuario y/o correo electrónico ya existe.", "OK");

                return;
            }


            // Proceda con el registro si todo está bien
            UserInfo userInfo = await _loginRepository.Register(Email, UserName, Password);

            if (userInfo != null)
            {
                if (Preferences.ContainsKey(nameof(App.UserInfo)))
                {
                    Preferences.Remove(nameof(App.UserInfo));
                }

                string userDetails = JsonConvert.SerializeObject(userInfo);
                Preferences.Set(nameof(App.UserInfo), userDetails);
                App.UserInfo = userInfo;

                Shell.Current.FlyoutHeader = new FlyoutHeaderControl();

                // Mostrar mensaje exitoso
                await _loadingService.ShowLoading();

                await App.Current.MainPage.DisplayAlert("Registro exitoso",
                    "El usuario se ha registrado correctamente.", "OK");

                await _loadingService.HideLoading();

                await Shell.Current.GoToAsync("SecurityQuestionPage");


                // Limpiar campos
                UserName = string.Empty;
                Email = string.Empty;
                Password = string.Empty;
                ConfirmPassword = string.Empty;
                SecurityQuestion = string.Empty;
                SecurityAnswer = string.Empty;
            }
            else
            {
                // Mostrar mensaje de error
                await App.Current.MainPage.DisplayAlert("Registro fallido",
                    "No se pudo completar el registro. Por favor, inténtalo de nuevo.", "OK");
            }
        }

        public bool IsPasswordSecure(string password)
        {
            if (password.Length < 6)
            {
                return false;
            }


            if (!password.Any(char.IsUpper))
            {
                return false;
            }


            if (!password.Any(char.IsSymbol) && !password.Any(char.IsPunctuation))
            {
                return false;
            }

            return true;
        }
    }
}