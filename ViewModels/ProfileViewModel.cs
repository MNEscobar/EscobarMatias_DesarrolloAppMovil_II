using EscobarMatias_DesarrolloAppMovil_II.Helpers;
using EscobarMatias_DesarrolloAppMovil_II.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EscobarMatias_DesarrolloAppMovil_II.ViewModels
{
    class ProfileViewModel : INotifyPropertyChanged
    {
        // Colores del estado del sistema (badge de arriba). Se definen acá,
        // no en el XAML, para que el ViewModel no dependa de recursos de la Vista.
        private static readonly Color SavingColor = Color.FromArgb("#512BD4"); // igual al Primary de la app
        private static readonly Color ErrorColor = Color.FromArgb("#D32F2F");
        private static readonly Color PendingColor = Color.FromArgb("#F2A93B");
        private static readonly Color SavedColor = Color.FromArgb("#3DA35D");

        private UserProfile _userModel;

        // Campos de respaldo
        private string? _name;
        private string? _ageText;
        private string? _description;
        private string? _nameError;
        private string? _ageError;
        private string? _descriptionError;
        private bool _isBusy;

        // true mientras se cargan los valores iniciales, para no marcar el
        // formulario como "con cambios sin guardar" apenas se abre la app.
        private bool _isInitializing;

        // true desde que el usuario edita algo hasta que se guarda con éxito.
        private bool _isDirty;

        // Propiedades públicas para el Binding.
        // Cada setter valida su propio campo al instante y actualiza el estado general.
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                NameError = ProfileValidator.ValidateName(_name);
                MarkAsEdited();
            }
        }

        // La edad se maneja como texto (no int) porque el Entry siempre entrega
        // un string: si se enlaza un int directo, al borrar el campo la conversión
        // implícita de MAUI falla en silencio y el ViewModel queda desincronizado.
        // Acá el parseo lo controlamos nosotros en ProfileValidator.
        public string? AgeText
        {
            get => _ageText;
            set
            {
                _ageText = value;
                OnPropertyChanged();
                AgeError = ProfileValidator.ValidateAge(_ageText, out _);
                MarkAsEdited();
            }
        }

        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
                DescriptionError = ProfileValidator.ValidateDescription(_description);
                MarkAsEdited();
            }
        }

        public string? ProfileImageUrl => _userModel.ProfileImageUrl;

        // Errores puntuales por campo, para mostrarlos debajo de cada control
        // y para pintar de rojo el borde del campo correspondiente (ver MainPage.xaml).
        public string? NameError
        {
            get => _nameError;
            private set { _nameError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasNameError)); }
        }

        public string? AgeError
        {
            get => _ageError;
            private set { _ageError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasAgeError)); }
        }

        public string? DescriptionError
        {
            get => _descriptionError;
            private set { _descriptionError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasDescriptionError)); }
        }

        public bool HasNameError => !string.IsNullOrEmpty(NameError);
        public bool HasAgeError => !string.IsNullOrEmpty(AgeError);
        public bool HasDescriptionError => !string.IsNullOrEmpty(DescriptionError);

        private bool IsFormValid => !HasNameError && !HasAgeError && !HasDescriptionError;

        // true mientras se está "guardando" (ver OnSaveClickedAsync). Deshabilita
        // el formulario y el botón, y hace aparecer el ActivityIndicator del badge.
        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
                NotifyFormStateChanged();
            }
        }

        public bool IsNotBusy => !IsBusy;

        // Estado general del sistema, en una sola fuente de verdad, con prioridad:
        // 1) Guardando  2) Hay errores  3) Cambios sin guardar  4) Todo guardado
        public string StatusText
        {
            get
            {
                if (IsBusy) return "Guardando cambios...";
                if (!IsFormValid) return "Hay datos para corregir";
                if (_isDirty) return "Cambios sin guardar";
                return "Todo guardado";
            }
        }

        public Color StatusColor
        {
            get
            {
                if (IsBusy) return SavingColor;
                if (!IsFormValid) return ErrorColor;
                if (_isDirty) return PendingColor;
                return SavedColor;
            }
        }

        // Se guarda como Command (no solo ICommand) porque necesitamos llamar
        // a ChangeCanExecute() cada vez que cambia la validez o el estado "ocupado".
        private readonly Command _saveCommand;
        public ICommand SaveCommand => _saveCommand;

        public ProfileViewModel()
        {
            _isInitializing = true;

            // Simula obtener los datos del modelo (ej. desde una base de datos)
            _userModel = new UserProfile
            {
                Name = "Juan Pérez",
                Age = 25,
                Description = "Estudiante apasionado por el código limpio.",
                ProfileImageUrl = "https://dotnet.microsoft.com/static/images/redesign/social/square.png"
            };

            // El Command es async: OnSaveClickedAsync simula una operación real
            // (ej. guardar en base de datos) para poder mostrar el estado "Guardando...".
            _saveCommand = new Command(async () => await OnSaveClickedAsync(), CanSave);

            // Carga los datos iniciales. Al pasar por los setters de arriba ya se
            // dispara la validación, pero como _isInitializing es true todavía,
            // no se marca el formulario como "con cambios sin guardar".
            Name = _userModel.Name;
            AgeText = _userModel.Age.ToString();
            Description = _userModel.Description;

            _isInitializing = false;
        }

        private bool CanSave() => IsFormValid && !IsBusy;

        // Se llama desde cada setter editable: guarda la marca de "sucio" (si
        // corresponde) y avisa tanto al Command como al badge de estado.
        private void MarkAsEdited()
        {
            if (!_isInitializing)
                _isDirty = true;

            NotifyFormStateChanged();
        }

        private void NotifyFormStateChanged()
        {
            _saveCommand?.ChangeCanExecute();
            OnPropertyChanged(nameof(StatusText));
            OnPropertyChanged(nameof(StatusColor));
        }

        private async Task OnSaveClickedAsync()
        {
            // Revalidamos todo por las dudas (el botón ya debería estar deshabilitado
            // si algo es inválido, esto es solo una defensa extra).
            NameError = ProfileValidator.ValidateName(Name);
            AgeError = ProfileValidator.ValidateAge(AgeText, out int parsedAge);
            DescriptionError = ProfileValidator.ValidateDescription(Description);

            if (!IsFormValid)
            {
                NotifyFormStateChanged();
                return;
            }

            IsBusy = true;

            // Simula una operación de guardado real (ej. escritura en base de datos
            // o una llamada a una API). Es lo que permite ver el estado "Guardando...".
            await Task.Delay(600);

            // Actualizamos el modelo con los datos ya validados
            _userModel.Name = Name;
            _userModel.Age = parsedAge;
            _userModel.Description = Description;

            _isDirty = false;
            IsBusy = false; // dispara NotifyFormStateChanged() -> queda en "Todo guardado"
        }

        // Boilerplate de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}