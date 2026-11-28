[CmdletBinding()]
param([Parameter(Mandatory = $true)][string] $App)

$ErrorActionPreference = 'Stop'
$appPath = (Resolve-Path -LiteralPath $App).Path
$testPath = Join-Path ([IO.Path]::GetTempPath()) ('lasergrbl-images-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $testPath | Out-Null
Push-Location -LiteralPath $testPath
$previousCurrentDirectory = [IO.Directory]::GetCurrentDirectory()
try {
    [IO.Directory]::SetCurrentDirectory($testPath)
    [Reflection.Assembly]::LoadFrom($appPath) | Out-Null
    Add-Type -AssemblyName WindowsBase
    Add-Type -Path (Join-Path $PSScriptRoot 'ImageRegression.cs') -ReferencedAssemblies @(
        'System.dll', 'System.Core.dll', 'System.Drawing.dll', 'System.Windows.Forms.dll', [System.Windows.Point].Assembly.Location, $appPath
    )
    [ImageRegression]::Run($testPath)
}
finally {
    [IO.Directory]::SetCurrentDirectory($previousCurrentDirectory)
    Pop-Location
    Remove-Item -LiteralPath $testPath -Recurse -Force
}
