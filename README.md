# Unsplash Wallpapers

Aplicación WPF para buscar fotos en Unsplash y establecerlas como fondo de pantalla.

## Requisitos

- Windows
- .NET 8 SDK
- Una Access Key de Unsplash

## Configurar la clave

La clave no está guardada en el repositorio. El proyecto usa .NET User Secrets mediante el `UserSecretsId` del archivo `.csproj`.

En cada equipo, configura la clave con:

```powershell
dotnet user-secrets set "Unsplash:AccessKey" "TU_ACCESS_KEY" --project .
```

Después ejecuta:

```powershell
dotnet run
```

Los User Secrets se almacenan fuera de la carpeta del proyecto y no deben subirse a GitHub.

## Seguridad

Si una clave se publica accidentalmente, revócala desde el panel de Unsplash y genera una nueva inmediatamente.