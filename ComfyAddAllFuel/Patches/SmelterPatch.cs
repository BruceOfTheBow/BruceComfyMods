using System;
using AddAllFuel.Extensions;
using HarmonyLib;

using UnityEngine;

using static AddAllFuel.AddAllFuel;
using static AddAllFuel.PluginConfig;

namespace AddAllFuel.Patches {
  [HarmonyPatch(typeof(Smelter))]
  public static class SmelterPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Smelter.OnAddOre))]
    public static bool SmelterOnAddOrePrefix(ref Smelter __instance, ref Switch sw, ref Humanoid user, ItemDrop.ItemData item, ref bool __result) {
      if (!IsModEnabled.Value || !Input.GetKey(AddAllModifier.Value)) {
        return true;
      }

      __result = false;

      if (__instance.GetQueueSize() >= __instance.m_maxOre) {
        user.Message(MessageHud.MessageType.Center, "$msg_itsfull", 0, null);
        return false;
      }

      if (item == null) {
        item = FindCookableItem(__instance, user.GetInventory());
      }

      if (item == null) {
        user.Message(MessageHud.MessageType.Center, "$msg_noprocessableitems", 0, null);
        return false;
      }

      if (!__instance.IsItemAllowed(item)) {
        user.Message(MessageHud.MessageType.Center, "$msg_wontwork", 0, null);
        return false;
      }

      int queueSizeLeft = __instance.m_maxOre - __instance.GetQueueSize();
      int queueSize = Math.Min(item.m_stack, queueSizeLeft);

      user.GetInventory().RemoveItem(item, queueSize);

      for (int i = 0; i < queueSize; i++) {
        __instance.m_nview.InvokeRPC("RPC_AddOre", new object[] { item.m_dropPrefab.name });
      }

      user.Message(MessageHud.MessageType.Center, $"$msg_added {queueSize} {item.m_shared.m_name}", 0, null);
      __result = true;
      return false;
    }  

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Smelter.OnAddFuel))]
    public static bool Prefix(ref Smelter __instance, ref bool __result, Switch sw, Humanoid user, ItemDrop.ItemData item) {
      if (!IsModEnabled.Value || !Input.GetKey(AddAllModifier.Value)) {
        return true;
      }

      __result = false;

      if (__instance.GetFuel() > __instance.m_maxFuel - 1) {
        user.Message(MessageHud.MessageType.Center, "$msg_itsfull", 0, null);
        return false;
      }

      item = user.GetInventory().GetItem(__instance.GetFuelName(), -1, false);

      if (item == null) {
        user.Message(MessageHud.MessageType.Center, $"$msg_donthaveany {__instance.GetFuelName()}", 0, null);
        return false;
      }

      int diffFromFull = (int)(__instance.m_maxFuel - __instance.GetFuel());
      int amountToAdd = Math.Min(item.m_stack, diffFromFull);

      user.GetInventory().RemoveItem(item, amountToAdd);

      for (int i = 0; i < amountToAdd; i++) {
        __instance.m_nview.InvokeRPC("RPC_AddFuel", Array.Empty<object>());
      }

      user.Message(MessageHud.MessageType.Center, $"$msg_added {amountToAdd} {__instance.GetFuelName()}", 0, null);

      __result = true;
      return false;
    }
  }
 }
