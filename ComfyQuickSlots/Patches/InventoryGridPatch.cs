namespace ComfyQuickSlots;

using HarmonyLib;

using TMPro;

using UnityEngine;

using static PluginConfig;

[HarmonyPatch(typeof(InventoryGrid))]
static class InventoryGridPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGrid.UpdateInventory))]
  static void UpdateInventoryPostfix(InventoryGrid __instance) {
    if (__instance == InventoryGui.m_instance.m_playerGrid && QuickSlotsManager.ShouldRefreshPlayerGrid) {
      UpdatePlayerGrid(__instance);
    }
  }

  static void UpdatePlayerGrid(InventoryGrid inventoryGrid) {
    QuickSlotsManager.ShouldRefreshPlayerGrid = false;
    
    SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8], "Head");
    SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8 + 1], "Chest");
    SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8 + 2], "Legs");
    SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8 + 3], "Cape");
    SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8 + 4], "Util");

    if (EnableQuickslots.Value) {
      SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8 + 5], KeyCodeUtils.ToShortString(QuickSlot1.Value));
      SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8 + 6], KeyCodeUtils.ToShortString(QuickSlot2.Value));
      SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8 + 7], KeyCodeUtils.ToShortString(QuickSlot3.Value));
    } else {
      SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8 + 5], string.Empty, enabled: false);
      SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8 + 6], string.Empty, enabled: false);
      SetupBindingLabel(inventoryGrid.m_elements[(QuickSlotsManager.GetRows() - 1) * 8 + 7], string.Empty, enabled: false);
    }
  }

  static RectTransform GetOrCreateBackground(InventoryGrid inventoryGrid, string name) {
    Transform existingBkg = inventoryGrid.transform.parent.Find(name);

    if (!existingBkg) {
      Transform bkg = inventoryGrid.transform.parent.Find("Bkg");

      GameObject background = Object.Instantiate(bkg.gameObject, bkg.parent);
      background.name = name;

      existingBkg = background.transform;
      existingBkg.SetSiblingIndex(bkg.GetSiblingIndex() + 1);
    }

    return existingBkg as RectTransform;
  }

  static void SetupBindingLabel(InventoryElement element, string text, bool enabled = true) {
    Transform binding = element.gameObject.transform.Find("binding");

    if (binding && binding.TryGetComponent(out TMP_Text label)) {
      label.text = text;
      label.enabled = enabled;
      label.fontSize = 12f;
      label.alignment = TextAlignmentOptions.TopLeft;
      label.overflowMode = TextOverflowModes.Overflow;
      label.textWrappingMode = TextWrappingModes.NoWrap;
    }
  }
}
