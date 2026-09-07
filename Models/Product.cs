using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscobarMatias_DesarrolloAppMovil_II.Models
{
    // Modelo simple del catálogo. Público (a diferencia de UserProfile) porque
    // CatalogoViewModel y DetalleProductoViewModel lo comparten directamente.
    public class Product
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }

        // URL remota de ejemplo (picsum.photos). Solo el ícono del carrito y la
        // silueta de perfil son recursos LOCALES.
        // las imágenes de producto podrían venir de una API real más adelante.
        public string ImagenUrl { get; set; } = string.Empty;
    }
}