namespace ComfyQuickSlots;

using UnityEngine;

using static PluginConfig;

static class HotkeyBarManager {
  public const string QuickSlotsHotKeyBarName = "QuickSlotsHotkeyBar";

  public static HotkeyBar QuickSlotsHotkeyBar;
  public static GameObject QuickSlotsHotkeyBarGameObject;

  public static void CreateHotkeyBarGameObject(Hud hud) {
    QuickSlotsHotkeyBarGameObject =
    Object.Instantiate(QuickSlotsHotkeyBar.gameObject, hud.m_rootObject.transform, true);
    QuickSlotsHotkeyBarGameObject.name = QuickSlotsHotKeyBarName;
    QuickSlotsHotkeyBar.m_selected = -1;

    ConfigPositionedElement configPositionedElement = QuickSlotsHotkeyBarGameObject.AddComponent<ConfigPositionedElement>();
    configPositionedElement.PositionConfig = QuickSlotsPosition;
    configPositionedElement.AnchorConfig = QuickSlotsAnchor;
    configPositionedElement.EnsureCorrectPosition();

    RemoveElements();
  }

  public static void RemoveElements() {
    QuickSlotsHotkeyBar.m_items.Clear();

    foreach (HotkeyBar.ElementData element in QuickSlotsHotkeyBar.m_elements) {
      Object.Destroy(element.m_go);
    }

    QuickSlotsHotkeyBar.m_elements.Clear();
  }
}
