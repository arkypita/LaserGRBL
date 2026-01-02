# This is a convenience script for quick testing until we can migrate everything to dotnet CLI

# Run LaserGRBL Tests
# First build the main project using MSBuild (required for LaserGRBL.exe)
Write-Host "Building main project..." -ForegroundColor Cyan
& "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe" LaserGRBL.sln /p:Configuration=Debug

if ($LASTEXITCODE -ne 0) {
    Write-Host "Main project build failed." -ForegroundColor Red
    exit $LASTEXITCODE
}

# Restore and build test project using dotnet (test project is .NET Core)
Write-Host "Restoring test project packages..." -ForegroundColor Cyan
dotnet restore LaserGRBL.Tests\LaserGRBL.Tests.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Host "Package restore failed." -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "Building test project..." -ForegroundColor Cyan
dotnet build LaserGRBL.Tests\LaserGRBL.Tests.csproj --no-restore --configuration Debug /p:BuildProjectReferences=false

if ($LASTEXITCODE -ne 0) {
    Write-Host "Test project build failed." -ForegroundColor Red
    exit $LASTEXITCODE
}

# Run tests without rebuilding
Write-Host "Running tests..." -ForegroundColor Cyan
dotnet test LaserGRBL.Tests\LaserGRBL.Tests.csproj --no-build --configuration Debug
