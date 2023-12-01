using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

using UnityEngine;

using static AddAllFuel.AddAllFuel;
using static AddAllFuel.PluginConfig;

namespace AddAllFuel.Patches {
  [HarmonyPatch(typeof(Fireplace))]
  public static class FireplacePatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Fireplace.Interact))]
    public static bool FireplaceInteractPrefix(ref Fireplace __instance, Humanoid user, bool hold, bool alt, ref bool __result) {
      if (!IsModEnabled.Value || !Input.GetKey(AddAllModifier.Value)) {
        return true;
      }

      __result = false;

      if (hold) {
        return false;
      }

      if (!__instance.m_nview.HasOwner()) {
        __instance.m_nview.ClaimOwnership();
      }

      string fuelName = __instance.m_fuelItem.m_itemData.m_shared.m_name;
      float fuelNow = (float)Mathf.CeilToInt(__instance.m_nview.GetZDO().GetFloat("fuel", 0f));
      if (fuelNow > __instance.m_maxFuel - 1) {
        user.Message(MessageHud.MessageType.Center, Localization.instance.Localize("$msg_cantaddmore", new string[]
            {fuelName}), 0, null);
        return false;
      }

      ItemDrop.ItemData item = user.GetInventory()?.GetItem(fuelName, -1, false);
      if (item == null) {
        user.Message(MessageHud.MessageType.Center, $"$msg_outof {fuelName}", 0, null);
        return false;
      }

      int fuelLeft = (int)(__instance.m_maxFuel - fuelNow);
      int fuelSize = Math.Min(item.m_stack, fuelLeft);

      user.GetInventory().RemoveItem(item, fuelSize);

      for (int i = 0; i < fuelSize; i++) {
        __instance.m_nview.InvokeRPC("AddFuel", Array.Empty<object>());
      }
      user.Message(MessageHud.MessageType.Center, Localization.instance.Localize("$msg_fireadding", new string[]
        {fuelName}), 0, null);

      __result = true;
      return false;
    }
  }
}
