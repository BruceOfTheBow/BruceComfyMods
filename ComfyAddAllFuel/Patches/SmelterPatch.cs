namespace AddAllFuel;

using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using UnityEngine;

using static PluginConfig;

[HarmonyPatch(typeof(Smelter))]
static class SmelterPatch {
  [HarmonyTranspiler]
  [HarmonyPatch(nameof(Smelter.Awake))]
  static IEnumerable<CodeInstruction> AwakeTranspiler(
      IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
    return new CodeMatcher(instructions, generator)
        .Start()
        .MatchStartForward(
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(OpCodes.Ldftn, AccessTools.Method(typeof(Smelter), nameof(Smelter.OnAddOre))),
            new CodeMatch(OpCodes.Newobj))
        .ThrowIfInvalid($"Could not patch Smelter.Awake()! (on-add-ore)")
        .Advance(offset: 3)
        .InsertAndAdvance(
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(
                OpCodes.Call, AccessTools.Method(typeof(SmelterPatch), nameof(OnAddOreCallbackDelegate))))
        .MatchStartForward(
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(OpCodes.Ldftn, AccessTools.Method(typeof(Smelter), nameof(Smelter.OnAddFuel))),
            new CodeMatch(OpCodes.Newobj))
        .ThrowIfInvalid($"Could not patch Smelter.Awake()! (on-add-fuel)")
        .Advance(offset: 3)
        .InsertAndAdvance(
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(
                OpCodes.Call, AccessTools.Method(typeof(SmelterPatch), nameof(OnAddFuelCallbackDelegate))))
        .InstructionEnumeration();
  }

  static Switch.Callback OnAddOreCallbackDelegate(Switch.Callback onAddOreCallback, Smelter smelter) {
    return IsModEnabled.Value ? new(smelter.OnRepeatAddOre) : onAddOreCallback;
  }

  public static bool OnRepeatAddOre(this Smelter smelter, Switch sw, Humanoid user, ItemDrop.ItemData item) {
    if (!IsModEnabled.Value || !ZInput.GetKey(AddAllModifier.Value)) {
      return smelter.OnAddOre(sw, user, item);
    }

    int requiredOre = smelter.m_maxOre - smelter.GetQueueSize();
    item ??= smelter.FindCookableItem(user.GetInventory());

    if (item != default) {
      requiredOre = Mathf.Max(Mathf.Min(requiredOre, item.m_stack), 0);
    }

    return smelter.RepeatAddOre(sw, user, item, requiredOre);
  }

  static Switch.Callback OnAddFuelCallbackDelegate(Switch.Callback onAddFuelCallback, Smelter smelter) {
    return IsModEnabled.Value ? new(smelter.OnRepeatAddFuel) : onAddFuelCallback;
  }

  public static bool OnRepeatAddFuel(this Smelter smelter, Switch sw, Humanoid user, ItemDrop.ItemData item) {
    if (!IsModEnabled.Value || !ZInput.GetKey(AddAllModifier.Value)) {
      return smelter.OnAddFuel(sw, user, item);
    }

    string fuelItemName = smelter.GetFuelItemName();
    item ??= user.GetInventory().GetItem(fuelItemName);

    int repeatCount = 0;

    if (item != default && item.m_shared.m_name == fuelItemName) {
      int requiredFuel = Mathf.RoundToInt(smelter.m_maxFuel - smelter.GetFuel());
      repeatCount = Mathf.Max(Mathf.Min(requiredFuel, item.m_stack), 0);
    }

    return smelter.RepeatAddFuel(sw, user, item, repeatCount);
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Smelter.FindCookableItem))]
  static bool FindCookableItemPrefix(Smelter __instance, Inventory inventory, ref ItemDrop.ItemData __result) {
    if (IsModEnabled.Value && SmelterManager.ExcludeCookableItems.Count > 0) {
      __instance.TryFindCookableItem(inventory, out __result);
      return false;
    }

    return true;
  }
}
