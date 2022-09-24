using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using System.IO;
using UnityEngine;
using UnityEngine.UI;

using static BatchDeposit.PluginConfig;
using static BatchDeposit.ComfyBatchDeposit;

namespace BatchDeposit {
  [HarmonyPatch(typeof(InventoryGrid))]
  public class InventoryGridPatch {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(InventoryGrid), "OnLeftClick")]
    public static bool OnLeftClickPrefix(InventoryGrid __instance, UIInputHandler clickHandler) {
      if (IsModEnabled.Value && Input.GetKey(KeyCode.LeftAlt)) {
        GameObject gameObject = clickHandler.gameObject;
        if(gameObject == null) {
          log("gameobject is null");
          return true;
        }
        Vector2i buttonPos = __instance.GetButtonPos(gameObject);
        if(buttonPos == null) {
          log("buttonPos is null");
          return true;
        }
        ItemDrop.ItemData item = __instance.m_inventory.GetItemAt(buttonPos.x, buttonPos.y);
        if(item == null) {
          log($"item at {buttonPos.x},{buttonPos.y} is null");
          return true;
        }

        if(item != null) {
          string itemName = item.m_shared.m_name;
          for (int i = 0; i < __instance.m_width; i++) {
            for (int j = 0; j < __instance.m_height; j++) {
              
              ItemDrop.ItemData checkItem = __instance.m_inventory.GetItemAt(i, j);
              if(checkItem != null) {
                log($"Checking if {checkItem.m_shared.m_name} matches {itemName}");
                if (checkItem.m_shared.m_name.Equals(itemName)) {
                  __instance.m_onSelected(__instance, checkItem, new Vector2i(i, j), InventoryGrid.Modifier.Move);
                }
              } else {
                log($"Item at {i},{j} is null");
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
