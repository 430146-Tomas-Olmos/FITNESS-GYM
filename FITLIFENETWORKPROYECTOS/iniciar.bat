@echo off
chcp 65001 >nul
title FitLife Network
cd /d "%~dp0FITLIFENETWORKPROYECTO"

where dotnet >nul 2>nul
if errorlevel 1 (
    echo.
    echo  No se encontro .NET instalado.
    echo  Instala el "SDK de .NET 10" desde https://dotnet.microsoft.com/download
    echo  y volve a ejecutar este archivo.
    echo.
    pause
    exit /b 1
)

echo.
echo  Iniciando FitLife Network...
echo  (la primera vez puede tardar un poco porque descarga paquetes)
echo  Para cerrar el programa: cerra esta ventana o presiona Ctrl+C.
echo.

start "" cmd /c "timeout /t 8 /nobreak >nul & start http://localhost:5275"
dotnet run --launch-profile http
pause
