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

    int addedRows = QuickSlotsManager.Rows - 4;
    float offset = -35f * addedRows;

    RectTransform gridBkg = GetOrCreateBackground(inventoryGrid, "ExtInvGrid");
    gridBkg.anchoredPosition = new Vector2(0f, offset);
    gridBkg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 590f);
    gridBkg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300f + 75f * addedRows);

    //Add Quick slots and equipment overlays
    //for(int i = 36; i < rows*columns - 1; i++) {
    SetupBindingLabel(inventoryGrid.m_elements[32], "Head");
    SetupBindingLabel(inventoryGrid.m_elements[33], "Chest");
    SetupBindingLabel(inventoryGrid.m_elements[34], "Legs");
    SetupBindingLabel(inventoryGrid.m_elements[35], "Cape");
    SetupBindingLabel(inventoryGrid.m_elements[36], "Util");

    if (EnableQuickslots.Value) {
      SetupBindingLabel(inventoryGrid.m_elements[37], KeyCodeUtils.ToShortString(QuickSlot1.Value));
      SetupBindingLabel(inventoryGrid.m_elements[38], KeyCodeUtils.ToShortString(QuickSlot2.Value));
      SetupBindingLabel(inventoryGrid.m_elements[39], KeyCodeUtils.ToShortString(QuickSlot3.Value));
    } else {
      SetupBindingLabel(inventoryGrid.m_elements[37], string.Empty, enabled: false);
      SetupBindingLabel(inventoryGrid.m_elements[38], string.Empty, enabled: false);
      SetupBindingLabel(inventoryGrid.m_elements[39], string.Empty, enabled: false);
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

  static void SetupBindingLabel(InventoryGrid.Element element, string text, bool enabled = true) {
    Transform binding = element.m_go.transform.Find("binding");

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
