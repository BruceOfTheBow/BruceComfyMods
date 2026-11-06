namespace ComfyBatchDeposit;

using System.Collections.Generic;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(Inventory))]
public static class InventoryPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(Inventory.MoveItemToThis), typeof(Inventory), typeof(ItemDrop.ItemData))]
  public static void MoveItemToThisPostfix(Inventory __instance, Inventory fromInventory, ItemDrop.ItemData item) {
    if (!IsModEnabled.Value || !Modifier.Value.IsPressed() || Player.m_localPlayer == null) {
      return;
    }

    foreach (ItemDrop.ItemData similarItem in ItemManager.GetAllItemsOfType(fromInventory, item)) {
      if (!__instance.HaveEmptySlot()) {
        break;
      }

      if (Player.m_localPlayer.IsItemEquiped(similarItem)) {
        continue;
      }

      if (!__instance.AddItem(similarItem)) {
        continue;
      }

      fromInventory.RemoveItem(similarItem);
    }

    __instance.Changed(false, false);
    fromInventory.Changed(false, false);
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.MoveAll))]
  public static bool MoveAllPrefix(Inventory __instance, Inventory fromInventory) {
    foreach (ItemDrop.ItemData moveItem in new List<ItemDrop.ItemData>(fromInventory.GetAllItems())) {
      if (!__instance.AddItem(moveItem)) {
        continue;
      }

      fromInventory.RemoveItem(moveItem);
    }

    __instance.Changed(false, false);
    fromInventory.Changed(false, false);
    return false;
  }
}
