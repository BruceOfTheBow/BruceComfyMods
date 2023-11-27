using HarmonyLib;
using HomieHeadcount.Core;
using UnityEngine;

using static HomieHeadcount.PluginConfig;

namespace HomieHeadcount.Patches {
  [HarmonyPatch(typeof(Player))]
  static class PlayerPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.Update))]
    public static void UpdatePostifx() {
      if (PanelManager.IsHomiePanelActive()) {
        PanelManager.Update();
      }

      if (!IsModEnabled.Value || !Input.GetKeyDown(ToggleHomiePanel.Value.MainKey)) {
        return;
      }

      PanelManager.ToggleHomieCountPanel();
    }
  }
}
