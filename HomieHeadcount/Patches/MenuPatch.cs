using HarmonyLib;

namespace HomieHeadcount.Patches {
  [HarmonyPatch(typeof(Menu))]
  static class MenuPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Menu.OnLogoutYes))]
    public static void OnLogoutYesPrefix(Menu __instance) {
      PanelManager.Destroy();
      HomieCounter.Clear();
    }
  }
}
