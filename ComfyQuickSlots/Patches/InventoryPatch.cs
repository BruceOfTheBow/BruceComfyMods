namespace ComfyQuickSlots;

using HarmonyLib;

using UnityEngine;

[HarmonyPatch(typeof(Inventory))]
static class InventoryPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.AddItem), typeof(ItemDrop.ItemData), typeof(int), typeof(int), typeof(int))]
  static bool AddItemPrefix(Inventory __instance, ItemDrop.ItemData item, int amount, int x, int y) {
    if (item == null) {
      return true;
    }

    if (__instance.m_name == QuickSlotsManager.PlayerInventoryName) {
      if (QuickSlotsManager.FirstLoad && QuickSlotsManager.IsArmor(item) && item.m_equipped) {
        if (!QuickSlotsManager.InitialEquippedArmor.Contains((item))) {
          QuickSlotsManager.InitialEquippedArmor.Add(item);
        }
        return false;
      }

      if (item.m_equipped && QuickSlotsManager.IsArmor(item) && Player.m_localPlayer != null) {
        QuickSlotsManager.UnequipItem(Player.m_localPlayer, item);
        Vector2i armorSlot = QuickSlotsManager.GetArmorSlot(item);

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
  [HarmonyPatch(
      nameof(Inventory.AddItem),
      argumentTypes: [
        typeof(string),
        typeof(int),
        typeof(int),
        typeof(int),
        typeof(long),
        typeof(string),
        typeof(Vector2i),
        typeof(bool)
      ])]
  static void AddItemStringIntIntIntLongStringVector2iBoolPrefix(Inventory __instance,ref Vector2i position) {
    if (__instance.m_name == QuickSlotsManager.PlayerInventoryName) {
      if (position.x < 5 && position.y == 4) {
        position = new(-1, -1);
      }
    }
  }

  [HarmonyPrefix]
  [HarmonyPatch(
      nameof(Inventory.MoveItemToThis),
      typeof(Inventory), typeof(ItemDrop.ItemData), typeof(int), typeof(int), typeof(int))]
  static bool MoveItemToThisPrefix(Inventory __instance, ref bool __result, int x, int y) {
    if (__instance.m_name == QuickSlotsManager.PlayerInventoryName) {
      if (x < 5 && y == 4) {
        return false;
      }
    }

    return true;
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.FindEmptySlot))]
  static bool FindEmptySlotPrefix(Inventory __instance, ref Vector2i __result, bool topFirst) {
    if (__instance.m_name == QuickSlotsManager.PlayerInventoryName) {
      __result = QuickSlotsManager.GetEmptyInventorySlot(__instance, topFirst);
      return false;
    }

    return true;
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.HaveEmptySlot))]
  static bool HaveEmptySlotPrefix(Inventory __instance, ref bool __result) {
    if (__instance.m_name == QuickSlotsManager.PlayerInventoryName) {
      __result = QuickSlotsManager.HasEmptyNonEquipmentSlot(__instance);
      return false;
    }

    return true;
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.CanAddItem), typeof(ItemDrop.ItemData), typeof(int))]
  static bool CanAddItemPrefix(Inventory __instance, ItemDrop.ItemData item, int stack, ref bool __result) {
    if (__instance == Player.m_localPlayer.m_inventory) {
      __result = QuickSlotsManager.CanAddItem(__instance, item, stack);
      return false;
    }

    return true;
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.FindFreeStackSpace))]
  static bool FindFreeStackSpacePrefix(Inventory __instance, string name, float worldLevel, ref int __result) {
    if (__instance == Player.m_localPlayer.m_inventory) {
      __result = QuickSlotsManager.FindFreeNonEquipmentStackSpace(__instance, name, Mathf.RoundToInt(worldLevel));
      return false;
    }

    return true;
  }
}
