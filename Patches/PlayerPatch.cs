using System.Collections.Generic;
using HarmonyLib;

using static ComfyQuickSlots.ComfyQuickSlots;

namespace ComfyQuickSlots {

  [HarmonyPatch(typeof(Player))]
  public static class PlayerPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.Awake))]
    public static void PlayerAwakePrefix(ref Player __instance) {
      __instance.m_inventory.m_name = "ComfyQuickSlotsInventory";
      __instance.m_inventory.m_height = rows;
      __instance.m_inventory.m_width = columns;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.Load))]
    public static void PlayerLoadPostFix(Player __instance) {
      if (__instance.m_knownTexts.ContainsKey(playerDataKey)) {
        ZPackage pkg = new ZPackage(__instance.m_knownTexts[playerDataKey]);
        __instance.GetInventory().Load(pkg);
        EquipArmorInArmorSlots(__instance);
        __instance.GetInventory().Changed();
      } else {
        firstLoad = true;
        ZLog.Log($"First load with {initialEquippedArmor.Count}");
        foreach (ItemDrop.ItemData armorPiece in initialEquippedArmor) {
          UnequipItem(__instance, armorPiece);
          __instance.GetInventory().AddItem(armorPiece);
          __instance.EquipItem(armorPiece);
          Vector2i armorSlot = GetArmorSlot(armorPiece);
          MoveArmorItemToSlot(__instance, armorPiece, armorSlot.x, armorSlot.y);
          __instance.GetInventory().Changed();
        }
        initialEquippedArmor = new List<ItemDrop.ItemData>();
      }

      foreach (ItemDrop.ItemData item in __instance.GetInventory().m_inventory) {
        if (item.IsEquipable() && !IsArmor(item) && item.m_equipped) {
          __instance.EquipItem(item);
        }
      }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.Save))]
    public static bool PlayerSavePrefix(Player __instance) {
      firstLoad = false;
      return Save(__instance);
    }

    // Prevents interaction with item stands and armor stands while item is equipping
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.UseHotbarItem))]
    public static bool UseHotBarItemPrefix(Player __instance, int index) {
      ItemDrop.ItemData itemAt = __instance.m_inventory.GetItemAt(index - 1, 0);
      if (__instance.IsEquipActionQueued(itemAt)) {
        return false;
      }
      return true;
    }
  }
}
