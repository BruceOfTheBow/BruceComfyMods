namespace PassWard;

using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using UnityEngine;

using static PluginConfig;

[HarmonyPatch(typeof(Player))]
static class PlayerPatch {
  [HarmonyTranspiler]
  [HarmonyPatch(nameof(Player.Update))]
  static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions) {
    return new CodeMatcher(instructions)
        .Start()
        .MatchStartForward(
            new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Player), nameof(Player.UpdateHover))))
        .ThrowIfInvalid($"Could not patch Player.Update()! (update-hover)")
        .Advance(offset: 1)
        .InsertAndAdvance(
            new CodeInstruction(OpCodes.Ldloc_1),
            new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PlayerPatch), nameof(UpdateHoverPostDelegate))))
        .InstructionEnumeration();
  }

  static void UpdateHoverPostDelegate(bool takeInput) {
    if (!takeInput || !IsModEnabled.Value || !Player.m_localPlayer) {
      return;
    }

    if (EnterPasswordKey.Value.IsDown()
        && Player.m_localPlayer.m_hovering
        && Player.m_localPlayer.m_hovering.TryGetComponentInParent(out PrivateArea privateArea)) {
      WardManager.EnterPassword(privateArea);
    }

    if (RemovePasswordKey.Value.IsDown()
        && Player.m_localPlayer.m_hovering
        && Player.m_localPlayer.m_hovering.TryGetComponentInParent(out privateArea)) {
      WardManager.RemovePassword(privateArea);
    }
  }

  public static T TryGetComponentInParent<T>(this GameObject gameObject, out T component) where T : Component {
    component = gameObject.GetComponentInParent<T>();
    return component;
  }
}
