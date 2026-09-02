using Microsoft.Maui.Controls;
using EscobarMatias_DesarrolloAppMovil_II.ViewModels;

namespace EscobarMatias_DesarrolloAppMovil_II.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Se instancia el ViewModel y lo asignamos como contexto de datos
            BindingContext = new ProfileViewModel();
        }
    }
}