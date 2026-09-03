using EscobarMatias_DesarrolloAppMovil_II.ViewModels;

namespace EscobarMatias_DesarrolloAppMovil_II.Views
{
    public partial class CatalogoPage : ContentPage
    {
        public CatalogoPage(CatalogoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        // El ÚNICO código que corre acá es una animación puramente visual del
        // ícono del carrito. No es navegación
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = AnimateCartIconAsync();
        }

        private async Task AnimateCartIconAsync()
        {
            // Extensiones de animación de MAUI (ScaleTo/RotateTo): un pequeño
            // "rebote" del carrito al entrar a la página.
            await CartIcon.ScaleTo(1.3, 200, Easing.CubicOut);
            await CartIcon.RotateTo(-15, 100);
            await CartIcon.RotateTo(15, 150);
            await CartIcon.RotateTo(0, 100);
            await CartIcon.ScaleTo(1, 150, Easing.CubicIn);
        }
    }
}