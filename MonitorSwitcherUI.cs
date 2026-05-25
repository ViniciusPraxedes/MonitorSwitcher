// Import required namespaces for Windows Forms, drawing, and process management
using System;
// Import namespace for Windows Forms controls and application
using System.Windows.Forms;
// Import namespace for GDI+ graphics and colors
using System.Drawing;
// Import namespace for running external processes
using System.Diagnostics;
// Import namespace for file and path operations
using System.IO;
// Import namespace for background async task execution
using System.Threading.Tasks;
// Import namespace for runtime interop services
using System.Runtime.InteropServices;

// Define the main namespace for the application
namespace MonitorSwitcherUI
// Begin namespace body
{
    // Define class to represent all monitor settings
    public class Settings
    // Begin Settings class body
    {
        // Monitor search identifier for MAG345CQR
        public string MagName = "MAG345CQR";
        // Monitor search identifier for G27CQ4
        public string G27Name = "G27CQ4";
        // Monitor search identifier for LG internal display
        public string LgName = "LGD046F";
        // PC Mode MAG width
        public int MagWidthPC = 3440;
        // PC Mode MAG height
        public int MagHeightPC = 1440;
        // PC Mode MAG rotation
        public string MagRotationPC = "None";
        // PC Mode MAG scale
        public int MagScalePC = 100;
        // PC Mode MAG X coordinate position
        public int MagXPC = -3440;
        // PC Mode MAG Y coordinate position
        public int MagYPC = 0;
        // PC Mode G27 width
        public int G27WidthPC = 1920;
        // PC Mode G27 height
        public int G27HeightPC = 1080;
        // PC Mode G27 rotation
        public string G27RotationPC = "Rotate270";
        // PC Mode G27 scale
        public int G27ScalePC = 100;
        // PC Mode G27 X coordinate position
        public int G27XPC = 0;
        // PC Mode G27 Y coordinate position
        public int G27YPC = 0;
        // PC Mode primary display choice
        public string PrimaryPC = "G27";
        // Xbox Mode MAG width
        public int MagWidthXbox = 2560;
        // Xbox Mode MAG height
        public int MagHeightXbox = 1440;
        // Xbox Mode MAG rotation
        public string MagRotationXbox = "Rotate270";
        // Xbox Mode MAG scale
        public int MagScaleXbox = 100;
        // Xbox Mode G27 rotation
        public string G27RotationXbox = "Rotate270";
        // Xbox Mode G27 scale
        public int G27ScaleXbox = 100;
        // Xbox Mode primary display choice
        public string PrimaryXbox = "MAG";
    // End Settings class body
    }

    // Define the main application form class
    public class MainForm : Form
    // Begin MainForm class body
    {
        // Declare the PC Mode button control
        private Button _pcButton;
        // Declare the Xbox Mode button control
        private Button _xboxButton;
        // Declare the settings dialog button
        private Button _settingsButton;
        // Declare the status label control for feedback messages
        private Label _statusLabel;
        // Declare the title label shown at the top of the window
        private Label _titleLabel;
        // Declare the subtitle label shown below the title
        private Label _subtitleLabel;
        // Declare the panel containing the two mode buttons
        private Panel _buttonPanel;
        // Declare the top header panel with title and subtitle
        private Panel _headerPanel;
        // Declare the bottom status bar panel
        private Panel _statusPanel;
        // Declare the left accent column panel for visual design
        private Panel _accentPanel;
        // Settings instance containing active configuration
        private Settings _settings;
        // Absolute file path to settings config file
        private string _settingsPath;

        // Define the dark background color used across the application
        private static readonly Color ColorBackground = Color.FromArgb(18, 18, 24);
        // Define the slightly lighter surface color for panels
        private static readonly Color ColorSurface = Color.FromArgb(26, 26, 36);
        // Define the card background color for button areas
        private static readonly Color ColorCard = Color.FromArgb(34, 34, 48);
        // Define the indigo-purple accent color for PC Mode
        private static readonly Color ColorPcAccent = Color.FromArgb(99, 102, 241);
        // Define the hover color for PC Mode button
        private static readonly Color ColorPcHover = Color.FromArgb(79, 82, 221);
        // Define the green accent color for Xbox Mode
        private static readonly Color ColorXboxAccent = Color.FromArgb(34, 197, 94);
        // Define the hover color for Xbox Mode button
        private static readonly Color ColorXboxHover = Color.FromArgb(22, 163, 74);
        // Define the primary text color
        private static readonly Color ColorText = Color.FromArgb(240, 240, 255);
        // Define the muted secondary text color
        private static readonly Color ColorMuted = Color.FromArgb(148, 148, 180);
        // Define the border color used on panels
        private static readonly Color ColorBorder = Color.FromArgb(50, 50, 70);

        // Constructor: initialize the form and build the UI
        public MainForm()
        // Begin MainForm constructor body
        {
            // Resolve path to settings.cfg next to application binary
            _settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.cfg");
            // Load application settings from file or initialize defaults
            _settings = LoadSettings(_settingsPath);
            // Initialize all UI controls and wire up events
            InitializeComponent();
        // End MainForm constructor body
        }

        // Method to parse settings file dynamically
        private static Settings LoadSettings(string path)
        // Begin LoadSettings method body
        {
            // Create a default settings instance
            Settings s = new Settings();
            // If the configuration file does not exist, return defaults
            if (!File.Exists(path))
            // Return default configuration
            {
                // Return default settings
                return s;
            }
            // Begin safe reading of the configuration file
            try
            // Begin try block
            {
                // Read all lines of the file
                string[] lines = File.ReadAllLines(path);
                // Iterate through each line of the configuration file
                foreach (string line in lines)
                // Begin line processing loop
                {
                    // Check if line is empty or comment
                    if (String.IsNullOrEmpty(line) || line.StartsWith("#"))
                    // Skip to next line
                    {
                        // Proceed to next iteration
                        continue;
                    }
                    // Split key and value by the equals character
                    string[] parts = line.Split(new char[] { '=' }, 2);
                    // Ensure the line was split into exactly two parts
                    if (parts.Length != 2)
                    // Skip malformed lines
                    {
                        // Proceed to next iteration
                        continue;
                    }
                    // Strip whitespace from the key
                    string key = parts[0].Trim();
                    // Strip whitespace from the value
                    string val = parts[1].Trim();
                    // Match key against known configurations
                    switch (key)
                    // Begin switch block
                    {
                        // Handle MAGName property
                        case "MagName": s.MagName = val; break;
                        // Handle G27Name property
                        case "G27Name": s.G27Name = val; break;
                        // Handle LgName property
                        case "LgName": s.LgName = val; break;
                        // Handle MagWidthPC property
                        case "MagWidthPC": s.MagWidthPC = int.Parse(val); break;
                        // Handle MagHeightPC property
                        case "MagHeightPC": s.MagHeightPC = int.Parse(val); break;
                        // Handle MagRotationPC property
                        case "MagRotationPC": s.MagRotationPC = val; break;
                        // Handle MagScalePC property
                        case "MagScalePC": s.MagScalePC = int.Parse(val); break;
                        // Handle MagXPC property
                        case "MagXPC": s.MagXPC = int.Parse(val); break;
                        // Handle MagYPC property
                        case "MagYPC": s.MagYPC = int.Parse(val); break;
                        // Handle G27WidthPC property
                        case "G27WidthPC": s.G27WidthPC = int.Parse(val); break;
                        // Handle G27HeightPC property
                        case "G27HeightPC": s.G27HeightPC = int.Parse(val); break;
                        // Handle G27RotationPC property
                        case "G27RotationPC": s.G27RotationPC = val; break;
                        // Handle G27ScalePC property
                        case "G27ScalePC": s.G27ScalePC = int.Parse(val); break;
                        // Handle G27XPC property
                        case "G27XPC": s.G27XPC = int.Parse(val); break;
                        // Handle G27YPC property
                        case "G27YPC": s.G27YPC = int.Parse(val); break;
                        // Handle PrimaryPC property
                        case "PrimaryPC": s.PrimaryPC = val; break;
                        // Handle MagWidthXbox property
                        case "MagWidthXbox": s.MagWidthXbox = int.Parse(val); break;
                        // Handle MagHeightXbox property
                        case "MagHeightXbox": s.MagHeightXbox = int.Parse(val); break;
                        // Handle MagRotationXbox property
                        case "MagRotationXbox": s.MagRotationXbox = val; break;
                        // Handle MagScaleXbox property
                        case "MagScaleXbox": s.MagScaleXbox = int.Parse(val); break;
                        // Handle G27RotationXbox property
                        case "G27RotationXbox": s.G27RotationXbox = val; break;
                        // Handle G27ScaleXbox property
                        case "G27ScaleXbox": s.G27ScaleXbox = int.Parse(val); break;
                        // Handle PrimaryXbox property
                        case "PrimaryXbox": s.PrimaryXbox = val; break;
                    // End switch block
                    }
                // End line processing loop
                }
            // End try block
            }
            // Handle parsing exceptions silently and fallback to default
            catch
            // Begin catch block
            {
                // Fallback action placeholder
            }
            // Return parsed settings
            return s;
        // End LoadSettings method body
        }

        // Method to save settings file dynamically
        private static void SaveSettings(string path, Settings s)
        // Begin SaveSettings method body
        {
            // Begin safe file write block
            try
            // Begin try block
            {
                // Open file stream for writing settings
                using (StreamWriter sw = new StreamWriter(path))
                // Begin stream writing block
                {
                    // Write MAG name setting
                    sw.WriteLine("MagName=" + s.MagName);
                    // Write G27 name setting
                    sw.WriteLine("G27Name=" + s.G27Name);
                    // Write LG name setting
                    sw.WriteLine("LgName=" + s.LgName);
                    // Write PC MAG width setting
                    sw.WriteLine("MagWidthPC=" + s.MagWidthPC);
                    // Write PC MAG height setting
                    sw.WriteLine("MagHeightPC=" + s.MagHeightPC);
                    // Write PC MAG rotation setting
                    sw.WriteLine("MagRotationPC=" + s.MagRotationPC);
                    // Write PC MAG scale setting
                    sw.WriteLine("MagScalePC=" + s.MagScalePC);
                    // Write PC MAG X coordinate position
                    sw.WriteLine("MagXPC=" + s.MagXPC);
                    // Write PC MAG Y coordinate position
                    sw.WriteLine("MagYPC=" + s.MagYPC);
                    // Write PC G27 width setting
                    sw.WriteLine("G27WidthPC=" + s.G27WidthPC);
                    // Write PC G27 height setting
                    sw.WriteLine("G27HeightPC=" + s.G27HeightPC);
                    // Write PC G27 rotation setting
                    sw.WriteLine("G27RotationPC=" + s.G27RotationPC);
                    // Write PC G27 scale setting
                    sw.WriteLine("G27ScalePC=" + s.G27ScalePC);
                    // Write PC G27 X coordinate position
                    sw.WriteLine("G27XPC=" + s.G27XPC);
                    // Write PC G27 Y coordinate position
                    sw.WriteLine("G27YPC=" + s.G27YPC);
                    // Write PC primary display choice
                    sw.WriteLine("PrimaryPC=" + s.PrimaryPC);
                    // Write Xbox MAG width setting
                    sw.WriteLine("MagWidthXbox=" + s.MagWidthXbox);
                    // Write Xbox MAG height setting
                    sw.WriteLine("MagHeightXbox=" + s.MagHeightXbox);
                    // Write Xbox MAG rotation setting
                    sw.WriteLine("MagRotationXbox=" + s.MagRotationXbox);
                    // Write Xbox MAG scale setting
                    sw.WriteLine("MagScaleXbox=" + s.MagScaleXbox);
                    // Write Xbox G27 rotation setting
                    sw.WriteLine("G27RotationXbox=" + s.G27RotationXbox);
                    // Write Xbox G27 scale setting
                    sw.WriteLine("G27ScaleXbox=" + s.G27ScaleXbox);
                    // Write Xbox primary display choice
                    sw.WriteLine("PrimaryXbox=" + s.PrimaryXbox);
                // End stream writing block
                }
            // End try block
            }
            // Catch exceptions silently
            catch
            // Begin catch block
            {
                // Fallback action placeholder
            }
        // End SaveSettings method body
        }

        // Method to initialize and configure all UI components
        private void InitializeComponent()
        // Begin InitializeComponent method body
        {
            // Set the form title shown in the taskbar
            this.Text = "Monitor Switcher";
            // Set the initial window width
            this.Width = 440;
            // Set the initial window height
            this.Height = 480;
            // Prevent the user from resizing the window
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            // Disable the maximize button
            this.MaximizeBox = false;
            // Set the background color of the form
            this.BackColor = ColorBackground;
            // Center the window on the screen at startup
            this.StartPosition = FormStartPosition.CenterScreen;
            // Use Segoe UI font for the entire form
            this.Font = new Font("Segoe UI", 9f, FontStyle.Regular);

            // Create and configure the left accent bar panel
            _accentPanel = new Panel();
            // Set the accent panel width
            _accentPanel.Width = 4;
            // Dock the accent panel to the left side
            _accentPanel.Dock = DockStyle.Left;
            // Apply gradient-like gradient by using a gradient image in Paint
            _accentPanel.Paint += AccentPanel_Paint;

            // Create and configure the header panel
            _headerPanel = new Panel();
            // Set the header panel background color
            _headerPanel.BackColor = ColorSurface;
            // Set the header panel height
            _headerPanel.Height = 110;
            // Dock the header panel to the top
            _headerPanel.Dock = DockStyle.Top;
            // Add a bottom border to the header panel by handling Paint
            _headerPanel.Paint += HeaderPanel_Paint;

            // Create the main title label
            _titleLabel = new Label();
            // Set the title text
            _titleLabel.Text = "Monitor Switcher";
            // Set the title font size and weight
            _titleLabel.Font = new Font("Segoe UI", 20f, FontStyle.Bold);
            // Set the title text color
            _titleLabel.ForeColor = ColorText;
            // Remove the label background
            _titleLabel.BackColor = Color.Transparent;
            // Set label position within the header
            _titleLabel.Location = new Point(28, 22);
            // Auto-size the label to fit its content
            _titleLabel.AutoSize = true;

            // Create the subtitle label beneath the title
            _subtitleLabel = new Label();
            // Set the subtitle text
            _subtitleLabel.Text = "Select a display configuration";
            // Set the subtitle font
            _subtitleLabel.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            // Set the subtitle text color to muted
            _subtitleLabel.ForeColor = ColorMuted;
            // Remove the label background
            _subtitleLabel.BackColor = Color.Transparent;
            // Position the subtitle below the title
            _subtitleLabel.Location = new Point(30, 62);
            // Auto-size the subtitle
            _subtitleLabel.AutoSize = true;

            // Create the settings gear button
            _settingsButton = new Button();
            // Set settings button text using Unicode gear character
            _settingsButton.Text = "⚙";
            // Set settings button size
            _settingsButton.Size = new Size(36, 36);
            // Position settings button at top right of header
            _settingsButton.Location = new Point(360, 24);
            // Set button background to surface card color
            _settingsButton.BackColor = ColorCard;
            // Set button text color to muted style
            _settingsButton.ForeColor = ColorText;
            // Set settings button font size
            _settingsButton.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            // Enable flat appearance
            _settingsButton.FlatStyle = FlatStyle.Flat;
            // Set border color
            _settingsButton.FlatAppearance.BorderColor = ColorBorder;
            // Set border thickness
            _settingsButton.FlatAppearance.BorderSize = 1;
            // Change cursor to hand
            _settingsButton.Cursor = Cursors.Hand;
            // Wire up settings button click event handler
            _settingsButton.Click += SettingsButton_Click;

            // Add the title, subtitle, and settings button to the header panel
            _headerPanel.Controls.Add(_titleLabel);
            // Add subtitle to header
            _headerPanel.Controls.Add(_subtitleLabel);
            // Add settings button to header
            _headerPanel.Controls.Add(_settingsButton);

            // Create the button panel that holds PC and Xbox buttons
            _buttonPanel = new Panel();
            // Set the button panel background color
            _buttonPanel.BackColor = ColorBackground;
            // Dock the button panel to fill the remaining space
            _buttonPanel.Dock = DockStyle.Fill;

            // Create the PC Mode button
            _pcButton = new Button();
            // Set the PC button text to empty to allow custom painting
            _pcButton.Text = "";
            // Set the PC button width
            _pcButton.Width = 360;
            // Set the PC button height
            _pcButton.Height = 80;
            // Position the PC button horizontally centered and vertically spaced
            _pcButton.Location = new Point(32, 40);
            // Set the PC button background color
            _pcButton.BackColor = ColorCard;
            // Set the PC button text color
            _pcButton.ForeColor = ColorText;
            // Set the PC button font
            _pcButton.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            // Remove the button border
            _pcButton.FlatStyle = FlatStyle.Flat;
            // Set the flat border color to PC accent
            _pcButton.FlatAppearance.BorderColor = ColorPcAccent;
            // Set the flat border size
            _pcButton.FlatAppearance.BorderSize = 2;
            // Remove the mouse-over color change
            _pcButton.FlatAppearance.MouseOverBackColor = ColorPcHover;
            // Remove the mouse-down color change
            _pcButton.FlatAppearance.MouseDownBackColor = ColorPcAccent;
            // Use custom cursor for the button
            _pcButton.Cursor = Cursors.Hand;
            // Wire up the PC button click event
            _pcButton.Click += PcButton_Click;
            // Wire up hover paint for PC button
            _pcButton.Paint += PcButton_Paint;

            // Create the Xbox Mode button
            _xboxButton = new Button();
            // Set the Xbox button text to empty to allow custom painting
            _xboxButton.Text = "";
            // Set the Xbox button width
            _xboxButton.Width = 360;
            // Set the Xbox button height
            _xboxButton.Height = 80;
            // Position the Xbox button below the PC button
            _xboxButton.Location = new Point(32, 140);
            // Set the Xbox button background color
            _xboxButton.BackColor = ColorCard;
            // Set the Xbox button text color
            _xboxButton.ForeColor = ColorText;
            // Set the Xbox button font
            _xboxButton.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            // Remove the button border
            _xboxButton.FlatStyle = FlatStyle.Flat;
            // Set the flat border color to Xbox accent
            _xboxButton.FlatAppearance.BorderColor = ColorXboxAccent;
            // Set the flat border size
            _xboxButton.FlatAppearance.BorderSize = 2;
            // Remove the mouse-over color change
            _xboxButton.FlatAppearance.MouseOverBackColor = ColorXboxHover;
            // Remove the mouse-down color change
            _xboxButton.FlatAppearance.MouseDownBackColor = ColorXboxAccent;
            // Use custom cursor for the button
            _xboxButton.Cursor = Cursors.Hand;
            // Wire up the Xbox button click event
            _xboxButton.Click += XboxButton_Click;
            // Wire up hover paint for Xbox button
            _xboxButton.Paint += XboxButton_Paint;

            // Create the status bar panel at the bottom of the form
            _statusPanel = new Panel();
            // Set the status panel background color
            _statusPanel.BackColor = ColorSurface;
            // Set the status panel height
            _statusPanel.Height = 44;
            // Dock the status panel to the bottom
            _statusPanel.Dock = DockStyle.Bottom;
            // Add a top border to the status panel
            _statusPanel.Paint += StatusPanel_Paint;

            // Create the status label inside the status panel
            _statusLabel = new Label();
            // Set the default status message
            _statusLabel.Text = "Ready";
            // Set the status label font
            _statusLabel.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            // Set the status label text color
            _statusLabel.ForeColor = ColorMuted;
            // Remove the label background
            _statusLabel.BackColor = Color.Transparent;
            // Position the status label inside the panel
            _statusLabel.Location = new Point(28, 14);
            // Auto-size to fit content
            _statusLabel.AutoSize = true;

            // Add the status label to the status panel
            _statusPanel.Controls.Add(_statusLabel);

            // Add PC and Xbox buttons to the button panel
            _buttonPanel.Controls.Add(_pcButton);
            // Add Xbox button to panel
            _buttonPanel.Controls.Add(_xboxButton);

            // Add all panels to the form in the correct z-order
            this.Controls.Add(_buttonPanel);
            // Add status panel
            this.Controls.Add(_statusPanel);
            // Add header panel
            this.Controls.Add(_headerPanel);
            // Add accent bar panel
            this.Controls.Add(_accentPanel);
        // End InitializeComponent method body
        }

        // Method to draw a custom gradient-like accent bar on the left panel
        private void AccentPanel_Paint(object sender, PaintEventArgs e)
        // Begin AccentPanel_Paint method body
        {
            // Create a vertical gradient brush from PC accent to Xbox accent
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                _accentPanel.ClientRectangle,
                ColorPcAccent,
                ColorXboxAccent,
                System.Drawing.Drawing2D.LinearGradientMode.Vertical))
            // Paint layout body
            {
                // Fill the accent panel rectangle with the gradient brush
                e.Graphics.FillRectangle(brush, _accentPanel.ClientRectangle);
            }
        // End AccentPanel_Paint method body
        }

        // Method to draw the bottom border of the header panel
        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        // Begin HeaderPanel_Paint method body
        {
            // Create a pen with the border color
            using (var pen = new Pen(ColorBorder, 1))
            // Paint header border body
            {
                // Draw the bottom border line
                e.Graphics.DrawLine(pen, 0, _headerPanel.Height - 1, _headerPanel.Width, _headerPanel.Height - 1);
            }
        // End HeaderPanel_Paint method body
        }

        // Method to draw the top border of the status panel
        private void StatusPanel_Paint(object sender, PaintEventArgs e)
        // Begin StatusPanel_Paint method body
        {
            // Create a pen with the border color
            using (var pen = new Pen(ColorBorder, 1))
            // Paint status border body
            {
                // Draw the top border line
                e.Graphics.DrawLine(pen, 0, 0, _statusPanel.Width, 0);
            }
        // End StatusPanel_Paint method body
        }

        // Method to draw custom button content including title, subtitle, and accent indicator
        private void DrawButtonContent(Graphics g, Button btn, string title, string subtitle, Color accentColor)
        // Begin DrawButtonContent method body
        {
            // Enable anti-aliasing for smooth text rendering
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // Draw the colored left accent strip inside the button
            using (var brush = new SolidBrush(accentColor))
            // Paint button accent strip body
            {
                // Fill the left accent rectangle inside the button
                g.FillRectangle(brush, new Rectangle(0, 0, 5, btn.Height));
            }
            // Draw the main button title text
            using (var brush = new SolidBrush(ColorText))
            // Paint title body
            {
                // Measure the title string size
                var titleFont = new Font("Segoe UI", 13f, FontStyle.Bold);
                // Draw the title at the correct position
                g.DrawString(title, titleFont, brush, new PointF(20, 18));
                // Dispose the title font after use
                titleFont.Dispose();
            }
            // Draw the subtitle text below the title
            using (var brush = new SolidBrush(ColorMuted))
            // Paint subtitle body
            {
                // Define the subtitle font
                var subFont = new Font("Segoe UI", 8.5f, FontStyle.Regular);
                // Draw the subtitle at the correct position
                g.DrawString(subtitle, subFont, brush, new PointF(21, 45));
                // Dispose the subtitle font after use
                subFont.Dispose();
            }
        // End DrawButtonContent method body
        }

        // Event handler for settings button click
        private void SettingsButton_Click(object sender, EventArgs e)
        // Begin SettingsButton_Click method body
        {
            // Open settings form modal dialog
            using (SettingsForm sf = new SettingsForm(_settings))
            // Settings dialog block
            {
                // Show settings dialog and check if user clicked Save
                if (sf.ShowDialog(this) == DialogResult.OK)
                // Begin dialog ok processing block
                {
                    // Store updated settings locally
                    _settings = sf.CurrentSettings;
                    // Persist updated settings to configuration file
                    SaveSettings(_settingsPath, _settings);
                    // Update status label to indicate settings have been updated
                    SetStatus("Configuration updated.", ColorMuted);
                // End dialog ok processing block
                }
            }
        // End SettingsButton_Click method body
        }

        // Event handler for PC Mode button click
        private void PcButton_Click(object sender, EventArgs e)
        // Begin PcButton_Click method body
        {
            // Run the mode switch for PC
            RunMode("PC");
        // End PcButton_Click method body
        }

        // Event handler for Xbox Mode button click
        private void XboxButton_Click(object sender, EventArgs e)
        // Begin XboxButton_Click method body
        {
            // Run the mode switch for Xbox
            RunMode("Xbox");
        // End XboxButton_Click method body
        }

        // Event handler for PC button custom paint
        private void PcButton_Paint(object sender, PaintEventArgs e)
        // Begin PcButton_Paint method body
        {
            // Draw custom content for the PC button
            DrawButtonContent(e.Graphics, _pcButton, "PC Mode", "Landscape Extended | Custom Setup", ColorPcAccent);
        // End PcButton_Paint method body
        }

        // Event handler for Xbox button custom paint
        private void XboxButton_Paint(object sender, PaintEventArgs e)
        // Begin XboxButton_Paint method body
        {
            // Draw custom content for the Xbox button
            DrawButtonContent(e.Graphics, _xboxButton, "Xbox Mode", "Duplicate | Custom Setup", ColorXboxAccent);
        // End XboxButton_Paint method body
        }

        // Method to execute a monitor mode switch using a background worker
        private void RunMode(string mode)
        // Begin RunMode method body
        {
            // Disable both buttons to prevent double-click during execution
            _pcButton.Enabled = false;
            // Disable the Xbox button as well
            _xboxButton.Enabled = false;
            // Update the status label to show the switch is in progress
            SetStatus(String.Format("Switching to {0} Mode...", mode), ColorMuted);

            // Create a background worker to run the script without blocking the UI
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            // Store the mode in a local variable for the closure
            string currentMode = mode;
            // Wire up the background work handler
            worker.DoWork += delegate(object s, System.ComponentModel.DoWorkEventArgs args)
            // Background thread execution body
            {
                // Resolve the absolute path to the script file next to the executable
                string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Switch-MonitorMode.ps1");
                // Reference the current settings instance locally
                Settings cfg = _settings;
                // Build dynamic arguments containing all settings variables
                string psArgs = String.Format(
                    "-NoProfile -NonInteractive -ExecutionPolicy Bypass -File \"{0}\" -Mode {1} " +
                    "-MagName \"{2}\" -G27Name \"{3}\" -LgName \"{4}\" " +
                    "-MagWidthPC {5} -MagHeightPC {6} -MagRotationPC {7} -MagScalePC {8} -MagXPC {9} -MagYPC {10} " +
                    "-G27WidthPC {11} -G27HeightPC {12} -G27RotationPC {13} -G27ScalePC {14} -G27XPC {15} -G27YPC {16} " +
                    "-PrimaryPC \"{17}\" " +
                    "-MagWidthXbox {18} -MagHeightXbox {19} -MagRotationXbox {20} -MagScaleXbox {21} " +
                    "-G27RotationXbox {22} -G27ScaleXbox {23} -PrimaryXbox \"{24}\"",
                    scriptPath, currentMode,
                    cfg.MagName, cfg.G27Name, cfg.LgName,
                    cfg.MagWidthPC, cfg.MagHeightPC, cfg.MagRotationPC, cfg.MagScalePC, cfg.MagXPC, cfg.MagYPC,
                    cfg.G27WidthPC, cfg.G27HeightPC, cfg.G27RotationPC, cfg.G27ScalePC, cfg.G27XPC, cfg.G27YPC,
                    cfg.PrimaryPC,
                    cfg.MagWidthXbox, cfg.MagHeightXbox, cfg.MagRotationXbox, cfg.MagScaleXbox,
                    cfg.G27RotationXbox, cfg.G27ScaleXbox, cfg.PrimaryXbox
                );
                // Create a new process start info object
                ProcessStartInfo psi = new ProcessStartInfo("powershell.exe", psArgs);
                // Do not show the PowerShell console window
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                // Do not use the OS shell to start the process
                psi.UseShellExecute = false;
                // Hide the console window creation
                psi.CreateNoWindow = true;
                // Disable redirection of standard output to prevent process deadlocks
                psi.RedirectStandardOutput = false;
                // Disable redirection of standard error to prevent process deadlocks
                psi.RedirectStandardError = false;
                // Start the process
                Process proc = Process.Start(psi);
                // Wait for the script to complete execution
                proc.WaitForExit();
                // Dispose of the process
                proc.Dispose();
            };
            // Wire up the completion handler to re-enable buttons on the UI thread
            worker.RunWorkerCompleted += delegate(object s, System.ComponentModel.RunWorkerCompletedEventArgs args)
            // Main thread execution complete handler body
            {
                // Re-enable the PC button after execution
                _pcButton.Enabled = true;
                // Re-enable the Xbox button after execution
                _xboxButton.Enabled = true;
                // Determine the accent color based on the mode
                Color accentColor = currentMode == "PC" ? ColorPcAccent : ColorXboxAccent;
                // Update the status label to show the switch is complete
                SetStatus(String.Format("{0} Mode applied successfully.", currentMode), accentColor);
            };
            // Start the background worker
            worker.RunWorkerAsync();
        // End RunMode method body
        }

        // Thread-safe helper method to update the status label text and color
        private void SetStatus(string message, Color color)
        // Begin SetStatus method body
        {
            // Check if the call needs to be marshalled to the UI thread
            if (this.InvokeRequired)
            // Begin marshalling block
            {
                // Marshal the call to the UI thread
                this.Invoke(new Action<string, Color>(SetStatus), message, color);
                // Return from the background thread
                return;
            // End marshalling block
            }
            // Set the status label text
            _statusLabel.Text = message;
            // Set the status label color
            _statusLabel.ForeColor = color;
        // End SetStatus method body
        }

        // Application entry point
        [STAThread]
        public static void Main()
        // Begin Main method body
        {
            // Enable visual styles for modern controls
            Application.EnableVisualStyles();
            // Set the default text rendering mode
            Application.SetCompatibleTextRenderingDefault(false);
            // Run the main form as the application window
            Application.Run(new MainForm());
        // End Main method body
        }
    // End MainForm class body
    }

    // Define the SettingsForm class for configuring options
    public class SettingsForm : Form
    // Begin SettingsForm class body
    {
        // Store edited settings instance
        public Settings CurrentSettings { get; private set; }

        // Define form controls
        private TextBox _magNameTxt;
        // Textbox for G27 search name
        private TextBox _g27NameTxt;
        // Textbox for LG search name
        private TextBox _lgNameTxt;
        // Textbox for PC MAG width
        private TextBox _magWidthPcTxt;
        // Textbox for PC MAG height
        private TextBox _magHeightPcTxt;
        // Combobox for PC MAG rotation
        private ComboBox _magRotPcCmb;
        // Textbox for PC MAG scale
        private TextBox _magScalePcTxt;
        // Textbox for PC MAG X coordinate position
        private TextBox _magXPcTxt;
        // Textbox for PC MAG Y coordinate position
        private TextBox _magYPcTxt;
        // Textbox for PC G27 width
        private TextBox _g27WidthPcTxt;
        // Textbox for PC G27 height
        private TextBox _g27HeightPcTxt;
        // Combobox for PC G27 rotation
        private ComboBox _g27RotPcCmb;
        // Textbox for PC G27 scale
        private TextBox _g27ScalePcTxt;
        // Textbox for PC G27 X coordinate position
        private TextBox _g27XPcTxt;
        // Textbox for PC G27 Y coordinate position
        private TextBox _g27YPcTxt;
        // Combobox for PC mode primary display selection
        private ComboBox _primaryPcCmb;
        // Textbox for Xbox MAG width
        private TextBox _magWidthXboxTxt;
        // Textbox for Xbox MAG height
        private TextBox _magHeightXboxTxt;
        // Combobox for Xbox MAG rotation
        private ComboBox _magRotXboxCmb;
        // Textbox for Xbox MAG scale
        private TextBox _magScaleXboxTxt;
        // Combobox for Xbox G27 rotation
        private ComboBox _g27RotXboxCmb;
        // Textbox for Xbox G27 scale
        private TextBox _g27ScaleXboxTxt;
        // Combobox for Xbox mode primary display selection
        private ComboBox _primaryXboxCmb;
        // Save button control
        private Button _saveBtn;
        // Cancel button control
        private Button _cancelBtn;

        // Custom panels for layout group styling
        private Panel _generalPanel;
        // Panel for PC Mode configurations
        private Panel _pcPanel;
        // Panel for Xbox Mode configurations
        private Panel _xboxPanel;

        // Visual Colors (matching MainForm)
        private static readonly Color ColorBackground = Color.FromArgb(18, 18, 24);
        // Custom group panel surface color
        private static readonly Color ColorSurface = Color.FromArgb(26, 26, 36);
        // Custom control inner card color
        private static readonly Color ColorCard = Color.FromArgb(34, 34, 48);
        // Custom control border color
        private static readonly Color ColorBorder = Color.FromArgb(50, 50, 70);
        // Custom primary text color
        private static readonly Color ColorText = Color.FromArgb(240, 240, 255);
        // Custom secondary text color
        private static readonly Color ColorMuted = Color.FromArgb(148, 148, 180);
        // Custom Xbox green accent color
        private static readonly Color ColorXboxAccent = Color.FromArgb(34, 197, 94);

        // Constructor accepting current configuration settings
        public SettingsForm(Settings initialSettings)
        // Begin SettingsForm constructor body
        {
            // Clone settings to prevent mutating values on cancel
            CurrentSettings = new Settings
            {
                MagName = initialSettings.MagName,
                G27Name = initialSettings.G27Name,
                LgName = initialSettings.LgName,
                MagWidthPC = initialSettings.MagWidthPC,
                MagHeightPC = initialSettings.MagHeightPC,
                MagRotationPC = initialSettings.MagRotationPC,
                MagScalePC = initialSettings.MagScalePC,
                MagXPC = initialSettings.MagXPC,
                MagYPC = initialSettings.MagYPC,
                G27WidthPC = initialSettings.G27WidthPC,
                G27HeightPC = initialSettings.G27HeightPC,
                G27RotationPC = initialSettings.G27RotationPC,
                G27ScalePC = initialSettings.G27ScalePC,
                G27XPC = initialSettings.G27XPC,
                G27YPC = initialSettings.G27YPC,
                PrimaryPC = initialSettings.PrimaryPC,
                MagWidthXbox = initialSettings.MagWidthXbox,
                MagHeightXbox = initialSettings.MagHeightXbox,
                MagRotationXbox = initialSettings.MagRotationXbox,
                MagScaleXbox = initialSettings.MagScaleXbox,
                G27RotationXbox = initialSettings.G27RotationXbox,
                G27ScaleXbox = initialSettings.G27ScaleXbox,
                PrimaryXbox = initialSettings.PrimaryXbox
            };
            // Build the layout and controls
            InitializeComponent();
        // End SettingsForm constructor body
        }

        // Helper to create a styled label
        private Label CreateLabel(string text, Point loc, Control parent)
        // Begin CreateLabel method body
        {
            // Create a label instance
            Label lbl = new Label();
            // Set label text
            lbl.Text = text;
            // Set label location
            lbl.Location = loc;
            // Set font family and size
            lbl.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            // Set text color to muted
            lbl.ForeColor = ColorMuted;
            // Auto size label
            lbl.AutoSize = true;
            // Add label to parent control
            parent.Controls.Add(lbl);
            // Return label instance
            return lbl;
        // End CreateLabel method body
        }

        // Helper to create a styled TextBox
        private TextBox CreateTextBox(string text, Point loc, int width, Control parent)
        // Begin CreateTextBox method body
        {
            // Create a textbox instance
            TextBox txt = new TextBox();
            // Set textbox text
            txt.Text = text;
            // Set location
            txt.Location = loc;
            // Set width
            txt.Width = width;
            // Set dark background color
            txt.BackColor = ColorCard;
            // Set light text color
            txt.ForeColor = ColorText;
            // Set flat border style
            txt.BorderStyle = BorderStyle.FixedSingle;
            // Set font family and size
            txt.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            // Add textbox to parent control
            parent.Controls.Add(txt);
            // Return textbox instance
            return txt;
        // End CreateTextBox method body
        }

        // Helper to create a styled ComboBox
        private ComboBox CreateComboBox(string[] items, string selectItem, Point loc, int width, Control parent)
        // Begin CreateComboBox method body
        {
            // Create a combobox instance
            ComboBox cmb = new ComboBox();
            // Add items to combobox
            cmb.Items.AddRange(items);
            // Set selected item
            cmb.SelectedItem = selectItem;
            // Set location
            cmb.Location = loc;
            // Set width
            cmb.Width = width;
            // Set dark background color
            cmb.BackColor = ColorCard;
            // Set light text color
            cmb.ForeColor = ColorText;
            // Set flat style
            cmb.FlatStyle = FlatStyle.Flat;
            // Set dropdown style to restrict typing
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            // Set font family and size
            cmb.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            // Add combobox to parent control
            parent.Controls.Add(cmb);
            // Return combobox instance
            return cmb;
        // End CreateComboBox method body
        }

        // Setup all visual panels and form input controls
        private void InitializeComponent()
        // Begin InitializeComponent method body
        {
            // Set the form title shown in titlebar
            this.Text = "Configuration Settings";
            // Set settings window width
            this.Width = 660;
            // Set settings window height
            this.Height = 580;
            // Prevent dynamic resizing
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            // Disable maximize button
            this.MaximizeBox = false;
            // Disable minimize button
            this.MinimizeBox = false;
            // Set background color of configuration form
            this.BackColor = ColorBackground;
            // Center within parent window
            this.StartPosition = FormStartPosition.CenterParent;
            // Base font family settings
            this.Font = new Font("Segoe UI", 9f, FontStyle.Regular);

            // Create general setup panel
            _generalPanel = new Panel();
            // Set general panel location coordinates
            _generalPanel.Location = new Point(15, 15);
            // Set general panel size
            _generalPanel.Size = new Size(615, 80);
            // Set general panel background
            _generalPanel.BackColor = ColorSurface;
            // Handle painting border outline
            _generalPanel.Paint += GroupPanel_Paint;

            // Title label for general monitor identities
            Label lblGen = CreateLabel("Monitor Identity Settings (Wildcard Match)", new Point(10, 5), _generalPanel);
            // Make group header bold
            lblGen.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            // Make group header white
            lblGen.ForeColor = ColorText;

            // Setup MAG name label
            CreateLabel("MAG Name:", new Point(10, 35), _generalPanel);
            // Setup MAG name textbox
            _magNameTxt = CreateTextBox(CurrentSettings.MagName, new Point(90, 32), 100, _generalPanel);

            // Setup G27 name label
            CreateLabel("G27 Name:", new Point(210, 35), _generalPanel);
            // Setup G27 name textbox
            _g27NameTxt = CreateTextBox(CurrentSettings.G27Name, new Point(290, 32), 100, _generalPanel);

            // Setup LG name label
            CreateLabel("LG Name:", new Point(410, 35), _generalPanel);
            // Setup LG name textbox
            _lgNameTxt = CreateTextBox(CurrentSettings.LgName, new Point(490, 32), 100, _generalPanel);

            // Create PC Mode group panel
            _pcPanel = new Panel();
            // Position PC panel below general setup panel
            _pcPanel.Location = new Point(15, 110);
            // Set PC panel size
            _pcPanel.Size = new Size(300, 360);
            // Set PC panel background color
            _pcPanel.BackColor = ColorSurface;
            // Handle painting border outline
            _pcPanel.Paint += GroupPanel_Paint;

            // Title label for PC settings group
            Label lblPcTitle = CreateLabel("PC Mode Configuration", new Point(10, 5), _pcPanel);
            // Make header bold
            lblPcTitle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            // Make header white
            lblPcTitle.ForeColor = ColorText;

            // Setup MAG width label in PC mode
            CreateLabel("MAG Resolution:", new Point(10, 35), _pcPanel);
            // Setup MAG width text box in PC mode
            _magWidthPcTxt = CreateTextBox(CurrentSettings.MagWidthPC.ToString(), new Point(130, 32), 50, _pcPanel);
            // Setup divider "x" label
            CreateLabel("x", new Point(188, 35), _pcPanel);
            // Setup MAG height text box in PC mode
            _magHeightPcTxt = CreateTextBox(CurrentSettings.MagHeightPC.ToString(), new Point(205, 32), 50, _pcPanel);

            // Setup MAG rotation label in PC mode
            CreateLabel("MAG Rotation:", new Point(10, 65), _pcPanel);
            // Setup MAG rotation dropdown in PC mode
            _magRotPcCmb = CreateComboBox(new string[] { "None", "Rotate90", "Rotate180", "Rotate270" }, CurrentSettings.MagRotationPC, new Point(130, 62), 125, _pcPanel);

            // Setup MAG scale percent label in PC mode
            CreateLabel("MAG Scale %:", new Point(10, 95), _pcPanel);
            // Setup MAG scale textbox in PC mode
            _magScalePcTxt = CreateTextBox(CurrentSettings.MagScalePC.ToString(), new Point(130, 92), 50, _pcPanel);

            // Setup MAG screen X position label in PC mode
            CreateLabel("MAG Pos X / Y:", new Point(10, 125), _pcPanel);
            // Setup MAG position X textbox
            _magXPcTxt = CreateTextBox(CurrentSettings.MagXPC.ToString(), new Point(130, 122), 50, _pcPanel);
            // Setup MAG position Y textbox
            _magYPcTxt = CreateTextBox(CurrentSettings.MagYPC.ToString(), new Point(205, 122), 50, _pcPanel);

            // Setup G27 resolution label in PC mode
            CreateLabel("G27 Resolution:", new Point(10, 165), _pcPanel);
            // Setup G27 width textbox
            _g27WidthPcTxt = CreateTextBox(CurrentSettings.G27WidthPC.ToString(), new Point(130, 162), 50, _pcPanel);
            // Setup divider label
            CreateLabel("x", new Point(188, 165), _pcPanel);
            // Setup G27 height textbox
            _g27HeightPcTxt = CreateTextBox(CurrentSettings.G27HeightPC.ToString(), new Point(205, 162), 50, _pcPanel);

            // Setup G27 rotation label in PC mode
            CreateLabel("G27 Rotation:", new Point(10, 195), _pcPanel);
            // Setup G27 rotation combobox
            _g27RotPcCmb = CreateComboBox(new string[] { "None", "Rotate90", "Rotate180", "Rotate270" }, CurrentSettings.G27RotationPC, new Point(130, 192), 125, _pcPanel);

            // Setup G27 scale label in PC mode
            CreateLabel("G27 Scale %:", new Point(10, 225), _pcPanel);
            // Setup G27 scale textbox
            _g27ScalePcTxt = CreateTextBox(CurrentSettings.G27ScalePC.ToString(), new Point(130, 222), 50, _pcPanel);

            // Setup G27 position labels in PC mode
            CreateLabel("G27 Pos X / Y:", new Point(10, 255), _pcPanel);
            // Setup G27 X textbox
            _g27XPcTxt = CreateTextBox(CurrentSettings.G27XPC.ToString(), new Point(130, 252), 50, _pcPanel);
            // Setup G27 Y textbox
            _g27YPcTxt = CreateTextBox(CurrentSettings.G27YPC.ToString(), new Point(205, 252), 50, _pcPanel);

            // Setup primary display selector in PC mode
            CreateLabel("Primary Monitor:", new Point(10, 295), _pcPanel);
            // Setup primary display combobox selection
            _primaryPcCmb = CreateComboBox(new string[] { "G27", "MAG" }, CurrentSettings.PrimaryPC, new Point(130, 292), 125, _pcPanel);

            // Create Xbox Mode group panel
            _xboxPanel = new Panel();
            // Position panel to the right of PC panel
            _xboxPanel.Location = new Point(330, 110);
            // Set panel size
            _xboxPanel.Size = new Size(300, 360);
            // Set panel surface background color
            _xboxPanel.BackColor = ColorSurface;
            // Handle painting border outline
            _xboxPanel.Paint += GroupPanel_Paint;

            // Title label for Xbox configuration group
            Label lblXboxTitle = CreateLabel("Xbox Mode Configuration", new Point(10, 5), _xboxPanel);
            // Make header bold
            lblXboxTitle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            // Make header white
            lblXboxTitle.ForeColor = ColorText;

            // Setup MAG width and height labels in Xbox Mode
            CreateLabel("MAG Resolution:", new Point(10, 35), _xboxPanel);
            // Setup MAG width textbox in Xbox Mode
            _magWidthXboxTxt = CreateTextBox(CurrentSettings.MagWidthXbox.ToString(), new Point(130, 32), 50, _xboxPanel);
            // Setup spacer character label
            CreateLabel("x", new Point(188, 35), _xboxPanel);
            // Setup MAG height textbox in Xbox Mode
            _magHeightXboxTxt = CreateTextBox(CurrentSettings.MagHeightXbox.ToString(), new Point(205, 32), 50, _xboxPanel);

            // Setup MAG rotation label in Xbox Mode
            CreateLabel("MAG Rotation:", new Point(10, 65), _xboxPanel);
            // Setup MAG rotation combobox selection
            _magRotXboxCmb = CreateComboBox(new string[] { "None", "Rotate90", "Rotate180", "Rotate270" }, CurrentSettings.MagRotationXbox, new Point(130, 62), 125, _xboxPanel);

            // Setup MAG scaling percent in Xbox Mode
            CreateLabel("MAG Scale %:", new Point(10, 95), _xboxPanel);
            // Setup MAG scale textbox
            _magScaleXboxTxt = CreateTextBox(CurrentSettings.MagScaleXbox.ToString(), new Point(130, 92), 50, _xboxPanel);

            // Setup G27 rotation label in Xbox Mode
            CreateLabel("G27 Rotation:", new Point(10, 135), _xboxPanel);
            // Setup G27 rotation combobox selection
            _g27RotXboxCmb = CreateComboBox(new string[] { "None", "Rotate90", "Rotate180", "Rotate270" }, CurrentSettings.G27RotationXbox, new Point(130, 132), 125, _xboxPanel);

            // Setup G27 scale percent in Xbox Mode
            CreateLabel("G27 Scale %:", new Point(10, 165), _xboxPanel);
            // Setup G27 scale textbox
            _g27ScaleXboxTxt = CreateTextBox(CurrentSettings.G27ScaleXbox.ToString(), new Point(130, 162), 50, _xboxPanel);

            // Setup primary display selector in Xbox Mode
            CreateLabel("Primary Monitor:", new Point(10, 205), _xboxPanel);
            // Setup primary display combobox selection
            _primaryXboxCmb = CreateComboBox(new string[] { "MAG", "G27" }, CurrentSettings.PrimaryXbox, new Point(130, 202), 125, _xboxPanel);

            // Create Save Button
            _saveBtn = new Button();
            // Set save button text
            _saveBtn.Text = "Save";
            // Set size of save button
            _saveBtn.Size = new Size(110, 36);
            // Position save button at bottom right
            _saveBtn.Location = new Point(390, 490);
            // Set dark panel inner card background
            _saveBtn.BackColor = ColorCard;
            // Set white text color
            _saveBtn.ForeColor = ColorText;
            // Enable flat appearance
            _saveBtn.FlatStyle = FlatStyle.Flat;
            // Match border outline to Xbox green accent
            _saveBtn.FlatAppearance.BorderColor = ColorXboxAccent;
            // Hand cursor on mouseover
            _saveBtn.Cursor = Cursors.Hand;
            // Wire up Click event handler
            _saveBtn.Click += SaveBtn_Click;

            // Create Cancel Button
            _cancelBtn = new Button();
            // Set cancel button text
            _cancelBtn.Text = "Cancel";
            // Set size of cancel button
            _cancelBtn.Size = new Size(110, 36);
            // Position cancel button to the right of save button
            _cancelBtn.Location = new Point(510, 490);
            // Set dark card background
            _cancelBtn.BackColor = ColorCard;
            // Set white text color
            _cancelBtn.ForeColor = ColorText;
            // Enable flat styling
            _cancelBtn.FlatStyle = FlatStyle.Flat;
            // Set default border color
            _cancelBtn.FlatAppearance.BorderColor = ColorBorder;
            // Hand cursor on mouseover
            _cancelBtn.Cursor = Cursors.Hand;
            // Set cancel button DialogResult
            _cancelBtn.DialogResult = DialogResult.Cancel;

            // Add general setup panel to the settings form
            this.Controls.Add(_generalPanel);
            // Add PC mode panel to settings form
            this.Controls.Add(_pcPanel);
            // Add Xbox mode panel to settings form
            this.Controls.Add(_xboxPanel);
            // Add save button
            this.Controls.Add(_saveBtn);
            // Add cancel button
            this.Controls.Add(_cancelBtn);
        // End InitializeComponent method body
        }

        // Paint event handler to draw thin borders on group panels
        private void GroupPanel_Paint(object sender, PaintEventArgs e)
        // Begin GroupPanel_Paint method body
        {
            // Reference the panel control being painted
            Panel p = (Panel)sender;
            // Create Pen with border color
            using (var pen = new Pen(ColorBorder, 1))
            // Paint outline border body
            {
                // Draw rectangle border around panel client area
                e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            }
        // End GroupPanel_Paint method body
        }

        // Click handler for validating input and saving settings
        private void SaveBtn_Click(object sender, EventArgs e)
        // Begin SaveBtn_Click method body
        {
            // Start safe numeric conversions
            try
            // Begin try block
            {
                // Assign monitor search wildcard strings
                CurrentSettings.MagName = _magNameTxt.Text.Trim();
                // Store G27 identity search
                CurrentSettings.G27Name = _g27NameTxt.Text.Trim();
                // Store LG identity search
                CurrentSettings.LgName = _lgNameTxt.Text.Trim();

                // Convert and assign PC Mode properties
                CurrentSettings.MagWidthPC = int.Parse(_magWidthPcTxt.Text);
                // Convert MAG height in PC Mode
                CurrentSettings.MagHeightPC = int.Parse(_magHeightPcTxt.Text);
                // Assign MAG rotation choice in PC Mode
                CurrentSettings.MagRotationPC = _magRotPcCmb.SelectedItem.ToString();
                // Convert MAG scale percentage in PC Mode
                CurrentSettings.MagScalePC = int.Parse(_magScalePcTxt.Text);
                // Convert MAG X coordinate location
                CurrentSettings.MagXPC = int.Parse(_magXPcTxt.Text);
                // Convert MAG Y coordinate location
                CurrentSettings.MagYPC = int.Parse(_magYPcTxt.Text);

                // Convert and assign G27 PC Mode properties
                CurrentSettings.G27WidthPC = int.Parse(_g27WidthPcTxt.Text);
                // Convert G27 height in PC Mode
                CurrentSettings.G27HeightPC = int.Parse(_g27HeightPcTxt.Text);
                // Assign G27 rotation choice in PC Mode
                CurrentSettings.G27RotationPC = _g27RotPcCmb.SelectedItem.ToString();
                // Convert G27 scale percentage in PC Mode
                CurrentSettings.G27ScalePC = int.Parse(_g27ScalePcTxt.Text);
                // Convert G27 X coordinate location
                CurrentSettings.G27XPC = int.Parse(_g27XPcTxt.Text);
                // Convert G27 Y coordinate location
                CurrentSettings.G27YPC = int.Parse(_g27YPcTxt.Text);

                // Store PC primary display choice
                CurrentSettings.PrimaryPC = _primaryPcCmb.SelectedItem.ToString();

                // Convert and assign Xbox Mode properties
                CurrentSettings.MagWidthXbox = int.Parse(_magWidthXboxTxt.Text);
                // Convert MAG height in Xbox Mode
                CurrentSettings.MagHeightXbox = int.Parse(_magHeightXboxTxt.Text);
                // Assign MAG rotation choice in Xbox Mode
                CurrentSettings.MagRotationXbox = _magRotXboxCmb.SelectedItem.ToString();
                // Convert MAG scale percentage in Xbox Mode
                CurrentSettings.MagScaleXbox = int.Parse(_magScaleXboxTxt.Text);

                // Assign G27 rotation choice in Xbox Mode
                CurrentSettings.G27RotationXbox = _g27RotXboxCmb.SelectedItem.ToString();
                // Convert G27 scale percentage in Xbox Mode
                CurrentSettings.G27ScaleXbox = int.Parse(_g27ScaleXboxTxt.Text);

                // Store Xbox primary display choice
                CurrentSettings.PrimaryXbox = _primaryXboxCmb.SelectedItem.ToString();

                // Set DialogResult to OK to close form with success state
                this.DialogResult = DialogResult.OK;
            // End try block
            }
            // Catch conversion exceptions and alert user
            catch (Exception ex)
            // Begin catch block
            {
                // Show message box alert
                MessageBox.Show("Please enter valid numeric values for all resolutions, scales, and positions.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        // End SaveBtn_Click method body
        }
    // End SettingsForm class body
    }
// End namespace body
}
