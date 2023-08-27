using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using UnityEngine;

using static ComfyQuickSlots.ComfyQuickSlots;
using static ComfyQuickSlots.InventoryExtensions;

namespace ComfyQuickSlots {

  [HarmonyPatch(typeof(Inventory))]
  public static class InventoryPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Inventory.AddItem))]
    public static bool AddItemGameObjectPrefix(Inventory __instance, ref bool __result, GameObject prefab, int amount) {
      if (__instance.IsPlayerInventory()) {
        //ComfyQuickSlots.log($"Adding {amount} game object {prefab.GetComponent<ItemDrop>().m_itemData.Clone().m_shared.m_name}");
        ItemDrop.ItemData item = prefab.GetComponent<ItemDrop>().m_itemData;
        if (HaveEmptyInventorySlot(__instance)) {
          Vector2i emptySlot = GetEmptyInventorySlot(__instance, __instance.TopFirst(item));
          __instance.AddItem(item, item.m_stack, emptySlot.x, emptySlot.y);
          return true;
        }
        __result = false;
        return false;
      }
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Inventory.AddItem))]
    public static bool AddItemPositionPrefix(Inventory __instance, ItemDrop.ItemData item, int amount, int x, int y) {
      if (__instance.IsPlayerInventory() && item != null) {
        Vector2i loc = new Vector2i(x, y);
        if (firstLoad && isArmor(item) && item.m_equipped) {
          if (!initialEquippedArmor.Contains((item))) {
            initialEquippedArmor.Add(item);
          }
          return false;
        }
        if (item.m_equipped && isArmor(item) && Player.m_localPlayer != null) {
          UnequipItem(Player.m_localPlayer, item);
          Vector2i armorSlot = GetArmorSlot(item);
          if (x == armorSlot.x && y == armorSlot.y) {
            return true;
          }
          return false;
        }
        if (Player.m_localPlayer == null) {
          return true;
        }
        if (x < 5 && y == 4) {
          return false;
        }
      }
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), "MoveItemToThis", typeof(Inventory), typeof(ItemDrop.ItemData), typeof(int), typeof(int), typeof(int))]
    public static bool MoveItemToThisPrefix(Inventory __instance, ref bool __result, Inventory fromInventory, ItemDrop.ItemData item, int amount, int x, int y) {
      if (__instance.IsPlayerInventory()) {
        if (x < 5 && y == 4) {
          return false;
        }
      }
      log($"Moving {item.m_shared.m_name} from {item.m_gridPos.x},{item.m_gridPos.y} in {fromInventory.m_name} to {x},{y} {__instance.m_name}");
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Inventory.FindEmptySlot))]
    public static bool FindEmptySlotPrefix(Inventory __instance, bool topFirst, ref Vector2i __result) {
      if (__instance.IsPlayerInventory()) {
        __result = GetEmptyInventorySlot(__instance, topFirst);
        return false;
      }
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Inventory), "HaveEmptySlot")]
    public static bool HaveEmptySlotPrefix(Inventory __instance, ref bool __result) {
      if (__instance.IsPlayerInventory()) {
        __result = HaveEmptyInventorySlot(__instance);
        return false;
      }
      return true;
    }
  }
}
