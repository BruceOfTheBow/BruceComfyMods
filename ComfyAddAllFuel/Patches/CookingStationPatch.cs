using AddAllFuel.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;

using UnityEngine;

using static AddAllFuel.PluginConfig;

namespace AddAllFuel.Patches {
  [HarmonyPatch(typeof(CookingStation))]
  static class CookingStationPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(CookingStation.OnAddFuelSwitch))]
    public static bool OnAddFuelSwitchPrefix(CookingStation __instance, ref bool __result, Switch sw, Humanoid user, ItemDrop.ItemData item) {
      if (!IsModEnabled.Value || !Input.GetKey(AddAllModifier.Value)) {
        return true;
      }

      __result = false;

      item = user.GetInventory().GetItem(__instance.GetFuelName(), -1, false);

      if (__instance.GetFuel() > __instance.m_maxFuel - 1) {
        user.Message(MessageHud.MessageType.Center, "$msg_itsfull", 0, null);
        return false;
      }

      if (item == null) {
        user.Message(MessageHud.MessageType.Center, $"$msg_donthaveany {__instance.GetFuelName()}", 0, null);
        return false;
      }

      int diffFromFull = (int)(__instance.m_maxFuel - __instance.GetFuel());
      int amountToAdd = Math.Min(item.m_stack, diffFromFull);

      user.GetInventory().RemoveItem(item, amountToAdd);

      for (int i = 0; i < amountToAdd; i++) {
        __instance.m_nview.InvokeRPC("AddFuel", Array.Empty<object>());
      }

      user.Message(MessageHud.MessageType.Center, $"$msg_added {amountToAdd} {__instance.GetFuelName()}", 0, null);

      __result = true;
      return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(CookingStation.CookItem))]
    public static bool OnAddFoodSwitchPrefix(CookingStation __instance, ref bool __result, Humanoid user, ItemDrop.ItemData item) {
      if (!IsModEnabled.Value || !Input.GetKey(AddAllModifier.Value)) {
        return true;
      }

      __result = false;

      if (!__instance.m_nview.HasOwner()) {
        __instance.m_nview.ClaimOwnership();
      }

      foreach (CookingStation.ItemMessage itemMessage in __instance.m_incompatibleItems) {
        if (itemMessage.m_item.m_itemData.m_shared.m_name == item.m_shared.m_name) {
          user.Message(MessageHud.MessageType.Center, itemMessage.m_message + " " + itemMessage.m_item.m_itemData.m_shared.m_name, 0, null);
          return true;
        }
      }

      if (!__instance.IsItemAllowed(item)) {
        return false;
      }

      int freeSlots = __instance.GetFreeSlots();

      user.GetInventory().RemoveItem(item, freeSlots);

      for (int i = 0; i < freeSlots; i++) {
        __instance.m_nview.InvokeRPC("AddItem", new object[] { item.m_dropPrefab.name });
      }

      __result = true;
      return true;
    }
  }
}
