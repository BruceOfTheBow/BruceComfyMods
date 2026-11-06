namespace ComfyBatchDeposit;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(InventoryGui))]
public class InventoryGuiPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(InventoryGui.OnSelectedItem))]
  public static void OnSelectedItemPrefix(ref InventoryGui __instance, ref InventoryGrid grid, ref ItemDrop.ItemData item,  ref Vector2i pos, ref InventoryGrid.Modifier mod) {
    if (!IsModEnabled.Value || !Modifier.Value.IsPressed()) {
      return;
    }

    mod = InventoryGrid.Modifier.Move;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.Show))]
  public static void ShowPostfix(InventoryGui __instance, Container container) {
    if (!__instance.IsContainerOpen()) {
      return;
    }

    if (!ButtonManager.IsInitialized()) {
      ButtonManager.Initialize(__instance);
    }
  }
}
