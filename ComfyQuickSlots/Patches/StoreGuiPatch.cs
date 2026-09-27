namespace ComfyQuickSlots;

using HarmonyLib;

[HarmonyPatch(typeof(StoreGui))]
static class StoreGuiPatch {

  [HarmonyPrefix]
  [HarmonyPatch(nameof(StoreGui.BuySelectedItem))]
  static void BuySelectedItemPrefix() {
    QuickSlotsManager.IsPurchasingItem = true;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(StoreGui.BuySelectedItem))]
  static void BuySelectedItemPostfix(StoreGui __instance) {
    QuickSlotsManager.IsPurchasingItem = false;
  }
}