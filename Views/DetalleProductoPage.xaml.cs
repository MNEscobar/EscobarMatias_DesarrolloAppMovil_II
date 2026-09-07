using EscobarMatias_DesarrolloAppMovil_II.ViewModels;

namespace EscobarMatias_DesarrolloAppMovil_II.Views
{
    public partial class DetalleProductoPage : ContentPage
    {
        public DetalleProductoPage(DetalleProductoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}