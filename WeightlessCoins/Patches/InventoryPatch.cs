namespace WeightlessCoins;

using HarmonyLib;

[HarmonyPatch(typeof(Inventory))]
static class InventoryPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.AddItem), argumentTypes: [
    typeof(ItemDrop.ItemData),
    typeof(int),
    typeof(int),
    typeof(int),
    typeof(bool)
  ])]
  static void AddItemItemDataIntIntIntPrefix(ItemDrop.ItemData item) {
    if (!WeightManager.IsCoins(item)) {
      return;
    }

    WeightManager.SetWeight(item);
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.AddItem), typeof(ItemDrop.ItemData))]
  static void AddItemItemDataPrefix(ItemDrop.ItemData item) {
    if (!WeightManager.IsCoins(item)) {
      return;
    }

    WeightManager.SetWeight(item);
  }
}