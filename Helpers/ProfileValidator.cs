using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscobarMatias_DesarrolloAppMovil_II.Helpers
{
    // Reglas de validación del perfil, separadas del ViewModel para que
    // no se llene de condiciones y sea más fácil de mantener.
    class ProfileValidator
    {
        public const int MinAge = 1;
        public const int MaxAge = 120;
        public const int MaxDescriptionLength = 200;

        // Devuelve null si es válido, o el mensaje de error a mostrar si no lo es.
        public static string? ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "El nombre no puede estar vacío.";
            }
            if (name.Trim().Length < 2)
            {
                return "El nombre debe tener al menos 2 caracteres.";
            }
            return null;
        }

        // Recibe el texto tal cual lo escribe el usuario
        // y devuelve además el valor numérico ya parseado en parsedAge.
        public static string? ValidateAge(string? ageText, out int parsedAge)
        {
            if (string.IsNullOrWhiteSpace(ageText))
            {
                parsedAge = 0;
                return "La edad no puede estar vacía.";
            }

            if (!int.TryParse(ageText, out parsedAge))
                return "La edad debe ser un número entero válido.";

            if (parsedAge < MinAge || parsedAge > MaxAge)
                return $"La edad debe estar entre {MinAge} y {MaxAge} años.";

            return null;
        }

        public static string? ValidateDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return "La descripción no puede estar vacía.";

            if (description.Trim().Length > MaxDescriptionLength)
                return $"La descripción no puede superar los {MaxDescriptionLength} caracteres.";

            return null;
        }
    }
}
