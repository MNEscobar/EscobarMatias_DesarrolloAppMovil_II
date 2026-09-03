using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EscobarMatias_DesarrolloAppMovil_II.Helpers
{
    // Reglas de validación del Login, separadas del ViewModel (mismo criterio que ProfileValidator.cs).
    public static class LoginValidator
    {
        // Regex básica de formato de correo: no es exhaustiva a nivel RFC, pero
        // alcanza para detectar los errores típicos de un formulario de login
        // (falta la arroba, falta el dominio, espacios sueltos, etc.).
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public static string? ValidateNombre(string? nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return "Ingresá tu nombre.";
            return null;
        }

        public static string? ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "El correo no puede estar vacío.";
            if (!EmailRegex.IsMatch(email.Trim()))
                return "El formato del correo no es válido.";
            return null;
        }

        public static string? ValidatePassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return "La contraseña no puede estar vacía.";
            if (password.Length < 8)
                return "La contraseña debe tener al menos 8 caracteres.";
            return null;
        }
    }
}