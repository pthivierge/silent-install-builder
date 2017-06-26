@echo off
:: Definition of the color
Color 8F
:: definition of the size of the windows
mode con:cols=180 lines=2500
@echo on

REM Add some settings to be applied to the local server
REM ===================================================

REM Definition of the execution policy
powershell Set-ExecutionPolicy Unrestricted -Scope Process
powershell Get-ExecutionPolicy

REM unblocking script files - necessary when files where downloaded from internet.
powershell Unblock-File .\setup.ps1
powershell Unblock-File .\Modules\*.psm1 


REM Executing Setup
REM =======================
powershell .\setup.ps1

pause