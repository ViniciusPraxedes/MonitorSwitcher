# Monitor Switcher

Monitor Switcher is a robust utility that simplifies managing multiple display configurations on Windows. Designed with an elegant C# WinForms user interface and powered by PowerShell under the hood, this tool allows you to easily switch between predefined monitor layouts (e.g., PC Mode, Xbox Mode) with a single click. It natively disables inactive displays, adjusts resolutions, changes scaling, modifies rotations, and repositions screens based on your custom topology.

## Features

- **PC Mode**: Configures displays for a standard desktop topology. Can extend your display across an ultrawide and secondary screen with precise coordinate positioning.
- **Xbox Mode**: Adapts your setup for console gaming by cloning your primary monitor and applying correct scaling and rotation settings.
- **Dynamic Configuration**: A built-in Settings menu lets you change resolutions, monitor names, offsets, and rotation settings without having to recompile the application. All configurations are stored locally in a `settings.cfg` file.
- **Background Execution**: Executes the complex display switching logic asynchronously in PowerShell, keeping the UI responsive.
- **Seamless Integrations**: Employs the `DisplayConfig` PowerShell module to interact directly with the Windows display APIs.

## Prerequisites

- **OS**: Windows 10 or Windows 11
- **Framework**: .NET Framework (v4.8 or higher recommended for the C# compiler)
- **PowerShell**: Windows PowerShell 5.1+
- **Module**: `DisplayConfig` PowerShell module (must be installed or available in your PowerShell module path)

## Installation & Build Instructions

1. Clone or download this repository to your local machine.
2. Open a PowerShell terminal.
3. Navigate to the project directory.
4. Execute the build script to compile the application:
   ```powershell
   # Compile the C# WinForms UI
   .\build.ps1
   ```
5. The `MonitorSwitcher.exe` executable will be generated in the directory.

## Usage

1. Launch `MonitorSwitcher.exe`.
2. To configure your displays, click the **Settings** gear icon (⚙) in the top right corner. Update the parameters (such as `MagName`, `G27Name`, widths, heights, and rotations) to match your physical setup. Click **Save**.
3. Click on the **PC Mode** or **Xbox Mode** buttons to trigger the layout change.
4. The status bar at the bottom will indicate when the display transition is complete.

## How It Works

- `MonitorSwitcherUI.cs`: Contains the WinForms application. When a mode is selected, it reads `settings.cfg` and executes `Switch-MonitorMode.ps1` in a hidden process, passing your configuration settings as command-line arguments.
- `Switch-MonitorMode.ps1`: Receives the dynamic settings and leverages the `DisplayConfig` module to toggle, resize, rotate, and rearrange monitors.
- `build.ps1`: A minimalist build script that calls `csc.exe` (the C# compiler) to compile the WinForms UI without needing a full IDE like Visual Studio.
