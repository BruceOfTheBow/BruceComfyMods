using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using static ComfyBatchDeposit.PluginConfig;
using static ComfyBatchDeposit.ComfyBatchDeposit;

namespace BatchDeposit {
  [HarmonyPatch(typeof(InventoryGrid))]
  public class InventoryGridPatch {
    private static RectTransform _sortButton;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(InventoryGrid.OnLeftClick))]
    public static bool OnLeftClickPrefix(InventoryGrid __instance, UIInputHandler clickHandler) {
      if (IsModEnabled.Value && Input.GetKey(Modifier.Value)) {
        GameObject gameObject = clickHandler.gameObject;
        if(gameObject == null) {
          return true;
        }
        Vector2i buttonPos = __instance.GetButtonPos(gameObject);
        if(buttonPos == null) {
          return true;
        }
        ItemDrop.ItemData item = __instance.m_inventory.GetItemAt(buttonPos.x, buttonPos.y);
        if(item == null) {
          return true;
        }

        if(item != null) {
          for (int j = 0; j < __instance.m_height; j++) {
            for (int i = __instance.m_width - 1; i >= 0; i--) {
              ItemDrop.ItemData checkItem = __instance.m_inventory.GetItemAt(i, j);
              if(checkItem != null) {
                if (checkItem.m_shared.m_name.Equals(item.m_shared.m_name)) {
                  __instance.m_onSelected(__instance, checkItem, new Vector2i(i, j), InventoryGrid.Modifier.Move);
                }
              }
            }
          }
        }
        return false;
      }
      return true;
    }
  }
}
