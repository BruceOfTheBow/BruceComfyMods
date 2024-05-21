# Comfy Add All Fuel

## Features

* You no longer need to press the button repeatedly to add fuel, food, ore, etc.
* When using the any smelters, cooking stations, or fireplaces, interact key (default e) will behavior as normal
* When pressing the modifier key, batch adding will be attempted up to the fuel/food/ore limit.

## Description

* The unit of batch injection is done per stack.
  * For example, if you have wood (10/50) and wood (20/50) in your inventory, 10 of them will be consumed from your inventory.
  * If you do it again, 20 of them will be consumed.
  * If the fuel limit is less than the stack, only that amount will be consumed.

## Settings

1. Enabled
   * true: MOD enabled
   * false: Mod disabled
2. ModifierKey
   * Modifier key used to control batch injection.
3. ExcludeNames
   * Item name to exclude from submission.
   * Example: $item_finewood,$item_roundlog
   * Refer to this document for item names.

## Supported Consumers

1. Smelter
2. Charcoal kiln
3. Blast furnace
4. Windmill
5. Spinning wheel
6. Campfire
7. Ovens
8. Cooking Stations
9. Various torches