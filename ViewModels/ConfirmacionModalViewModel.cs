using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace EscobarMatias_DesarrolloAppMovil_II.ViewModels
{
    public class ConfirmacionModalViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private string? _mensaje;
        public string? Mensaje
        {
            get => _mensaje;
            private set { _mensaje = value; OnPropertyChanged(); }
        }

        private readonly Command _confirmarCommand;
        public ICommand ConfirmarCommand => _confirmarCommand;

        private readonly Command _cancelarCommand;
        public ICommand CancelarCommand => _cancelarCommand;

        public ConfirmacionModalViewModel()
        {
            _confirmarCommand = new Command(async () => await CerrarAsync(confirmado: true));
            _cancelarCommand = new Command(async () => await CerrarAsync(confirmado: false));
        }

        // Recibe el mensaje a mostrar; llega desde DetalleProductoViewModel
        // (un solo parámetro simple, por eso alcanza con IQueryAttributable
        // básico en vez de necesitar [QueryProperty] adicional).
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("mensaje", out var valor))
                Mensaje = valor?.ToString();
        }

        private async Task CerrarAsync(bool confirmado)
        {
            if (confirmado)
                await Toast.Make("Confirmaste la acción.").Show();
            else
                await Toast.Make("Cancelaste la acción.").Show();

            // -----------------------------------------------------------------
            // NAVEGACIÓN MODAL: volvemos a DetalleProductoPage pasando el resultado
            // como parámetro de navegación hacia atrás.
            // DetalleProductoViewModel lo recibe en su propio
            // ApplyQueryAttributes y decide ahí si "aplica" o "ignora" el cambio
            // sin tocar el Code-Behind de ninguna página.
            // -----------------------------------------------------------------
            var parametros = new Dictionary<string, object> { { "confirmado", confirmado } };
            await Shell.Current.GoToAsync("..", parametros);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public interface IAppNavigator
    {
        Task SetMainShellAsync(Page shell);
        Task ShowToastAsync(string message);
    }

    public class AppNavigator : IAppNavigator
    {
        public Task SetMainShellAsync(Page shell) =>
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                // opción más clara que Windows[0].Page
                Application.Current!.MainPage = shell;
            });

        public Task ShowToastAsync(string message) =>
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                try { await Toast.Make(message).Show(); }
                catch { /* log o fallback a DisplayAlert */ }
            });
    }
}