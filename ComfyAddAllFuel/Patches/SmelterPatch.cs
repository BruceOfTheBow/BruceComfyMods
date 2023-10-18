using System;

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
      bool isAddOne = !Input.GetKey(ModifierKey.Value);
      if (!IsModEnabled.Value || isAddOne) {
        return true;
      }
      __result = false;

      int queueSizeNow = Traverse.Create(__instance).Method("GetQueueSize").GetValue<int>();
      if (queueSizeNow >= __instance.m_maxOre) {
        user.Message(MessageHud.MessageType.Center, "$msg_itsfull", 0, null);
        return false;
      }


      if (item == null) {
        item = FindCookableItem(__instance, user.GetInventory(), isAddOne);
      }

      if (item == null) {
        if (isAddOne) {
          return true;
        } else {
          user.Message(MessageHud.MessageType.Center, "$msg_noprocessableitems", 0, null);
          return false;
        }
      }

      if (!Traverse.Create(__instance).Method("IsItemAllowed", item.m_dropPrefab.name).GetValue<bool>()) {
        user.Message(MessageHud.MessageType.Center, "$msg_wontwork", 0, null);
        return false;
      }



      user.Message(MessageHud.MessageType.Center, "$msg_added " + item.m_shared.m_name, 0, null);

      int queueSizeLeft = __instance.m_maxOre - queueSizeNow;
      int queueSize = 1;
      if (!isAddOne)
        queueSize = Math.Min(item.m_stack, queueSizeLeft);


      //log($"{item.m_shared.m_name}({item.m_stack})");
      //log($"{queueSizeNow} / {__instance.m_maxOre}");
      //log($"{queueSize}");

      user.GetInventory().RemoveItem(item, queueSize);

      for (int i = 0; i < queueSize; i++)
        __instance.m_nview.InvokeRPC("AddOre", new object[] { item.m_dropPrefab.name });

      __result = true;
      return false;
    }  

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Smelter.OnAddFuel))]
    public static bool Prefix(ref Smelter __instance, ref bool __result, Switch sw, Humanoid user, ItemDrop.ItemData item) {
      bool isAddOne = !Input.GetKey(ModifierKey.Value);
      if (!IsModEnabled.Value || isAddOne) {
        return true;
      }
      __result = false;

      string fuelName = __instance.m_fuelItem.m_itemData.m_shared.m_name;

      if (item != null && item.m_shared.m_name != fuelName) {
        user.Message(MessageHud.MessageType.Center, "$msg_wrongitem", 0, null);
        return false;
      }

      float fuelNow = Traverse.Create(__instance).Method("GetFuel").GetValue<float>();
      if (fuelNow > __instance.m_maxFuel - 1) {
        user.Message(MessageHud.MessageType.Center, "$msg_itsfull", 0, null);
        return false;
      }



      item = user.GetInventory().GetItem(fuelName, -1, false);

      if (item == null) {
        if (isAddOne) {
          return true;
        } else {
          user.Message(MessageHud.MessageType.Center, $"$msg_donthaveany {fuelName}", 0, null);
          return false;
        }

      }

      user.Message(MessageHud.MessageType.Center, $"$msg_added {fuelName}", 0, null);


      int fuelLeft = (int)(__instance.m_maxFuel - fuelNow);
      int fuelSize = 1;
      if (!isAddOne)
        fuelSize = Math.Min(item.m_stack, fuelLeft);

      //log($"{item.m_shared.m_name}({item.m_stack})");
      //log($"{fuelNow} / {__instance.m_maxFuel}");
      //log($"{fuelSize}");


      user.GetInventory().RemoveItem(item, fuelSize);

      for (int i = 0; i < fuelSize; i++)
        __instance.m_nview.InvokeRPC("AddFuel", Array.Empty<object>());


      __result = true;
      return false;
    }

    public static void RPC_AddOre(Smelter instance, ZNetView m_nview, string name, int count) {
      if (!m_nview.IsOwner())
        return;
      if (!Traverse.Create(instance).Method("IsItemAllowed", name).GetValue<bool>())
        return;

      int start = Traverse.Create(instance).Method("GetQueueSize").GetValue<int>();
      for (int i = 0; i < count; i++) {
        m_nview.GetZDO().Set($"item{start + i}", name);
      }
      m_nview.GetZDO().Set("queued", start + count);

      instance.m_oreAddedEffects.Create(instance.transform.position, instance.transform.rotation, null, 1f);
    }

    public static void RPC_AddFuel(Smelter instance, ZNetView m_nview, float count) {
      if (!m_nview.IsOwner())
        return;

      float now = Traverse.Create(instance).Method("GetFuel").GetValue<float>();
      m_nview.GetZDO().Set("fuel", now + count);
      instance.m_fuelAddedEffects.Create(
          instance.transform.position, instance.transform.rotation, instance.transform, 1f);
    }
  }
 }
