# ComfyBatchDeposit

  * Single-click batch action of inventory items of similar type. 
  * Sort and stack items within a chest with a single button press (S).
  * Dump like-type items into chest with a single button press (D).
    * Toggle to sort automatically after dumping player inventory contents.
  * Mass drops or adds to container/inventory using left alt + left click by default.
  *   Modifier key configurable

## Instructions

### Manual

  * Un-zip `BatchDeposit.dll` to your `/Valheim/BepInEx/plugins/` folder.

### Thunderstore (manual)

  * Go to Settings > Import local mod > Select `ComfyBatchDeposit_v1.2.0.zip`.
  * Click "OK/Import local mod" on the pop-up for information.

## Attributions

  * Button code and sort code taken in part from StorageUtils by turtton [https://github.com/turtton/StorageUtils]
  * License maintained as GPL

## Changelog

### 1.2.0

  * Added dump feature
    * Deposits all items from your inventory that are in the currently opened chest/container.
  * Added toggle to sort items whenever dumped into chest.

### 1.1.0

  * Added sorting function pulled from StorageUtils by turtton
  * Fixed sort function to not stack fish of differing qualities
  * Fixed sort function to properly store items with TopFirst

### 1.0.2
  * Updated icon

### 1.0.1
  * Changed deposit order to be reverse of ctrl + click order.
  * Added configurable keybind for modifier + Lclick for batch deposit.

### 1.0.0

  * Initial release.