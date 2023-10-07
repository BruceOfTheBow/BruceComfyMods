# AssemblyLine

  * Why make one when you can have an assembly line?

## Instructions

  * Comfy replacement for MultiCraft by mjdegue.
  * Adds UI elements for selecting a crafting amount to be made while only clicking craft once.
  * Adds toggle to config to allow first shift + click increment to go from 1 to 10 rather than 1 to 11 
  so that perfect stack sizes are made with fewer clicks.

### Manual

  * Un-zip `AssemblyLine.dll` to your `/Valheim/BepInEx/plugins/` folder.

### Thunderstore (manual)

  2. Go to Settings > Import local mod > Select `AssemblyLine_v1.1.0.zip`.
  3. Click "OK/Import local mod" on the pop-up for information.

## Attributions

  * Icon sourced from: [Assembly line icons created by Muhammad Waqas Khan - Flaticon](https://www.flaticon.com/free-icons/assembly-line)

## Changelog

### 1.1.0

  * Updated for patch 0.217.22. Updated to TMP Text fields
  * Fixed bug where setting max craft amount set count to 0. Count will always be a positive integer.
  * Fixed bug where increment and decrement buttons non-interactable after closing InventoryGUI on upgrade tab and reopening.
  * Fixed bug where crafting continued when InventoryGUI opened after closing without cancelling crafting.

### 1.0.0

  * Initial release.