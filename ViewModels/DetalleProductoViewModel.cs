using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using EscobarMatias_DesarrolloAppMovil_II.Models;
using EscobarMatias_DesarrolloAppMovil_II.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EscobarMatias_DesarrolloAppMovil_II.ViewModels
{
    // IQueryAttributable permite recibir TODOS los parámetros de la URL de
    // navegación en un solo método (ApplyQueryAttributes), a diferencia del
    // atributo [QueryProperty] que solo sirve para un valor fijo. Lo usamos acá
    // porque esta página recibe parámetros en DOS momentos distintos de su
    // ciclo de vida: el "id" al llegar desde el Catálogo, y "confirmado" al
    // volver desde el modal de confirmación.
    public class DetalleProductoViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private Product? _producto;
        public Product? Producto
        {
            get => _producto;
            private set { _producto = value; OnPropertyChanged(); OnPropertyChanged(nameof(TieneProducto)); }
        }

        public bool TieneProducto => Producto is not null;

        private string? _mensajeError;
        public string? MensajeError
        {
            get => _mensajeError;
            private set { _mensajeError = value; OnPropertyChanged(); OnPropertyChanged(nameof(TieneError)); }
        }

        public bool TieneError => !string.IsNullOrEmpty(MensajeError);

        private readonly Command _volverCommand;
        public ICommand VolverCommand => _volverCommand;

        private readonly Command _abrirConfirmacionCommand;
        public ICommand AbrirConfirmacionCommand => _abrirConfirmacionCommand;

        public DetalleProductoViewModel()
        {
            // Navegación jerárquica hacia atrás: ".."
            // le dice a Shell que saque esta página de su pila de navegación
            // y muestre la anterior (el Catálogo).
            _volverCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            _abrirConfirmacionCommand = new Command(async () => await AbrirConfirmacionAsync());
        }

        // ---------------------------------------------------------------
        // RECEPCIÓN DE PARÁMETROS DE NAVEGACIÓN
        // ---------------------------------------------------------------
        // Shell llama a este método automáticamente cada vez que se navega
        // HACIA esta página: tanto al llegar por primera vez desde el
        // Catálogo (trae "id"), como al volver desde ConfirmacionModalPage
        // (trae "confirmado"). El diccionario "query" puede traer una,
        // otra, o ninguna de las dos claves según el caso.
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("id", out var idValor))
            {
                CargarProducto(idValor?.ToString());
            }

            if (query.TryGetValue("confirmado", out var confirmadoValor))
            {
                _ = AplicarResultadoConfirmacionAsync(confirmadoValor);
            }
        }

        private void CargarProducto(string? idTexto)
        {
            // Validación: el id no puede ser nulo, vacío, ni un valor no numérico.
            if (string.IsNullOrWhiteSpace(idTexto) || !int.TryParse(idTexto, out int id))
            {
                MensajeError = "El producto solicitado no es válido.";
                Producto = null;
                return;
            }

            // Búsqueda en los datos mock, compartidos con CatalogoViewModel.
            var encontrado = CatalogoViewModel.MockProducts.FirstOrDefault(p => p.Id == id);

            if (encontrado is null)
            {
                MensajeError = $"No se encontró un producto con el id {id}.";
                Producto = null;
                return;
            }

            MensajeError = null;
            Producto = encontrado;
        }

        private async Task AbrirConfirmacionAsync()
        {
            if (Producto is null) return;

            // Escapamos el mensaje porque va dentro de una URI (puede tener
            // espacios, comillas, signos de pregunta, etc.).
            var mensaje = Uri.EscapeDataString($"¿Confirmás la compra de \"{Producto.Nombre}\"?");
            await Shell.Current.GoToAsync($"{nameof(ConfirmacionModalPage)}?mensaje={mensaje}");
        }

        private async Task AplicarResultadoConfirmacionAsync(object valor)
        {
            // Los valores que viajan por query parameters de Shell llegan
            // boxeados en "object"; los convertimos con cuidado en vez de
            // castear directo.
            bool confirmado = valor is bool b && b;

            if (confirmado)
            {
                // Acá iría la lógica real de "Comprar"/"Eliminar" sobre Producto.
                await Toast.Make("Acción confirmada.").Show();
            }
            else
            {
                await Toast.Make("Acción cancelada.").Show();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}