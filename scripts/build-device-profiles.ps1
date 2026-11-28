[CmdletBinding()]
param(
    [string] $MSBuildPath,
    [string] $InnoCompiler,
    [switch] $SkipInstaller
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'
$root = Split-Path -Parent $PSScriptRoot
$artifacts = Join-Path $root 'artifacts\device-profiles'
$tools = Join-Path $artifacts 'build-tools'
$appOutput = Join-Path $artifacts 'app'
$objOutput = Join-Path $artifacts 'obj'
$installerOutput = Join-Path $artifacts 'installer'
$null = New-Item -ItemType Directory -Path $tools -Force

function Get-VerifiedDownload([string] $Uri, [string] $Path, [string] $Sha256) {
    if (!(Test-Path -LiteralPath $Path) -or (Get-FileHash -LiteralPath $Path).Hash -ne $Sha256) {
        Invoke-WebRequest -Uri $Uri -OutFile $Path -UseBasicParsing
    }
    if ((Get-FileHash -LiteralPath $Path).Hash -ne $Sha256) {
        throw "Unexpected SHA-256 for $Path"
    }
}

if (!$MSBuildPath) {
    $programFiles = [Environment]::GetFolderPath([Environment+SpecialFolder]::ProgramFilesX86)
    if (!$programFiles) {
        $programFiles = [Environment]::GetFolderPath([Environment+SpecialFolder]::ProgramFiles)
    }
    $vswhere = Join-Path $programFiles 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (Test-Path -LiteralPath $vswhere) {
        $MSBuildPath = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\Current\Bin\MSBuild.exe' |
            Select-Object -First 1
    }
    if (!$MSBuildPath) {
        $MSBuildPath = (Get-Command MSBuild.exe -ErrorAction SilentlyContinue).Source
    }
}
if (!$MSBuildPath -or !(Test-Path -LiteralPath $MSBuildPath -PathType Leaf)) {
    throw 'Visual Studio 2022 or Visual Studio 2022 Build Tools with MSBuild is required.'
}

$frameworkPackage = 'microsoft.netframework.referenceassemblies.net40'
$frameworkVersion = '1.0.3'
$frameworkZip = Join-Path $tools "$frameworkPackage.$frameworkVersion.zip"
$frameworkArchive = Join-Path $tools "net40-$frameworkVersion"
$frameworkRoot = Join-Path $frameworkArchive 'build'
$frameworkList = Join-Path $frameworkRoot '.NETFramework\v4.0\RedistList\FrameworkList.xml'
Get-VerifiedDownload `
    "https://api.nuget.org/v3-flatcontainer/$frameworkPackage/$frameworkVersion/$frameworkPackage.$frameworkVersion.nupkg" `
    $frameworkZip '54d6e20a1b61caf79395d6d71d091265e81f5a5705ac4ae52af45ca143c3c694'
if (!(Test-Path -LiteralPath $frameworkList -PathType Leaf)) {
    Expand-Archive -LiteralPath $frameworkZip -DestinationPath $frameworkArchive -Force
}

$project = Join-Path $root 'LaserGRBL\LaserGRBL.csproj'
& $MSBuildPath $project /nologo /m /v:minimal /t:Rebuild /p:Configuration=Release /p:DeviceProfileEdition=true `
    "/p:TargetFrameworkRootPath=$frameworkRoot/" "/p:OutputPath=$appOutput/" "/p:IntermediateOutputPath=$objOutput/"
if ($LASTEXITCODE -ne 0) {
    throw "MSBuild failed with exit code $LASTEXITCODE."
}

if ($SkipInstaller) {
    return
}

if (!$InnoCompiler) {
    $innoVersion = '6.7.1'
    $innoDir = Join-Path $tools "innosetup-$innoVersion"
    $InnoCompiler = Join-Path $innoDir 'ISCC.exe'
    if (!(Test-Path -LiteralPath $InnoCompiler -PathType Leaf)) {
        $innoSetup = Join-Path $tools "innosetup-$innoVersion.exe"
        Get-VerifiedDownload `
            "https://github.com/jrsoftware/issrc/releases/download/is-6_7_1/innosetup-$innoVersion.exe" `
            $innoSetup '4d11e8050b6185e0d49bd9e8cc661a7a59f44959a621d31d11033124c4e8a7b0'
        $arguments = "/PORTABLE=1 /CURRENTUSER /VERYSILENT /SUPPRESSMSGBOXES /NORESTART /DIR=`"$innoDir`""
        $process = Start-Process -FilePath $innoSetup -ArgumentList $arguments -Wait -PassThru
        if ($process.ExitCode -ne 0) {
            throw "Inno Setup initialization failed with exit code $($process.ExitCode)."
        }
    }
}
if (!(Test-Path -LiteralPath $InnoCompiler -PathType Leaf)) {
    throw "Inno Setup compiler not found at $InnoCompiler"
}

& $InnoCompiler "/DBuildOutput=$appOutput" "/O$installerOutput" (Join-Path $root 'setup-device-profiles.iss')
if ($LASTEXITCODE -ne 0) {
    throw "Inno Setup compilation failed with exit code $LASTEXITCODE."
}

$installer = Join-Path $installerOutput 'LaserGRBL-Device-Profiles-Setup.exe'
$sha256 = (Get-FileHash -LiteralPath $installer).Hash.ToLowerInvariant()
"$sha256  $([IO.Path]::GetFileName($installer))" | Set-Content -LiteralPath "$installer.sha256" -Encoding ASCII
Get-Item -LiteralPath $installer
