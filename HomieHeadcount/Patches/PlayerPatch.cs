using HarmonyLib;

using static HomieHeadcount.PluginConfig;

namespace HomieHeadcount.Patches {
  [HarmonyPatch(typeof(Player))]
  static class PlayerPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.Update))]
    public static void UpdatePostifx() {
      if (!IsModEnabled.Value || !Hud.instance) {
        return;
      }

      if (PanelManager.IsHomiePanelActive() && HomieCounter.Count() == 0) {
        PanelManager.Toggle();
        return;
      }

      if (!PanelManager.IsHomiePanelActive() && HomieCounter.Count() > 0) {
        PanelManager.Toggle();
        return;
      }

      if (PanelManager.IsHomiePanelActive()) {
        PanelManager.Update();
      }
    }
  }
}
