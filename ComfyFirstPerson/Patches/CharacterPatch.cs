namespace ComfyFirstPerson;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(Character))]
public static class CharacterPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Character.SetVisible))]
  static void SetVisiblePrefix(ref Character __instance, ref bool visible) {
    if (!IsModEnabled.Value || !__instance.IsPlayer() || !IsFirstPersonEnabled.Value) {
      return;
    }

    visible = false;
  }
}
