# Comfy Armor and Quick Slots

## Installation

### Manual

  * Un-zip `ComfyQuickSlots.dll` to your `/Valheim/BepInEx/plugins/` folder.

### Thunderstore (manual)

  * Go to Settings > Import local mod > Select `ComfyQuickSlots_v1.3.0.zip`.
  * Click "OK/Import local mod" on the pop-up for information.

## Features

![Imgur Image](https://imgur.com/P3FYbqc.jpeg)

### Armor Slots

  * Armor slots for items place in the bottom row of the new 8x5 inventory grid. 
  * Armor may only be placed here on equip and moved out on unequip
  * Armor will not unequip if there is insufficient inventory space (no empty spots in the original 8x4 inventory grid or 3 quick slots)

### Quick Slots

  * Quick slots are configurable via the F1 configuration manager but default to Z, V, and B for item use
  * GUI will display items in quickslots next to forsaken power. Configurable via configuration manager with F1

### On Death

  * Items in armor slots and quick slots will be placed on a second, smaller grave that spawns on top of the regular player tombstone while the regular inventory will be in the usual 8x4 player grave. This is to avoid issues with unmodded players opening graves and thus deleting items.

## Notes

  * **If using better UI set showDurabilityColor to disabled to prevent error log spam.**
  * If a character is logged into unmodded after a player save using this mod, any deltas in the inventory between the last modded save and a vanilla save will be overwritten by the last modded save.
  * This is a rework of Randy Knapp's Equipment and Quick Slots mod that looks to fix the issues around item deletion as well as issues around vanilla and modded player interaction. Tombstone handling modeled off Fang86's MoreSlots mod.
  * See source at: [GitHub](https://github.com/BruceOfTheBow/ComfyArmorAndQuickSlots).
  * Looking for a chill Valheim server? [Comfy Valheim Discord](https://discord.gg/ameHJz5PFk)
  * Check out our community driven listing site at: [valheimlist.org](https://valheimlist.org/)

## Changelog

### 1.3.0

  * Updates for patch 0.216.8

### 1.2.1

  * Minor performance fixes.

### 1.2.0

  * Added enable/disable toggle for quickslots. Enabled by default. No restarts required.

### 1.1.0

  * Fixed positioning of open containers for compatibility with patch 0.214.2

### 1.0.10

  * Added IsQuickSlot method for mod compatibility purposes. Fixed fish bait return bug.

### 1.0.9

  * Fixed bug on initial load with mod.

### 1.0.8

  * Fixed bug where upgrading equipped armor/shoulder/utility items resulted in deletion when inventory otherwise full.

### 1.0.7

  * Changed reference for blocking equipment use to IsEquipActionQueued for mistlands ptb.

### 1.0.6

  * Fixed issues with item use on equip.

### 1.0.5

  * Disables debugging log statements.

### 1.0.4

  * Reduced pop of second grave to prevent clipping through low ceilings.
  * Disabled selecting of items in armor slots to prevent unwanted inventory behavior and related bugs. Armor and utility items must be unequipped before removing from inventory.

### 1.0.3

  * Disables debugging log statements.

### 1.0.2

  * Fixed issue where logging out after first log-in with mod enabled could delete armor. First log-in check now reset on player save.

### 1.0.1

  * Disables debugging log statements.

### 1.0.0

  * Initial release.