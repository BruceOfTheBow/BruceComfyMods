namespace AddAllFuel;

using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using UnityEngine;

using static PluginConfig;

[HarmonyPatch(typeof(Fireplace))]
static class FireplacePatch {
  [HarmonyTranspiler]
  [HarmonyPatch(nameof(Fireplace.Interact))]
  static IEnumerable<CodeInstruction> InteractTranspiler(
      IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
    return new CodeMatcher(instructions, generator)
        .Start()
        .MatchStartForward(
            new CodeMatch(OpCodes.Ldloc_1),
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(Fireplace), nameof(Fireplace.m_fuelItem))),
            new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(ItemDrop), nameof(ItemDrop.m_itemData))),
            new CodeMatch(
                OpCodes.Ldfld, AccessTools.Field(typeof(ItemDrop.ItemData), nameof(ItemDrop.ItemData.m_shared))),
            new CodeMatch(
                OpCodes.Ldfld,
                AccessTools.Field(typeof(ItemDrop.ItemData.SharedData), nameof(ItemDrop.ItemData.SharedData.m_name))),
            new CodeMatch(OpCodes.Ldc_I4_1),
            new CodeMatch(OpCodes.Ldc_I4_M1),
            new CodeMatch(OpCodes.Ldc_I4_1),
            new CodeMatch(
                OpCodes.Callvirt,
                AccessTools.Method(
                    typeof(Inventory),
                    nameof(Inventory.RemoveItem),
                    parameters: [typeof(string), typeof(int), typeof(int), typeof(bool)])))
        .ThrowIfInvalid($"Could not patch Fireplace.Interact()! (remove-item)")
        .CreateLabel(out Label removeItemLabel)
        .InsertAndAdvance(
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(OpCodes.Ldarg_1),
            new CodeInstruction(OpCodes.Ldnull),
            new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(FireplacePatch), nameof(AddFuelDelegate))),
            new CodeInstruction(OpCodes.Brfalse, removeItemLabel),
            new CodeInstruction(OpCodes.Ldc_I4_1),
            new CodeInstruction(OpCodes.Ret))
        .InstructionEnumeration();
  }

  [HarmonyTranspiler]
  [HarmonyPatch(nameof(Fireplace.UseItem))]
  static IEnumerable<CodeInstruction> UseItemTranspiler(
      IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
    return new CodeMatcher(instructions, generator)
        .Start()
        .MatchStartForward(
            new CodeMatch(OpCodes.Ldloc_0),
            new CodeMatch(OpCodes.Ldarg_2),
            new CodeMatch(OpCodes.Ldc_I4_1),
            new CodeMatch(
                OpCodes.Callvirt,
                AccessTools.Method(
                    typeof(Inventory),
                    nameof(Inventory.RemoveItem),
                    parameters: [typeof(ItemDrop.ItemData), typeof(int)])))
        .ThrowIfInvalid($"Could not patch Fireplace.UseItem()! (remove-item)")
        .CreateLabel(out Label removeItemLabel)
        .InsertAndAdvance(
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(OpCodes.Ldarg_1),
            new CodeInstruction(OpCodes.Ldarg_2),
            new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(FireplacePatch), nameof(AddFuelDelegate))),
            new CodeInstruction(OpCodes.Brfalse, removeItemLabel),
            new CodeInstruction(OpCodes.Ldc_I4_1),
            new CodeInstruction(OpCodes.Ret))
        .InstructionEnumeration();
  }

  static bool AddFuelDelegate(Fireplace fireplace, Humanoid user, ItemDrop.ItemData item) {
    if (IsModEnabled.Value && ZInput.GetKey(AddAllModifier.Value)) {
      string fuelItemName = fireplace.GetFuelItemName();
      item ??= user.GetInventory().GetItem(fuelItemName);

      if (item == default || item.m_shared.m_name != fuelItemName) {
        return false;
      }

      int requiredFuel = Mathf.CeilToInt(fireplace.m_maxFuel - fireplace.GetFuel());
      requiredFuel = Mathf.Max(Mathf.Min(requiredFuel, item.m_stack), 0);

      if (requiredFuel <= 0) {
        return false;
      }

      if (!user.GetInventory().RemoveItem(item, requiredFuel)) {
        return false;
      }

      fireplace.AddFuel(requiredFuel);
      return true;
    }

    return false;
  }
}
