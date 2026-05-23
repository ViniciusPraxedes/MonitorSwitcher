# Define the parameter block for the script
param (
    # The monitor configuration mode, must be either PC or Xbox
    [Parameter(Mandatory = $true)]
    # Restrict the mode parameter to the valid modes
    [ValidateSet("PC", "Xbox")]
    # The target monitor mode string variable
    [string]$Mode,
    # The search name for the MSI MAG345CQR display
    [string]$MagName = "MAG345CQR",
    # The search name for the MSI G27CQ4 display
    [string]$G27Name = "G27CQ4",
    # The search name for the LG LGD046F display
    [string]$LgName = "LGD046F",
    # The PC mode MAG width
    [int]$MagWidthPC = 3440,
    # The PC mode MAG height
    [int]$MagHeightPC = 1440,
    # The PC mode MAG rotation
    [string]$MagRotationPC = "None",
    # The PC mode MAG scale
    [int]$MagScalePC = 100,
    # The PC mode MAG X position
    [int]$MagXPC = -3440,
    # The PC mode MAG Y position
    [int]$MagYPC = 0,
    # The PC mode G27 width
    [int]$G27WidthPC = 1920,
    # The PC mode G27 height
    [int]$G27HeightPC = 1080,
    # The PC mode G27 rotation
    [string]$G27RotationPC = "Rotate270",
    # The PC mode G27 scale
    [int]$G27ScalePC = 100,
    # The PC mode G27 X position
    [int]$G27XPC = 0,
    # The PC mode G27 Y position
    [int]$G27YPC = 0,
    # The PC mode primary display
    [string]$PrimaryPC = "G27",
    # The Xbox mode MAG width
    [int]$MagWidthXbox = 2560,
    # The Xbox mode MAG height
    [int]$MagHeightXbox = 1440,
    # The Xbox mode MAG rotation
    [string]$MagRotationXbox = "Rotate270",
    # The Xbox mode MAG scale
    [int]$MagScaleXbox = 100,
    # The Xbox mode G27 rotation
    [string]$G27RotationXbox = "Rotate270",
    # The Xbox mode G27 scale
    [int]$G27ScaleXbox = 100,
    # The Xbox mode primary display
    [string]$PrimaryXbox = "MAG"
# End of parameter block
)

# Import the DisplayConfig module to manage monitors
Import-Module DisplayConfig -ErrorAction Stop

# Retrieve all display configurations currently known by the OS
$Displays = Get-DisplayInfo

# Locate the display matching the MAG monitor name
$MagDisplay = $Displays | Where-Object { $_.DisplayName -like "*$MagName*" -or $_.DevicePath -like "*$MagName*" }

# Assign the MAG display ID if found, otherwise null
$MagId = if ($MagDisplay) { $MagDisplay.DisplayId } else { $null }

# Locate the display matching the G27 monitor name
$G27Display = $Displays | Where-Object { $_.DisplayName -like "*$G27Name*" -or $_.DevicePath -like "*$G27Name*" }

# Assign the G27 display ID if found, otherwise null
$G27Id = if ($G27Display) { $G27Display.DisplayId } else { $null }

# Locate the display matching the LG monitor name
$LgDisplay = $Displays | Where-Object { $_.DisplayName -like "*$LgName*" -or $_.DevicePath -like "*$LgName*" }

# Assign the LG display ID if found, otherwise null
$LgId = if ($LgDisplay) { $LgDisplay.DisplayId } else { $null }

