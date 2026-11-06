namespace ComfyBatchDeposit;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(Humanoid))]
public static class HumanoidPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(Humanoid.DropItem))]
  public static void DropItemPostfix(Humanoid __instance, Inventory inventory, ItemDrop.ItemData item, int amount) {
    if (!IsModEnabled.Value
        || !Modifier.Value.IsPressed()
        ||  !__instance.IsPlayer()
        || Player.m_localPlayer == null
        || ItemManager.IsDropping) {
      return;
    }

    // Avoid infinite recursion by blocking next Humanoid.DropItem with this flag.
    ItemManager.IsDropping = true;

    foreach (ItemDrop.ItemData similarItem in ItemManager.GetAllItemsOfType(inventory, item)) {
      if (Player.m_localPlayer.IsItemEquiped(similarItem)) {
        continue;
      }

      __instance.DropItem(inventory, similarItem, item.m_stack);
    }

    ItemManager.IsDropping = false;
  }
}
