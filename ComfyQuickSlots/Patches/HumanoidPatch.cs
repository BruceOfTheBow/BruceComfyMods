using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using static ComfyQuickSlots.ComfyQuickSlots;

namespace ComfyQuickSlots {
  [HarmonyPatch(typeof(Humanoid))]
  public class HumanoidPatch {
    public static bool wasEquipped { get; set; } = false;
    public static ItemDrop.ItemData unequippedItem;
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Humanoid.EquipItem))]
    public static bool EquipItemPrefix(Humanoid __instance, bool __result, ItemDrop.ItemData item) {
      if (IsArmor(item)) {
        Vector2i armorSlot = GetArmorSlot(item);
        if (IsArmorTypeEquipped(__instance, item)) {
          if (IsArmorTypeEquipped(__instance, item)) {
            ItemDrop.ItemData swapItem = GetArmorItemToSwap(__instance, item);
            UnequipItem(__instance, swapItem);
            log($"Armor item already equipped. Swapping {item.m_shared.m_name} for {swapItem.m_shared.m_name}");
            EquipItem(__instance, item);
            MoveArmorItemToSlot(__instance, item, armorSlot.x, armorSlot.y);
            __result = true;
            return false;
          }
        }
        log($"Equipping item {item.m_shared.m_name}");
        EquipItem(__instance, item);
        if (!(item.m_gridPos.x == armorSlot.x && item.m_gridPos.y == armorSlot.y)) {
          MoveArmorItemToSlot(__instance, item, armorSlot.x, armorSlot.y);
          log($"Moving armor to item slot {armorSlot.x},{armorSlot.y}");
        }
        __result = true;
        return false;
      }
      return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Humanoid.EquipItem))]
    public static void EquipItemPostfix(Humanoid __instance, ItemDrop.ItemData item) {
      if (__instance != null && item != null) {
        log($"Item equipped {item.m_equipped}");
        __instance.GetInventory().Changed();
      }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Humanoid.UnequipItem))]
    public static bool UnequipItemPrefix(Humanoid __instance, ItemDrop.ItemData item, bool triggerEquipEffects) {
      if (item != null) {
        if (IsArmor(item) && item.m_equipped) {
          if (!HaveEmptyInventorySlot(__instance.GetInventory())) {
            log("No empty slots found. Will not unequip item.");
            __instance.Message(MessageHud.MessageType.Center, "Inventory full. Item not unequipped.");
            return false;
          }
          log("Unequipping item.");
          UnequipItem(__instance, item);
          Vector2i emptyLoc = GetEmptyInventorySlot(__instance.GetInventory(), true);
          MoveArmorItemToSlot(__instance, item, emptyLoc.x, emptyLoc.y);
          return false;
        }
      }
      return true;
    }
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Humanoid.UnequipItem))]
    public static void UnequipItemPostfix(Humanoid __instance, ItemDrop.ItemData item, bool triggerEquipEffects) {
      if (__instance != null && item != null) {
        log("Unequipping item complete.");
      }
    }
  }
}
