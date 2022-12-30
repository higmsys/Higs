@echo off
@setlocal ENABLEDELAYEDEXPANSION
cd /d %~DP0

rem Remove previous files.
rmdir /S /Q "%~DP0\TestResults"
rmdir /S /Q "%~DP0\coveragereport"

rem Run test and generate a coverage file.
dotnet test --collect:"XPlat Code Coverage"

rem Get the directory name of the coverage file.
if !errorlevel! equ 0 (
    dir /B /AD "%~DP0\TestResults" > %temp%\%~N0.txt
    set /p uuid=<%temp%\%~N0.txt
    del %temp%\%~N0.txt
)

rem Generate coverage report files and show on web browzer.
if !errorlevel! equ 0 (
    reportgenerator -reports:"%~DP0TestResults\%uuid%\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
    start shell:Appsfolder\Microsoft.MicrosoftEdge_8wekyb3d8bbwe^^!MicrosoftEdge "%~DP0\coveragereport\index.html"
)

