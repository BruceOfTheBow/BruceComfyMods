using HarmonyLib;

namespace Pintervention.Patches {
  [HarmonyPatch(typeof(Menu))]
  static class MenuPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Menu.OnLogoutYes))]
    public static void OnLogoutYesPrefix() {
      ForeignPinManager.Clear();
    }
  }
}
