## Changelog

### 1.14.0

  * Added new config option `[Ignored] ignorePrefabNameList`.
    * Comma-separated toggle-list of values where Gizmo-rotation will be ignored for prefabs with matching names.
    * Each value is in the format of: `prefab_name=0` or `prefab_name=1` where `0|1` indicate toggled off/on.
    * Example: `Beech_Sapling=1,Birch_Sapling=1,VineGreen_sapling=1`
    * Implements a custom user-friendly UI accessible in `ConfigurationManager`.

### 1.13.0

  * Updated for the `v0.219.16` patch.
  * Added new config option `[Ignored] ignoreTerrainOpPrefab`.
    * If enabled, rotation will be ignored for terrain-modifying prefabs.

### 1.12.0

  * Fixed for the `v0.219.14` patch.

### 1.11.0

  * Fixed a NRE with prefab shaders in certain cases where ComfyGizmo is not installed alongside Jotunn.

### 1.10.0

  * Bumped up `<LangVersion>` to C# 12.
  * Code clean-up and refactoring.
  * Created new `AssetUtils` and added logic to replace Gizmo prefab shaders at runtime.

### 1.9.2

  * Fixed bug where manually entering in snap division changes did not respect reset rotation configuration value.

### 1.9.1

  * Fixed bug where rotations occurred with build panel open.
  * Added CHANGELOG.md.

### 1.9.0

  * Added Roof Mode to rotation corner roof pieces in sync with other roof pieces.
    * Rotations occur with the x-z plane rotated +45 degrees.
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