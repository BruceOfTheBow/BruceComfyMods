namespace AddAllFuel;

using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using UnityEngine;

using static PluginConfig;

[HarmonyPatch(typeof(ShieldGenerator))]
static class ShieldGeneratorPatch {
  [HarmonyTranspiler]
  [HarmonyPatch(nameof(ShieldGenerator.Start))]
  static IEnumerable<CodeInstruction> StartTranspiler(
      IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
    return new CodeMatcher(instructions, generator)
        .Start()
        .MatchStartForward(
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(
                OpCodes.Ldftn, AccessTools.Method(typeof(ShieldGenerator), nameof(ShieldGenerator.OnAddFuel))),
            new CodeMatch(OpCodes.Newobj))
        .ThrowIfInvalid($"Could not patch ShieldGenerator.Awake()! (on-add-fuel)")
        .Advance(offset: 3)
        .InsertAndAdvance(
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(
                OpCodes.Call, AccessTools.Method(typeof(ShieldGeneratorPatch), nameof(OnAddFuelCallbackDelegate))))
        .InstructionEnumeration();
  }

  static Switch.Callback OnAddFuelCallbackDelegate(Switch.Callback onAddFuelCallback, ShieldGenerator shieldGenerator) {
    return IsModEnabled.Value ? new(shieldGenerator.OnRepeatAddFuel) : onAddFuelCallback;
  }

  public static bool OnRepeatAddFuel(
      this ShieldGenerator shieldGenerator, Switch sw, Humanoid user, ItemDrop.ItemData item) {
    if (!IsModEnabled.Value
        || !ZInput.GetKey(AddAllModifier.Value)
        || (item != default && !shieldGenerator.IsMatchingFuelItem(item.m_shared.m_name))) {
      return shieldGenerator.OnAddFuel(sw, user, item);
    }

    int repeatCount = 0;
    item ??= shieldGenerator.GetMatchingFuelItem(user.GetInventory());

    if (item != default) {
      int requiredFuel = Mathf.CeilToInt(shieldGenerator.m_maxFuel - shieldGenerator.GetFuel());
      repeatCount = Mathf.Max(Mathf.Min(requiredFuel, item.m_stack), 0);
    }

    return shieldGenerator.RepeatAddFuel(sw, user, item, repeatCount);
  }
}
