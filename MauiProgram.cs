using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using EscobarMatias_DesarrolloAppMovil_II.Models;
using EscobarMatias_DesarrolloAppMovil_II.ViewModels;
using EscobarMatias_DesarrolloAppMovil_II.Views;

namespace EscobarMatias_DesarrolloAppMovil_II
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit() // Habilita Toast/Snackbar
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // -----------------------------------------------------------------
            // INYECCIÓN DE DEPENDENCIAS
            // -----------------------------------------------------------------
            // UserProfile como Singleton: es el "estado de sesión" compartido.
            // LoginViewModel escribe el nombre ahí, PerfilViewModel lo lee: al
            // ser la MISMA instancia durante toda la vida de la app, no hace
            // falta pasarlo a mano por parámetros de navegación.
            builder.Services.AddSingleton<UserProfile>();

            // AppShell como Singleton: se construye una sola vez (con sus
            // rutas ya registradas) y LoginViewModel lo reutiliza para
            // "montarlo" como raíz de la ventana al loguearse con éxito.
            builder.Services.AddSingleton<AppShell>();

            // Páginas/ViewModels de las pestañas: Singleton, viven mientras
            // vive la app (igual que las pestañas de un TabBar real).
            builder.Services.AddSingleton<CatalogoViewModel>();
            builder.Services.AddSingleton<CatalogoPage>();
            builder.Services.AddSingleton<PerfilViewModel>();
            builder.Services.AddSingleton<PerfilPage>();

            // Páginas "de paso" (Login, Detalle, Modal): Transient, se crean
            // de nuevo en cada navegación y no quedan colgadas en memoria.
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<DetalleProductoViewModel>();
            builder.Services.AddTransient<DetalleProductoPage>();
            builder.Services.AddTransient<ConfirmacionModalViewModel>();
            builder.Services.AddTransient<ConfirmacionModalPage>();

            return builder.Build();
        }
    }
}