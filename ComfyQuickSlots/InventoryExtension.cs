using System;
using System.Linq;

using static ComfyQuickSlots.ComfyQuickSlots;

namespace ComfyQuickSlots {
  public static class InventoryExtensions {
    public static bool IsPlayerInventory(this Inventory inventory) {
      if (inventory.m_name.Equals(PlayerInventoryName)) {
        return true;
      }
      return false;
    }
  }
}