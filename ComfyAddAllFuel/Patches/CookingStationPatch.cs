namespace AddAllFuel;

using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using UnityEngine;

using static PluginConfig;

[HarmonyPatch(typeof(CookingStation))]
static class CookingStationPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(CookingStation.GetFreeSlot))]
  static bool GetFreeSlotPrefix(CookingStation __instance, ref int __result) {
    if (IsModEnabled.Value) {
      __result = __instance.TryGetFreeSlot(out int slotIndex) ? slotIndex : -1;
      return false;
    }

    return true;
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(CookingStation.HaveDoneItem))]
  static bool HaveDoneItemPrefix(CookingStation __instance, ref bool __result) {
    if (IsModEnabled.Value) {
      __result = __instance.HaveDoneItemByStatus();
      return false;
    }

    return true;
  }

  [HarmonyTranspiler]
  [HarmonyPatch(nameof(CookingStation.Awake))]
  static IEnumerable<CodeInstruction> AwakeTranspiler(
      IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
    return new CodeMatcher(instructions, generator)
        .Start()
        .MatchStartForward(
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(
                OpCodes.Ldftn, AccessTools.Method(typeof(CookingStation), nameof(CookingStation.OnAddFuelSwitch))),
            new CodeMatch(OpCodes.Newobj))
        .ThrowIfInvalid($"Could not patch CookingStation.Awake()! (on-add-fuel-switch)")
        .Advance(offset: 3)
        .InsertAndAdvance(
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(
                OpCodes.Call, AccessTools.Method(typeof(CookingStationPatch), nameof(OnAddFuelSwitchCallbackDelegate))))
        .InstructionEnumeration();
  }

  static Switch.Callback OnAddFuelSwitchCallbackDelegate(
      Switch.Callback onAddFuelSwitchCallback, CookingStation smelter) {
    return IsModEnabled.Value ? new(smelter.OnRepeatAddFuelSwitch) : onAddFuelSwitchCallback;
  }

  public static bool OnRepeatAddFuelSwitch(
      this CookingStation cookingStation, Switch sw, Humanoid user, ItemDrop.ItemData item) {
    if (!IsModEnabled.Value || !ZInput.GetKey(AddAllModifier.Value)) {
      return cookingStation.OnAddFuelSwitch(sw, user, item);
    }

    string fuelItemName = cookingStation.GetFuelItemName();
    item ??= user.GetInventory().GetItem(fuelItemName);

    int repeatCount = 0;

    if (item != default && item.m_shared.m_name == fuelItemName) {
      int requiredFuel = Mathf.RoundToInt(cookingStation.m_maxFuel - cookingStation.GetFuel());
      repeatCount = Mathf.Max(Mathf.Min(requiredFuel, item.m_stack), 0);
    }

    return cookingStation.RepeatAddFuel(sw, user, item, repeatCount);
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(CookingStation.OnAddFoodSwitch))]
  static bool OnAddFoodSwitchPrefix(
      CookingStation __instance, Humanoid user, ItemDrop.ItemData item, ref bool __result) {
    if (IsModEnabled.Value && ZInput.GetKey(AddAllModifier.Value)) {
      __result = item == default ? OnRepeatInteract(__instance, user) : OnRepeatUseItem(__instance, user, item);
      return false;
    }

    return true;
  }

  public static bool OnRepeatUseItem(CookingStation cookingStation, Humanoid user, ItemDrop.ItemData item) {
    int freeSlotCount = cookingStation.GetFreeSlotCount();
    return cookingStation.RepeatUseItem(user, item, Mathf.Min(freeSlotCount, item.m_stack));
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(CookingStation.Interact))]
  static bool InteractPrefix(CookingStation __instance, Humanoid user, bool hold, ref bool __result) {
    if (!hold && __instance.m_addFoodSwitch == default && IsModEnabled.Value && ZInput.GetKey(AddAllModifier.Value)) {
      __result = OnRepeatInteract(__instance, user);
      return false;
    }

    return true;
  }

  public static bool OnRepeatInteract(CookingStation cookingStation, Humanoid user) {
    int doneItemCount = cookingStation.GetDoneItemCount();

    if (doneItemCount > 0) {
      return cookingStation.RepeatInteract(user, doneItemCount);
    }

    int freeSlotCount = cookingStation.GetFreeSlotCount();
    return cookingStation.RepeatInteract(user, freeSlotCount);
  }
}
