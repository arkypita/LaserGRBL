#ifndef BuildOutput
  #error BuildOutput is required. Run scripts\build-device-profiles.ps1.
#endif

#define AppName "LaserGRBL Device Profiles"
#define AppVersion GetFileVersion(BuildOutput + "\LaserGRBL.exe")

[Setup]
AppId={{94B5254D-5B8D-4844-8B12-1C7F19B60A2C}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher=LaserGRBL Device Profiles contributors
DefaultDirName={localappdata}\Programs\{#AppName}
DisableDirPage=yes
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
OutputBaseFilename=LaserGRBL-Device-Profiles-Setup
Compression=lzma2
SolidCompression=yes
SetupIconFile=install.ico
UninstallDisplayIcon={app}\LaserGRBL.exe
LicenseFile=LICENSE.md

[Files]
Source: "{#BuildOutput}\LaserGRBL.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildOutput}\LaserGRBL.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildOutput}\LaserGRBL.resources.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "{#BuildOutput}\Autotrace\autotrace.exe"; DestDir: "{app}\Autotrace"; Flags: ignoreversion
Source: "{#BuildOutput}\Driver\*"; DestDir: "{app}\Driver"; Flags: ignoreversion
Source: "{#BuildOutput}\Firmware\*"; DestDir: "{app}\Firmware"; Flags: ignoreversion
Source: "{#BuildOutput}\Sound\*"; DestDir: "{app}\Sound"; Flags: ignoreversion
Source: "{#BuildOutput}\StandardButtons.zbn"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildOutput}\StandardMaterials.psh"; DestDir: "{userappdata}\{#AppName}"; Flags: onlyifdoesntexist uninsneveruninstall
Source: "LICENSE.md"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{userprograms}\{#AppName}"; Filename: "{app}\LaserGRBL.exe"; WorkingDir: "{app}"
Name: "{userprograms}\{#AppName} (disable OpenGL)"; Filename: "{app}\LaserGRBL.exe"; Parameters: "nogl"; WorkingDir: "{app}"
