## Changelog

### 1.7.0

  * Fixed for the `v0.219.14` patch.
  * Bumped up `<LangVersion>` to C# 12.
  * Removed `MoreSlotsPatcherForComfyQuickslots` compatibility plugin as that mod is no longer supported.
  * Added new config-option `Container.InventoryGrid`-`anchoredPosition` to position the Container InventoryGrid panel.
  * QuickSlot bindings in the InventoryGrid will refresh only one time, each time the Player inventory is shown.
  * Major code clean-up and refactoring (WIP).

### 1.6.0

  * Updated for patch `v0.217.38`.
  * Fixed a bug where PlayerId and PlayerName were not added to the extra tombstone created on player death.
  * Added check for additional tombstone to not give corpse run multiple times, otherwise acts like regular tombstone.
  * Additional tombstone will now retrieve all items on interact as a normal tombstone.
  * Moved changelog to CHANGELOG.md

### 1.5.0

  * Updates for patch 0.217.22. Added refernce to TMP for ElementData.m_amount
  * Updated text and font details for TMPro elements.
  * Updated repo link in manifest to BruceComfyMods repo.

### 1.4.0

  * Updates for patch 0.217.14

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
  * Disabled selecting of items in armor slots to prevent unwanted inventory behavior and related bugs.
  * Armor and utility items must be unequipped before removing from inventory.

### 1.0.3

  * Disables debugging log statements.

### 1.0.2

  * Fixed issue where logging out after first log-in with mod enabled could delete armor.
  * First log-in check now reset on player save.

### 1.0.1

  * Disables debugging log statements.

### 1.0.0

  * Initial release.
