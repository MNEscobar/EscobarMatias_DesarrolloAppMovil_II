using EscobarMatias_DesarrolloAppMovil_II.ViewModels;

namespace EscobarMatias_DesarrolloAppMovil_II.Views
{
    public partial class ConfirmacionModalPage : ContentPage
    {
        public ConfirmacionModalPage(ConfirmacionModalViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}