using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using UnityEngine;

using static ComfyQuickSlots.ComfyQuickSlots;

namespace ComfyQuickSlots {

  [HarmonyPatch(typeof(Inventory))]
  public static class InventoryPatch {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), "AddItem", typeof(ItemDrop.ItemData), typeof(int), typeof(int), typeof(int))]
    public static bool AddItemPositionPrefix(Inventory __instance, ItemDrop.ItemData item, int amount, int x, int y) {
      if (item != null) {
        if (__instance.m_name == "ComfyQuickSlotsInventory") {
          Vector2i loc = new Vector2i(x, y);
          log($"Attempting to add item {item.m_shared.m_name} to {x},{y}. Is equipped? {item.m_equipped}. Local Player?{Player.m_localPlayer != null}. On Menu Load? {ComfyQuickSlots.onMenuLoad}");
          if (firstLoad && IsArmor(item) && item.m_equipped) {
            log($"Adding {item.m_shared.m_name} to initial armor list. Item not added to inventory");
            if (!initialEquippedArmor.Contains((item))) {
              initialEquippedArmor.Add(item);
            }
            return false;
          }
          if (item.m_equipped && IsArmor(item) && Player.m_localPlayer != null) {
            UnequipItem(Player.m_localPlayer, item);
            Vector2i armorSlot = GetArmorSlot(item);
            if (x == armorSlot.x && y == armorSlot.y) {
              log($"Adding equipped item {item.m_shared.m_name} to armor slot.");
              return true;
            }
            return false;
          }
          if (Player.m_localPlayer == null) {
            log("Adding item during menu loading phase.");
            return true;
          }
          if (x < 5 && y == 4) {
            return false;
          }

        }
      }
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), "MoveItemToThis", typeof(Inventory), typeof(ItemDrop.ItemData), typeof(int), typeof(int), typeof(int))]
    public static bool MoveItemToThisPrefix(Inventory __instance, ref bool __result, Inventory fromInventory, ItemDrop.ItemData item, int amount, int x, int y) {
      if (__instance.m_name.Equals("ComfyQuickSlotsInventory")) {
        if (x < 5 && y == 4) {
          return false;
        }
      }
      log($"Moving {item.m_shared.m_name} from {item.m_gridPos.x},{item.m_gridPos.y} in {fromInventory.m_name} to {x},{y} {__instance.m_name}");
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), "FindEmptySlot")]
    public static bool FindEmptySlotPrefix(Inventory __instance, bool topFirst, ref Vector2i __result) {
      if (__instance.m_name == "ComfyQuickSlotsInventory") {
        __result = GetEmptyInventorySlot(__instance, topFirst);
        log($"Found empty slot in ${__instance.m_name} at {__result.x},{__result.y}");
        return false;
      }
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), "HaveEmptySlot")]
    public static bool HaveEmptySlotPrefix(Inventory __instance, ref bool __result) {
      log("Checking have empty slots");
      if (__instance.m_name == "ComfyQuickSlotsInventory") {
        __result = HaveEmptyInventorySlot(__instance);
        return false;
      }
      return true;
    }
  }
}
