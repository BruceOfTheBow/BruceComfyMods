namespace ComfyQuickSlots;

using System.Collections.Generic;

using UnityEngine;

using static PluginConfig;

static class HotkeyBarManager {
  public const string QuickSlotsHotKeyBarName = "QuickSlotsHotkeyBar";

  public static HotkeyBar QuickSlotsHotkeyBar;
  public static GameObject QuickSlotsHotkeyBarGameObject;

  public static bool ShouldRefreshQuickSlots = false;

  public static string[] HotkeyTexts = [
    KeyCodeUtils.ToShortString(QuickSlot1.Value),
    KeyCodeUtils.ToShortString(QuickSlot2.Value),
    KeyCodeUtils.ToShortString(QuickSlot3.Value)];

  public static void ResetItems(HotkeyBar hkb, Player player) {
    hkb.m_items.Clear();

    foreach (Vector2i quickSlot in QuickSlotsManager.QuickSlots) {
      ItemDrop.ItemData item = player.GetInventory().GetItemAt(quickSlot.x, quickSlot.y);

      if (item == null) {
        continue;
      }

      hkb.m_items.Add(item);
    }
  }

  public static void CreateHotkeyBarGameObject(Hud hud) {
    QuickSlotsHotkeyBarGameObject =
    Object.Instantiate(QuickSlotsHotkeyBar.gameObject, hud.m_rootObject.transform, true);
    QuickSlotsHotkeyBarGameObject.name = QuickSlotsHotKeyBarName;
    QuickSlotsHotkeyBar.m_selected = -1;

    ConfigPositionedElement configPositionedElement = QuickSlotsHotkeyBarGameObject.AddComponent<ConfigPositionedElement>();
    configPositionedElement.PositionConfig = QuickSlotsPosition;
    configPositionedElement.AnchorConfig = QuickSlotsAnchor;
    configPositionedElement.EnsureCorrectPosition();

    RemoveElements(QuickSlotsHotkeyBar);
  }

  public static bool HaveItemsChanged(HotkeyBar hkb, Player player) {
    List<ItemDrop.ItemData> items = new List<ItemDrop.ItemData>() { 
      player.GetInventory().GetItemAt(QuickSlotsManager.QuickSlot1.x, QuickSlotsManager.QuickSlot1.y), 
      player.GetInventory().GetItemAt(QuickSlotsManager.QuickSlot2.x, QuickSlotsManager.QuickSlot2.y), 
      player.GetInventory().GetItemAt(QuickSlotsManager.QuickSlot3.x, QuickSlotsManager.QuickSlot3.y) };

    foreach (ItemDrop.ItemData item in items) {
      if (hkb.m_items.Contains(item)) {
        continue;
      }

      return true;
    }

    return false;
  }

  public static void RemoveElements(HotkeyBar hkb) {
    hkb.m_items.Clear();

    foreach (HotkeyBar.ElementData element in hkb.m_elements) {
      Object.Destroy(element.m_go);
    }

    hkb.m_elements.Clear();
  }

  public static void ResetHotkeyTexts() {
    HotkeyTexts = [
      KeyCodeUtils.ToShortString(QuickSlot1.Value),
      KeyCodeUtils.ToShortString(QuickSlot2.Value),
      KeyCodeUtils.ToShortString(QuickSlot3.Value)];
  }
}
