using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using UnityEngine;

using static ComfyQuickSlots.ComfyQuickSlots;

namespace ComfyQuickSlots.Patches {
  [HarmonyPatch(typeof(InventoryGui))]
  public class InventoryGuiPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(InventoryGui.OnSelectedItem))]
    public static bool OnSelectedItemPrefix(InventoryGrid grid, ItemDrop.ItemData item, Vector2i pos, InventoryGrid.Modifier mod) {
      if (IsArmorSlot(pos)) {
        return false;
      }

      if (Player.m_localPlayer.IsEquipActionQueued(item)) {
        return false;
      }
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(InventoryGui.OnCraftPressed))]
    public static bool OnCraftPressedPrefix(ref InventoryGui __instance) {
      if (__instance.m_selectedRecipe.Value != null) {
        if (__instance.m_selectedRecipe.Value.m_equipped && !HaveEmptyInventorySlot(Player.m_localPlayer.GetInventory()) && ItemCountInInventory(__instance.m_selectedRecipe.Value) == 1) {
          Player.m_localPlayer.Message(MessageHud.MessageType.Center, "Inventory full. Make room to upgrade equipped item.");
          return false;
        }
      }
      return true;
    }
  }
}
