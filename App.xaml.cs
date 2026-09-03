using EscobarMatias_DesarrolloAppMovil_II.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EscobarMatias_DesarrolloAppMovil_II
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        // MAUI resuelve App a través del mismo contenedor de Inyección de
        // Dependencias que se configuro en MauiProgram.cs, así que podimos
        // por constructor cualquier servicio ya registrado ahí.
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // La app arranca en LoginPage, FUERA del Shell
            // AppShell recién se monta como raíz de la ventana
            // cuando el login es válido (ver LoginViewModel.ExecuteLoginAsync).
            var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
            return new Window(loginPage);
        }
    }
}