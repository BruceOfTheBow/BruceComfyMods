namespace Pintervention;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(Minimap))]
static class MinimapPatch {
  static int _pinCount = 999999;

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Minimap.Update))]
  static void UpdatePostfix(Minimap __instance) {
    if (!IsModEnabled.Value 
          || !Minimap.IsOpen()
          || !__instance
          || !__instance.m_largeRoot
          || !DisplayFilterPanel.Value.IsDown()) {

      return;
    }

    PinOwnerManager.Initialize();
    PlayerFilterPanelManager.ToggleFilterPanel();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Minimap.UpdatePins))]
  static void UpdatePinsPostfix(Minimap __instance) {
    if (!IsModEnabled.Value
          || !Minimap.IsOpen()
          || !__instance
          || !__instance.m_largeRoot) {

      return;
    }

    PinOwnerManager.FilterPins();
    PlayerFilterPanelManager.UpdatePinCounts();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Minimap.UpdatePlayerPins))]
  static void UpdatePlayerPinsPostfix(Minimap __instance) {
    if (!IsModEnabled.Value
          || !__instance
          || !Player.m_localPlayer) {
    }

    if (_pinCount <= __instance.m_pins.Count) {
      return;
    }

    _pinCount = __instance.m_pins.Count;

    PinOwnerManager.AddAllLocalPlayerPins();
    PlayerFilterPanelManager.UpdatePanel();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Minimap.RemovePin), typeof(Minimap.PinData))]
  static void RemovePinPostfix(Minimap __instance, Minimap.PinData pin) {
    long pid = pin.m_ownerID;

    PinOwnerManager.RemoveLocalPlayerPin(pin);

    if (PinOwnerManager.GetPinsByPid(pid).Count == 0) {
      PinOwnerManager.RemoveForeignPinOwner(pid);
    }

    PlayerFilterPanelManager.UpdatePanel();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Minimap.ResetSharedMapData))]
  static void ResetSharedMapData(Minimap __instance) {
    PinOwnerManager.ForeignPinOwners.RemoveAll(x => PinOwnerManager.GetPinsByPid(x).Count == 0);
    PlayerFilterPanelManager.UpdatePanel();
  }
}
