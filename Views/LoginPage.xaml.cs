using EscobarMatias_DesarrolloAppMovil_II.ViewModels;

namespace EscobarMatias_DesarrolloAppMovil_II.Views
{
    // Vive FUERA del Shell: es el punto de entrada de la app.
    // No hay NINGUNA lógica de navegación acá; eso lo resuelve
    // LoginViewModel por completo (incluyendo el paso a AppShell).
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
