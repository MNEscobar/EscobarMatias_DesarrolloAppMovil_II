using EscobarMatias_DesarrolloAppMovil_II.Views;

namespace EscobarMatias_DesarrolloAppMovil_II
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // -----------------------------------------------------------------
            // REGISTRO DE RUTAS
            // -----------------------------------------------------------------
            // Las páginas de las pestañas (Catálogo/Perfil) ya quedan
            // registradas implícitamente por estar declaradas en el TabBar de arriba.
            // Las páginas "secundarias" -a las que se llega con GoToAsync y que NO son pestañas-
            // hay que registrarlas acá a mano, para que Shell sepa qué Page mostrar para cada ruta.
            Routing.RegisterRoute(nameof(DetalleProductoPage), typeof(DetalleProductoPage));
            Routing.RegisterRoute(nameof(ConfirmacionModalPage), typeof(ConfirmacionModalPage));
        }
    }
}