# Check if the requested mode is PC
if ($Mode -eq "PC")
# Begin the PC mode block
{
    # Check if the LG display is connected and active
    if ($LgId -ne $null)
    # Begin LG disable block
    {
        # Disconnect the LG display
        Disable-Display -DisplayId $LgId
    # End LG disable block
    }
    
    # Check if the MAG display is connected
    if ($MagId -ne $null)
    # Begin MAG enable block
    {
        # Enable the MAG display
        Enable-Display -DisplayId $MagId
        # Set the scale of MAG display
        Set-DisplayScale -DisplayId $MagId -Scale $MagScalePC
    # End MAG enable block
    }
    
    # Check if the G27 display is connected
    if ($G27Id -ne $null)
    # Begin G27 enable block
    {
        # Enable the G27 display
        Enable-Display -DisplayId $G27Id
        # Set the scale of G27 display
        Set-DisplayScale -DisplayId $G27Id -Scale $G27ScalePC
    # End G27 enable block
    }

    # Determine the primary display ID
    $PrimaryIdPC = if ($PrimaryPC -eq "G27") { $G27Id } else { $MagId }

    # Initialize the display configuration pipeline
    $Config = Get-DisplayConfig

    # Check if MAG display is present
    if ($MagId -ne $null)
    # Begin MAG config block
    {
        # Apply MAG resolution, rotation, and position
        $Config = $Config |
        # Set the resolution for MAG
        Set-DisplayResolution -DisplayId $MagId -Width $MagWidthPC -Height $MagHeightPC |
        # Set orientation for MAG
        Set-DisplayRotation -DisplayId $MagId -Rotation $MagRotationPC |
        # Place MAG at coordinates
        Set-DisplayPosition -DisplayId $MagId -XPosition $MagXPC -YPosition $MagYPC
    # End MAG config block
    }

    # Check if G27 display is present
    if ($G27Id -ne $null)
    # Begin G27 config block
    {
        # Apply G27 resolution, rotation, and position
        $Config = $Config |
        # Set the resolution for G27
        Set-DisplayResolution -DisplayId $G27Id -Width $G27WidthPC -Height $G27HeightPC |
        # Set orientation for G27
        Set-DisplayRotation -DisplayId $G27Id -Rotation $G27RotationPC |
        # Place G27 at coordinates
        Set-DisplayPosition -DisplayId $G27Id -XPosition $G27XPC -YPosition $G27YPC
    # End G27 config block
    }

    # Check if a primary display is set
    if ($PrimaryIdPC -ne $null)
    # Begin primary display config block
    {
        # Set the primary display
        $Config = $Config | Set-DisplayPrimary -DisplayId $PrimaryIdPC
    # End primary display config block
    }
    
    # Apply all queued changes in the display configuration pipeline
    $Config | Use-DisplayConfig
# End the PC mode block
}
# Check if the requested mode is Xbox
elseif ($Mode -eq "Xbox")
# Begin the Xbox mode block
{
    # Check if the LG display is connected and active
    if ($LgId -ne $null)
    # Begin LG disable block
    {
        # Disconnect the LG display
        Disable-Display -DisplayId $LgId
    # End LG disable block
    }
    
    # Check if the MAG display is connected
    if ($MagId -ne $null)
    # Begin MAG enable block
    {
        # Enable the MAG display
        Enable-Display -DisplayId $MagId
        # Set the scale of MAG display
        Set-DisplayScale -DisplayId $MagId -Scale $MagScaleXbox
    # End MAG enable block
    }
    
    # Check if the G27 display is connected
    if ($G27Id -ne $null)
    # Begin G27 enable block
    {
        # Enable the G27 display
        Enable-Display -DisplayId $G27Id
        # Set the scale of G27 display
        Set-DisplayScale -DisplayId $G27Id -Scale $G27ScaleXbox
    # End G27 enable block
    }
    
    # Determine the primary display ID
    $PrimaryIdXbox = if ($PrimaryXbox -eq "G27") { $G27Id } else { $MagId }

    # Initialize the display configuration pipeline
    $Config = Get-DisplayConfig

    # Check if MAG display is present
    if ($MagId -ne $null)
    # Begin MAG config block
    {
        # Apply MAG resolution and rotation
        $Config = $Config |
        # Set the resolution for MAG
        Set-DisplayResolution -DisplayId $MagId -Width $MagWidthXbox -Height $MagHeightXbox |
        # Set orientation for MAG
        Set-DisplayRotation -DisplayId $MagId -Rotation $MagRotationXbox
    # End MAG config block
    }

    # Check if G27 display is present
    if ($G27Id -ne $null)
    # Begin G27 config block
    {
        # Apply G27 rotation
        $Config = $Config |
        # Set orientation for G27
        Set-DisplayRotation -DisplayId $G27Id -Rotation $G27RotationXbox
    # End G27 config block
    }

    # Check if a primary display is set
    if ($PrimaryIdXbox -ne $null)
    # Begin primary display config block
    {
        # Set the primary display
        $Config = $Config | Set-DisplayPrimary -DisplayId $PrimaryIdXbox
    # End primary display config block
    }

    # Duplicate MAG onto G27 if both exist
    if ($MagId -ne $null -and $G27Id -ne $null)
    # Begin duplicate display block
    {
        # Duplicate/clone MAG onto G27
        $Config = $Config | Copy-DisplaySource -DisplayId $MagId -DestinationDisplayId $G27Id
    # End duplicate display block
    }

    # Apply all queued changes in the display configuration pipeline
    $Config | Use-DisplayConfig
# End the Xbox mode block
}
