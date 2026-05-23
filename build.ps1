# Define the path to the .NET Framework C# compiler
$CscPath = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
# Define the source file to compile
$SourceFile = Join-Path $PSScriptRoot "MonitorSwitcherUI.cs"
# Define the output executable file name
$OutputFile = Join-Path $PSScriptRoot "MonitorSwitcher.exe"
# Compile the C# source into a Windows Forms executable
& $CscPath /target:winexe /out:"$OutputFile" /reference:System.dll /reference:System.Windows.Forms.dll /reference:System.Drawing.dll "$SourceFile"
# Check if the compilation was successful
if (Test-Path $OutputFile)
# Begin success message block
{
    # Print a success message to the console
    Write-Host "Build succeeded: $OutputFile" -ForegroundColor Green
# End success message block
}
# Handle failed compilation
else
# Begin failure message block
{
    # Print a failure message to the console
    Write-Host "Build failed." -ForegroundColor Red
# End failure message block
}
