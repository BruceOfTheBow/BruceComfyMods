using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;
using static ComfyBatchDeposit.ComfyBatchDeposit;
using static ComfyBatchDeposit.PluginConfig;

namespace BatchDeposit {
  [HarmonyPatch(typeof(InventoryGrid))]
  public class InventoryGridPatch {
    private static RectTransform _sortButton;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(InventoryGrid.OnLeftClick))]
    public static void OnLeftClickPrefix(InventoryGrid __instance, UIInputHandler clickHandler) {
      if (!IsModEnabled.Value || !Input.GetKey(Modifier.Value) || __instance.m_hasTouchSelected) {
        return;
      }

      if(clickHandler.gameObject == null) {
        return;
      }
      
      Vector2i buttonPos = __instance.GetButtonPos(clickHandler.gameObject);
     

      if (buttonPos == null) {
        return;
      }

      ItemDrop.ItemData item = __instance.m_inventory.GetItemAt(buttonPos.x, buttonPos.y);

      if(item == null) {
        return;
      }

      for (int j = 0; j < __instance.m_height; j++) {
        for (int i = __instance.m_width - 1; i >= 0; i--) {
          ItemDrop.ItemData checkItem = __instance.m_inventory.GetItemAt(i, j);

          if (checkItem == null || !checkItem.m_shared.m_name.Equals(item.m_shared.m_name)) {
            continue;
          }

          // TODO: Fix by using logic in Dump but use logic to dump or drop based on if chest is open.
          __instance.SetTouchSelection(buttonPos);
          __instance.m_onSelected(__instance, checkItem, new Vector2i(i, j), InventoryGrid.Modifier.Move);
        }
      }

      return;
    }
  }
}
