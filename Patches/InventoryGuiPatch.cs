
using HarmonyLib;

using UnityEngine;

using static DumpsterFire.PluginConfig;

namespace DumpsterFire.Patches {
  [HarmonyPatch(typeof(InventoryGui))]
  public class InventoryGuiPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.UpdateItemDrag))]
    public static void UpdateItemDragPostfix(InventoryGui __instance) {
      if (!IsModEnabled.Value || !DeleteKey.Value.IsDown()) {
        return;
      }
      if (__instance.m_dragAmount.Equals(__instance.m_dragItem.m_stack)) {
        Player.m_localPlayer.RemoveEquipAction(__instance.m_dragItem);
        Player.m_localPlayer.UnequipItem(__instance.m_dragItem);
        __instance.m_dragInventory.RemoveItem(__instance.m_dragItem);
      } else {
        __instance.m_dragInventory.RemoveItem(__instance.m_dragItem, __instance.m_dragAmount);
      }

      __instance.SetupDragItem(null, null, 1);
    }
  }
}
