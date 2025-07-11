namespace ComfyQuickSlots;

using HarmonyLib;

using UnityEngine;

using static PluginConfig;

[HarmonyPatch(typeof(InventoryGui))]
static class InventoryGuiPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.Show))]
  static void ShowPostfix(InventoryGui __instance) {
    if (__instance.m_currentContainer) {
      SetContainerGridAnchoredPosition(__instance);
    }

    QuickSlotsManager.ShouldRefreshPlayerGrid = true;
  }

  static void SetContainerGridAnchoredPosition(InventoryGui inventoryGui) {
    if (inventoryGui.m_containerGrid.transform.parent.TryGetComponent(out RectTransform rectTransform)) {
      rectTransform.anchoredPosition = ContainerInventoryGridAnchoredPosition.Value;
    }
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(InventoryGui.OnSelectedItem))]
  static bool OnSelectedItemPrefix(InventoryGrid grid, ItemDrop.ItemData item, Vector2i pos) {
    if (QuickSlotsManager.IsArmorSlot(pos)) {
      return false;
    }

    if (Player.m_localPlayer.IsEquipActionQueued(item)) {
      return false;
    }

    return true;
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(InventoryGui.OnCraftPressed))]
  static bool OnCraftPressedPrefix(InventoryGui __instance) {
    ItemDrop.ItemData item = __instance.m_selectedRecipe.ItemData;

    if (item != null
        && item.m_equipped
        && !QuickSlotsManager.HasEmptyNonEquipmentSlot(Player.m_localPlayer.GetInventory())
        && QuickSlotsManager.ItemCountInInventory(Player.m_localPlayer.GetInventory(), item) >= 1) {
      Player.m_localPlayer.Message(
          MessageHud.MessageType.Center, "Inventory full. Make room to upgrade equipped item.");

      return false;
    }

    return true;
  }
}
