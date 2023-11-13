# ComfyGizmo

*Comfy version of Gizmo.*

  * Configurable modifier hot-keys for:
    * X-axis rotation (default: hold `LeftShift`)
    * Z-axis rotation (default: hold `LeftAlt`)
    * Reset selected axis rotation (default: `V`)
    * Reset **ALL** axis rotations (disabled by default)
  * Can disable the Gizmo placement visual.
  * Can set the snap angles per 180 degrees from 2 - 256.
  * Original Euler-style rotation.
 
## Installation

### Manual

  1. Unzip `Gizmo.dll` to your `/valheim/BepInEx/plugins/` folder

### Thunderstore (manual)

  1. **Disable or uninstall** any other `Gizmo` mods.
  2. **Disable or uninstall** any manually installed `ComfyGizmo_v1.3.0` or earlier.
  3. Go to Settings > Import local mod > select `ComfyGizmo_v1.9.0.zip`.
  4. Click "OK/Import local mod" on the pop-up for information.
  
## Changelog

### 1.9.0

  * Icon, plugin GUID,  and dependencies update.

### 1.8.0

  * Fixed method signature change in patch 0.217.14.

### 1.7.4

  * Updated piece cache to name + description to disambiguate between vertical and horizontal item stands

### 1.7.3

  * Added additional null checks for SearsCatalog compatibility checks.

### 1.7.2

  * Added null check.

### 1.7.1

  * Added compatibility support for SearsCatalog.

### 1.7.0

  * Added feature to copy target piece for placing. Default keybind 'P'.

### 1.6.0

  * Moved patches to their own files
  * Added color configuration values for each gizmo (x, y, and z) updating on change.

### 1.5.1
  * Added toggle to allow for v1.2.0 rotation scheme versus v1.3.0 rotation scheme. Restart required on enabling/disabling
  * Fixed issue with gizmo visual axes not rotating correctly.

### 1.5.0
  * Added hotkeys for halving and doubling snap divisions. PageUp and PageDown by default.
  * Added toggle for resetting piece rotation on snap division change. Enabled by default.
  * Added feature to copy target piece's rotation.
  * Added alternate rotation method for using local frame coordinates.
  * Added configuration option to rotate using local frame coordinates.
  * Hot key added to toggle between default and local frame rotation modes. Default set to back quote.
  * Added toggle to reset piece orientation which changing between rotation frames. Enabled by default.
  
### 1.4.0

  * Create a new GameObject `ComfyGizmo` to maintain the current Quaternion rotation state.
  * Re-ordered the mapping/assignment of original Gizmo's prefab XYZ roots/transforms per request by Jere.
  * Increased the snap-angles maximum from 128 to 256.
  * Moved plugin configuration logic into its own class `PluginConfig`.
  * Renamed the author field in `manifest.json` to `ComfyMods`.

### 1.3.1

  * Try to add compatability with other mods that also transpile `UpdatePlacementGhost`.

### 1.3.0

  * Added configuration option for a 'reset all rotations' hot-key (default to un-set).
  * Cleaned-up the UpdatePlacementGhost transpiler.

### 1.2.0

  * Modified GizmoRoot instantiate to trigger on `Game.Start()`.

### 1.1.0

  * Turn SnapDivisions into a slider.
  * Moved `Resources.GetResources()` into the main plugin file.

### 1.0.0

  * Rewrite of Gizmo for Comfy-specific use.
  * Supprot re-binding of modifier keys and simplify code to one file.