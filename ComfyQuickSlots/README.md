# ComfyQuickSlots

## Features

![Imgur Image](https://imgur.com/P3FYbqc.jpeg)

### Armor Slots

  * Armor slots for items place in the bottom row of the new 8x5 inventory grid. 
  * Armor may only be placed here on equip and moved out on unequip.
  * Armor will not unequip if there is insufficient inventory space (no empty spots in the original 8x4 inventory grid
    or 3 quick slots).

### Quick Slots

  * Quick slots are configurable via the F1 configuration manager but default to Z, V, and B for item use.
  * GUI will display items in quickslots next to forsaken power. Configurable via configuration manager with F1.

### On Death

  * Items in armor slots and quick slots will be placed on a second, smaller grave that spawns on top of the regular
    player tombstone while the regular inventory will be in the usual 8x4 player grave.
  * This is to avoid issues with unmodded players opening graves and thus deleting items.

## Installation

### Before Installation

* If you are uninstalling this mod or re-installing it after a period of playing without it, **remove all items from
  your inventory prior to doing so.**

### Manual

  * Un-zip `ComfyQuickSlots.dll` to your `/Valheim/BepInEx/plugins/` folder.

### Thunderstore (manual)

  * Go to Settings > Import local mod > Select `ComfyQuickSlots_v1.8.0.zip`.
  * Click "OK/Import local mod" on the pop-up for information.

## Notes

  * **If using better UI set showDurabilityColor to disabled to prevent error log spam.**
  * If a character is logged into unmodded after a player save using this mod, any deltas in the inventory between the
    last modded save and a vanilla save will be overwritten by the last modded save.
  * This is a rework of Randy Knapp's Equipment and Quick Slots mod that looks to fix the issues around item deletion
    as well as issues around vanilla and modded player interaction.
  * Tombstone handling modeled off Fang86's MoreSlots mod.
  * See source at: [GitHub](https://github.com/BruceOfTheBow/ComfyArmorAndQuickSlots).
  * Looking for a chill Valheim server? [Comfy Valheim Discord](https://discord.gg/ameHJz5PFk)
