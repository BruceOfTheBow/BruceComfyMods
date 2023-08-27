using System;

using HarmonyLib;

using Common;

using UnityEngine;
using UnityEngine.UI;

using static ComfyQuickSlots.PluginConfig;
using static ComfyQuickSlots.HotkeyBarPatch;

namespace ComfyQuickSlots.Patches {
  [HarmonyPatch(typeof(Hud))]
  public static class Hud_Awake_Patch {
    private static HotkeyBar hotkeyBar;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Hud.Awake))]
    public static void AwakePostfix(Hud __instance) {
      hotkeyBar = __instance.GetComponentInChildren<HotkeyBar>();

      if (IsModEnabled.Value && !QuickSlotsHotkeyBar && EnableQuickslots.Value) {
        QuickSlotsHotkeyBar = UnityEngine.Object.Instantiate(hotkeyBar.gameObject, __instance.m_rootObject.transform, true);
        QuickSlotsHotkeyBar.name = "QuickSlotsHotkeyBar";
        QuickSlotsHotkeyBar.GetComponent<HotkeyBar>().m_selected = -1;

        var configPositionedElement = QuickSlotsHotkeyBar.AddComponent<ConfigPositionedElement>();
        configPositionedElement.PositionConfig = QuickSlotsPosition;
        configPositionedElement.AnchorConfig = QuickSlotsAnchor;
        configPositionedElement.EnsureCorrectPosition();
      }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Hud.Update))]
    public static void UpdatePostfix(Hud __instance) {
      if (IsModEnabled.Value && !QuickSlotsHotkeyBar && EnableQuickslots.Value) {
        QuickSlotsHotkeyBar = UnityEngine.Object.Instantiate(hotkeyBar.gameObject, __instance.m_rootObject.transform, true);
        QuickSlotsHotkeyBar.name = "QuickSlotsHotkeyBar";
        HotkeyBar hkb = QuickSlotsHotkeyBar.GetComponent<HotkeyBar>();
        hkb.m_selected = -1;

        var configPositionedElement = QuickSlotsHotkeyBar.AddComponent<ConfigPositionedElement>();
        configPositionedElement.PositionConfig = QuickSlotsPosition;
        configPositionedElement.AnchorConfig = QuickSlotsAnchor;
        configPositionedElement.EnsureCorrectPosition();

        hkb.m_items.Clear();
        foreach (HotkeyBar.ElementData element in hkb.m_elements) {
          UnityEngine.GameObject.Destroy(element.m_go);
        }
        hkb.m_elements.Clear();
      }
    }
  }
}
