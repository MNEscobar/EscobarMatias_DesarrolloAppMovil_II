using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using EscobarMatias_DesarrolloAppMovil_II.Helpers;
using EscobarMatias_DesarrolloAppMovil_II.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EscobarMatias_DesarrolloAppMovil_II.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly UserProfile _userProfile;
        private readonly AppShell _appShell;

        private string? _nombre;
        private string? _email;
        private string? _password;
        private string? _nombreError;
        private string? _emailError;
        private string? _passwordError;
        private bool _isBusy;

        public string? Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); NombreError = LoginValidator.ValidateNombre(value); RefreshCanLogin(); }
        }

        public string? Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); EmailError = LoginValidator.ValidateEmail(value); RefreshCanLogin(); }
        }

        public string? Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); PasswordError = LoginValidator.ValidatePassword(value); RefreshCanLogin(); }
        }

        public string? NombreError { get => _nombreError; private set { _nombreError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasNombreError)); } }
        public string? EmailError { get => _emailError; private set { _emailError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasEmailError)); } }
        public string? PasswordError { get => _passwordError; private set { _passwordError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasPasswordError)); } }

        public bool HasNombreError => !string.IsNullOrEmpty(NombreError);
        public bool HasEmailError => !string.IsNullOrEmpty(EmailError);
        public bool HasPasswordError => !string.IsNullOrEmpty(PasswordError);

        private bool IsFormValid => !HasNombreError && !HasEmailError && !HasPasswordError;

        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; OnPropertyChanged(); RefreshCanLogin(); }
        }

        private readonly Command _loginCommand;
        public ICommand LoginCommand => _loginCommand;

        // UserProfile y AppShell llegan inyectados como Singleton (ver
        // MauiProgram.cs). Al ser la MISMA instancia de UserProfile que después
        // recibe PerfilViewModel, con solo escribir el nombre acá alcanza para
        // que el tab Perfil ya lo muestre, sin pasarlo por parámetros de navegación.
        public LoginViewModel(UserProfile userProfile, AppShell appShell)
        {
            _userProfile = userProfile;
            _appShell = appShell;
            _loginCommand = new Command(async () => await ExecuteLoginAsync(), CanLogin);

            // Validación inicial: con el formulario vacío, el botón arranca deshabilitado.
            NombreError = LoginValidator.ValidateNombre(Nombre);
            EmailError = LoginValidator.ValidateEmail(Email);
            PasswordError = LoginValidator.ValidatePassword(Password);
        }

        private bool CanLogin() => IsFormValid && !IsBusy;
        private void RefreshCanLogin() => _loginCommand.ChangeCanExecute();

        private async Task ExecuteLoginAsync()
        {
            // Revalidación defensiva (el botón ya debería estar deshabilitado si algo falta).
            NombreError = LoginValidator.ValidateNombre(Nombre);
            EmailError = LoginValidator.ValidateEmail(Email);
            PasswordError = LoginValidator.ValidatePassword(Password);

            if (!IsFormValid)
            {
                await Toast.Make("Revisá los datos marcados en rojo.").Show();
                return;
            }

            IsBusy = true;
            await Task.Delay(500); // Simula una autenticación real contra un servidor.

            // Transmitimos el nombre ingresado al modelo UserProfile.
            _userProfile.Name = Nombre!.Trim();

            await Toast.Make("Inicio de sesión exitoso.").Show();

            // -----------------------------------------------------------------
            // ÚNICA EXCEPCIÓN a la regla "toda navegación con Shell.Current.GoToAsync":
            // hasta este punto la app corre en LoginPage, FUERA del Shell (Shell.Current
            // todavía no existe como raíz de la ventana, así que no hay nada a lo
            // que hacer GoToAsync). Este paso no es una "navegación" entre páginas
            // de Shell: es el arranque del Shell en sí, reemplazando la página raíz
            // de la ventana por el AppShell ya armado (inyectado por DI).
            // A partir de acá, TODA la navegación (Catálogo, Perfil, Detalle, Modal)
            // sí pasa exclusivamente por Shell.Current.GoToAsync() desde los ViewModels.
            // -----------------------------------------------------------------
            Application.Current!.Windows[0].Page = _appShell;

            IsBusy = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}