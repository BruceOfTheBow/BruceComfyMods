# ComfyGizmo

*Comfy version of Gizmo.*

## Features

  * Configurable modifier hot-keys for:
    * X-axis rotation (default: hold `LeftShift`)
    * Z-axis rotation (default: hold `LeftAlt`)
    * Reset selected axis rotation (default: `V`)
    * Reset **ALL** axis rotations (disabled by default)
  * Can disable the Gizmo placement visual.
  * Can set the snap angles per 180 degrees from 2 - 256.
  * Original Euler-style rotation.
 
## Rotation Modes

### Local Frame Mode

  * Allows rotation around the piece's local Y axis rather than the world Y axis.
  * If roof mode was enabled, it is disabled when local frame mode is enabled.

### Old Rotation Mode

  * Uses rotation scheme from ComfyGizmo `v1.2.0` - `v1.4.0`.
  * Differs from default by behavior when rotating around multiple axes.

### Roof Mode

  * Allows rotations for the 8 corner-roof pieces with the x-z axes shifted 45 degrees about Y.
  * Rotations align with the beam of the roof to sync with inclined flat roof pieces.
  * Will be disabled on enabling Local Frame mode.
  * Is only active when one of the 8 corner-roof pieces are selected.
  * Can remain enabled unless a player wishes to rotate a corner roof piece along its tradtional x or z axes.

## Configuration

### Ignoring Rotation

Prefabs can be ignored for Gizmo-rotation during placement using the following config-options under `[Ignored]`.

  * `ignoreTerrainOpPrefab`
    * If enabled, rotation will be ignored for terrain-modifying prefabs.
  * `ignorePrefabNameList` 
    * Comma-separated toggle-list of values where Gizmo-rotation will be ignored for prefabs with matching names.
    * Each value is in the format of: `prefab_name=0` or `prefab_name=1` where `0|1` indicates toggled off/on.
    * Example: `Beech_Sapling=1,Birch_Sapling=1,VineGreen_sapling=1`
    * Implements a custom user-friendly UI accessible in `ConfigurationManager`.

## Installation

### Manual

  1. Unzip `ComfyGizmo.dll` to your `/valheim/BepInEx/plugins/` folder

### Thunderstore (manual)

  1. **Disable or uninstall** any installed `ComfyGizmo_v1.8.0` or earlier.
  2. Go to Settings > Import local mod > select `ComfyGizmo_v1.15.0.zip`.
  3. Click "OK/Import local mod" on the pop-up for information.
  
## Notes

  * See source at: [GitHub](https://github.com/BruceOfTheBow/BruceComfyMods/tree/main/ComfyGizmo).
  * Looking for a chill Valheim server? [Comfy Valheim Discord](https://discord.gg/ameHJz5PFk)
  * ComfyGizmo icon created by [@jenniely](https://twitter.com/jenniely) (jenniely.com)