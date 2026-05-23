# Skills and Development Guide

This document outlines the architecture, design choices, and technical skills required to build and maintain the Monitor Switcher application.

## System Architecture

Monitor Switcher bridges two different ecosystems:
1. **Frontend**: A custom-painted C# WinForms GUI providing a responsive and modern user experience.
2. **Backend**: A PowerShell script (`Switch-MonitorMode.ps1`) executing deep system-level display modifications via the `DisplayConfig` module.

### Core Components

- **`MonitorSwitcherUI.cs`**: The entry point and frontend. It creates a borderless, flat-styled interface. It parses the local `settings.cfg` configuration file to determine monitor names, resolutions, offsets, and primary display choices.
- **`Switch-MonitorMode.ps1`**: The engine. It takes input arguments from the frontend and constructs a display pipeline (e.g., `Set-DisplayResolution`, `Set-DisplayRotation`, `Set-DisplayPosition`) to apply the requested topology.
- **`build.ps1`**: The compiler utility. It uses `csc.exe` (the native C# compiler bundled with the .NET framework) to build the standalone executable.

## Technical Skills Required

### C# WinForms Development
- **Custom UI Painting**: Understanding `Paint` event handlers and using `System.Drawing.Graphics` to create gradients, flat buttons, custom borders, and dynamic accent colors without relying on third-party UI libraries.
- **Asynchronous Execution**: Using `System.ComponentModel.BackgroundWorker` to run external processes without blocking the main UI thread.
- **File I/O**: Creating robust configuration parsers using `StreamWriter` and `StreamReader` to read and write key-value pairs dynamically.

### PowerShell Scripting
- **Parameter Validation**: Writing strict parameter blocks using `[ValidateSet()]` and mandatory flags to ensure data integrity when called from external applications.
- **Process Redirection**: Executing PowerShell scripts hidden (`-WindowStyle Hidden`) and managing standard output and error streams.
- **Pipeline Operations**: Constructing advanced pipelines using OS-level modules (like `DisplayConfig`) to locate devices via WMI or device paths and apply multiple configuration changes simultaneously.

### Bridging C# and PowerShell
- Using the `System.Diagnostics.Process` class to trigger PowerShell executables.
- Formatting complex strings to pass variables gracefully from a C# application to a PowerShell script as arguments.
- Handling race conditions and ensuring that UI buttons remain disabled until the background OS-level processes confirm execution completion.

## Design Patterns

- **Separation of Concerns**: The C# code exclusively handles the user experience, validation, and configuration saving. It contains zero logic regarding the Windows display API. The PowerShell script solely handles hardware interaction and contains zero user interface logic.
- **Dynamic Configuration**: Hardcoded variables are avoided. All display parameters are read from a plain-text configuration file (`settings.cfg`), ensuring that the application adapts to future hardware changes without requiring recompilation.
