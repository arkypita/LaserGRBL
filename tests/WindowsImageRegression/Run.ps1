[CmdletBinding()]
param([Parameter(Mandatory = $true)][string] $App)

$ErrorActionPreference = 'Stop'
$appPath = (Resolve-Path -LiteralPath $App).Path
$appDirectory = Split-Path -Parent $appPath
$testPath = Join-Path ([IO.Path]::GetTempPath()) ('lasergrbl-images-' + [Guid]::NewGuid().ToString('N'))
$testExe = Join-Path $appDirectory ('LaserGRBL.ImageRegression.' + [Guid]::NewGuid().ToString('N') + '.exe')
$root = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
$tools = Join-Path $root 'artifacts\device-profiles\build-tools'
$framework = Join-Path $tools 'net40-1.0.3\build\.NETFramework\v4.0'
$vswhere = Join-Path ([Environment]::GetFolderPath('ProgramFilesX86')) 'Microsoft Visual Studio\Installer\vswhere.exe'
$compiler = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\Current\Bin\Roslyn\csc.exe' |
    Select-Object -First 1
if (!$compiler -or !(Test-Path -LiteralPath $compiler)) {
    throw 'Visual Studio Roslyn C# compiler was not found.'
}
if (!(Test-Path -LiteralPath $framework)) {
    throw 'Build the application first to download the .NET Framework 4.0 reference assemblies.'
}

New-Item -ItemType Directory -Path $testPath | Out-Null
try {
    $references = @('mscorlib.dll', 'System.dll', 'System.Core.dll', 'System.Drawing.dll', 'System.Windows.Forms.dll',
        'WindowsBase.dll') | ForEach-Object { '/reference:' + (Join-Path $framework $_) }
    $arguments = @('/nologo', '/noconfig', '/nostdlib+', '/target:exe', ('/out:' + $testExe),
        ('/reference:' + $appPath)) + $references + @((Join-Path $PSScriptRoot 'ImageRegression.cs'))
    & $compiler @arguments
    if ($LASTEXITCODE -ne 0) {
        throw "Image regression compilation failed with exit code $LASTEXITCODE."
    }
    & $testExe $testPath
    if ($LASTEXITCODE -ne 0) {
        throw "Image regression failed with exit code $LASTEXITCODE."
    }
}
finally {
    Remove-Item -LiteralPath $testExe -Force -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $testPath -Recurse -Force -ErrorAction SilentlyContinue
}
