namespace ComfyFirstPerson;

using HarmonyLib;

using UnityEngine;

using static PluginConfig;

[HarmonyPatch(typeof(Character))]
public static class CharacterPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Character.SetVisible))]
  static bool SetVisiblePrefix(ref Character __instance, bool visible) {
    if (!IsModEnabled.Value) {
      return true;
    }

    if (__instance.m_lodGroup == null) {
      return false;
    }
    if (__instance.m_lodVisible == visible) {
      return false;
    }
    if (__instance.IsPlayer() && !visible) {
      return false;
    }

    __instance.m_lodVisible = visible;

    if (__instance.m_lodVisible) {
      __instance.m_lodGroup.localReferencePoint = __instance.m_originalLocalRef;
      return false;
    }
    __instance.m_lodGroup.localReferencePoint = new Vector3(999999f, 999999f, 999999f);
    return false;
  }
}
