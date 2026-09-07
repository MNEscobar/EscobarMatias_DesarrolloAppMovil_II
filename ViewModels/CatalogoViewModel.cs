using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EscobarMatias_DesarrolloAppMovil_II.Models;
using EscobarMatias_DesarrolloAppMovil_II.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EscobarMatias_DesarrolloAppMovil_II.ViewModels
{
    public class CatalogoViewModel : INotifyPropertyChanged
    {
        // -------------------------------------------------------------------
        // DATOS MOCK: en una app real esto vendría de una API o una base de datos.
        // Se define acá, en el ViewModel del Catálogo.
        // DetalleProductoViewModel reutiliza esta MISMA lista (vía CatalogoViewModel.MockProducts)
        // par poder buscar un producto por Id sin duplicar los datos.
        // -------------------------------------------------------------------
        public static readonly List<Product> MockProducts = new()
        {
            new Product { Id = 1, Nombre = "Auriculares Inalámbricos", Descripcion = "Cancelación de ruido activa, 30 hs de batería.", Precio = 45999m, ImagenUrl = "https://picsum.photos/seed/1/400/400" },
            new Product { Id = 2, Nombre = "Teclado Mecánico", Descripcion = "Switches rojos, retroiluminado RGB.", Precio = 38500m, ImagenUrl = "https://picsum.photos/seed/2/400/400" },
            new Product { Id = 3, Nombre = "Mouse Ergonómico", Descripcion = "Sensor óptico de alta precisión, inalámbrico.", Precio = 21990m, ImagenUrl = "https://picsum.photos/seed/3/400/400" },
            new Product { Id = 4, Nombre = "Monitor 27\" 144Hz", Descripcion = "Panel IPS, ideal para diseño y gaming.", Precio = 289999m, ImagenUrl = "https://picsum.photos/seed/4/400/400" },
            new Product { Id = 5, Nombre = "Webcam Full HD", Descripcion = "1080p con micrófono integrado.", Precio = 32999m, ImagenUrl = "https://picsum.photos/seed/5/400/400" },
        };

        public ObservableCollection<Product> Productos { get; }

        private Product? _productoSeleccionado;
        public Product? ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                if (ReferenceEquals(_productoSeleccionado, value)) return;
                _productoSeleccionado = value;
                OnPropertyChanged();

                if (value is not null)
                    _ = AbrirDetalleAsync(value);
            }
        }

        public CatalogoViewModel()
        {
            Productos = new ObservableCollection<Product>(MockProducts);
        }

        private async Task AbrirDetalleAsync(Product producto)
        {
            // -----------------------------------------------------------------
            // NAVEGACIÓN: se pasa ÚNICAMENTE el Id por la URI, no el objeto Product completo.
            // Esto desacopla el detalle del catálogo (podría llegarse a esta misma pantalla desde un
            // deep link externo, una notificación, etc., sin depender de que el objeto ya esté en memoria).
            // -----------------------------------------------------------------
            await Shell.Current.GoToAsync($"{nameof(DetalleProductoPage)}?id={producto.Id}");

            // Limpiamos la selección para poder volver a tocar el mismo ítem
            // más adelante (CollectionView no dispara el binding si el valor
            // "nuevo" es el mismo objeto que ya estaba seleccionado).
            ProductoSeleccionado = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}