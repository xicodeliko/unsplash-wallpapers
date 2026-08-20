[Setup]
AppName=Unsplash Wallpapers
AppVersion=1.0
DefaultDirName={autopf}\UnsplashWallpapers
DefaultGroupName=Unsplash Wallpapers
OutputDir=installer_output
OutputBaseFilename=UnsplashWallpapersSetup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64
SetupIconFile=publish\Resources\tray.ico

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Files]
Source: "publish\UnsplashWallpapers.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\Resources\*"; DestDir: "{app}\Resources"; Flags: ignoreversion recursesubdirs

[Tasks]
Name: "desktopicon"; Description: "Crear acceso directo en el Escritorio"; GroupDescription: "Accesos directos:"
Name: "startupicon"; Description: "Iniciar Unsplash Wallpapers automáticamente con Windows"; GroupDescription: "Opciones de inicio:"; Flags: unchecked

[Icons]
Name: "{group}\Unsplash Wallpapers"; Filename: "{app}\UnsplashWallpapers.exe"
Name: "{commondesktop}\Unsplash Wallpapers"; Filename: "{app}\UnsplashWallpapers.exe"; Tasks: desktopicon

[Registry]
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "UnsplashWallpapers"; ValueData: """{app}\UnsplashWallpapers.exe"""; Tasks: startupicon; Flags: uninsdeletevalue

[Run]
Filename: "{app}\UnsplashWallpapers.exe"; Description: "Ejecutar Unsplash Wallpapers"; Flags: nowait postinstall skipifsilent
