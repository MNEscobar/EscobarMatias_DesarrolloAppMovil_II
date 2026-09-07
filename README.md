# Catálogo de Productos — EscobarMatias_DesarrolloAppMovil_II

Aplicación desarrollada en **.NET MAUI** para la materia Desarrollo de Aplicaciones Móviles II. Implementa un flujo completo de Login, un catálogo de productos con navegación jerárquica y modal, y un perfil de usuario, siguiendo el patrón **MVVM** de punta a punta.

## Tecnologías

- .NET MAUI (net9.0)
- C# / XAML
- [CommunityToolkit.Maui](https://github.com/CommunityToolkit/Maui)
- Inyección de dependencias nativa de .NET (`Microsoft.Extensions.DependencyInjection`)

## Funcionalidades

- **Login bloqueante**, fuera del Shell, con validación de nombre, formato de correo y contraseña (mínimo 8 caracteres).
- **Navegación con Shell**: dos pestañas principales (Catálogo y Perfil) y rutas registradas para páginas secundarias.
- **Catálogo de productos** con datos simulados (mock) y un ícono de carrito animado al cargar la página.
- **Detalle de producto**, con validación del parámetro recibido por navegación y búsqueda en los datos mock.
- **Confirmación mediante página modal**, que devuelve el resultado (Sí/No) a la página anterior sin usar Code-Behind.
- **Perfil de usuario**, con el nombre capturado en el Login y una foto por defecto (recurso local).
- **Feedback visual** con Toasts en los eventos clave (login exitoso, errores de validación, confirmación/cancelación).

Toda la navegación se maneja desde los ViewModels mediante `Shell.Current.GoToAsync()`.

## Estructura del proyecto

```
Models/       Modelos de datos (UserProfile, Product)
ViewModels/   Lógica de presentación, validación y navegación
Views/        Páginas XAML (Login, Catálogo, Detalle, Modal, Perfil)
Helpers/      Validadores (ProfileValidator, LoginValidator)
Resources/    Estilos, fuentes e imágenes (incluye recursos locales del carrito y la silueta de perfil)
```

## Cómo ejecutar

1. Clonar el repositorio.
2. Abrir `EscobarMatias_DesarrolloAppMovil_II.sln` en Visual Studio 2022 (con la carga de trabajo de .NET MAUI instalada).
3. Restaurar los paquetes NuGet.
4. Seleccionar la plataforma de destino (Android, Windows, etc.) y ejecutar.