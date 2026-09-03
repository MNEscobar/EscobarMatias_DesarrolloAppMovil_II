using EscobarMatias_DesarrolloAppMovil_II.ViewModels;

namespace EscobarMatias_DesarrolloAppMovil_II.Views
{
    public partial class PerfilPage : ContentPage
    {
        public PerfilPage(PerfilViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        // No es navegación: solo le pide al ViewModel que refresque el nombre
        // mostrado cada vez que se entra a este tab (por si el ViewModel se
        // construyó antes de que el login terminara de escribir el nombre).
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PerfilViewModel viewModel)
                viewModel.RefreshFromSession();
        }
    }
}