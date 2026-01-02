# Build and Run LaserGRBL Storer Edition
& "C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe" LaserGRBL.sln
if ($LASTEXITCODE -eq 0) {
    Write-Host "Build successful! Starting application..." -ForegroundColor Green
    Start-Process .\LaserGRBL\bin\Debug\LaserGRBL.exe
} else {
    Write-Host "Build failed!" -ForegroundColor Red
}
