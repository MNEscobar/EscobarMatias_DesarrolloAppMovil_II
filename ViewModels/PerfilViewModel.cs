using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using EscobarMatias_DesarrolloAppMovil_II.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EscobarMatias_DesarrolloAppMovil_II.ViewModels
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        private readonly UserProfile _userProfile;

        // Nombre capturado en el Login. UserProfile llega inyectado como Singleton
        // (ver MauiProgram.cs) y es la MISMA instancia que LoginViewModel completó al validar el login.
        public string NombreUsuario =>
            string.IsNullOrWhiteSpace(_userProfile.Name) ? "Invitado" : _userProfile.Name;

        // Silueta clásica por defecto (recurso LOCAL, ver Resources/Images)
        // hasta que el usuario elija una foto propia.
        private string _fotoPerfil = "profile_placeholder.svg";
        public string FotoPerfil
        {
            get => _fotoPerfil;
            private set { _fotoPerfil = value; OnPropertyChanged(); }
        }

        private readonly Command _editarFotoCommand;
        public ICommand EditarFotoCommand => _editarFotoCommand;

        public PerfilViewModel(UserProfile userProfile)
        {
            _userProfile = userProfile;
            _editarFotoCommand = new Command(async () => await EditarFotoAsync());
        }

        private async Task EditarFotoAsync()
        {
            // Comando "preparado": queda listo para conectarse más adelante con
            // MediaPicker.PickPhotoAsync() (elegir de la galería) o MediaPicker.CapturePhotoAsync()
            // (sacar una foto nueva). Por ahora solo confirma con un Toast que el flujo
            // ya está enganchado de punta a punta.
            await Toast.Make("Función de editar foto lista para conectar con MediaPicker.").Show();
        }

        // Se llama desde PerfilPage.xaml.cs en OnAppearing (ver comentario ahí):
        // no es navegación, solo refresca el binding por si el nombre cambió
        // (por ejemplo, si Shell ya había construido este ViewModel antes del login).
        public void RefreshFromSession()
        {
            OnPropertyChanged(nameof(NombreUsuario));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}