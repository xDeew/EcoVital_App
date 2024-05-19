# EcoVital

## Configuración

Para usar el chatbot, necesitas una clave de API de OpenAI. Sigue estos pasos para configurar tu clave:

1. Crea una carpeta llamada `Config` en la raíz del proyecto.
2. Dentro de la carpeta `Config`, crea un archivo `ApiConfig.cs`.
3. Define la clase `ApiConfig` en `ApiConfig.cs` con el siguiente contenido:

```csharp
namespace EcoVital.Config
{
    public static class ApiConfig
    {
        public static string ApiKey { get; } = "sk-xxxxxx"; // Reemplaza "sk-xxxxxx" con tu clave real
    }
}
