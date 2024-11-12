namespace AssemblyLine;

using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using ComfyLib;

using HarmonyLib;

using UnityEngine;

[HarmonyPatch(typeof(InventoryGui))]
static class InventoryGuiPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.Show))]
  static void ShowPostfix(InventoryGui __instance, Container container) {
    CraftingManager.CreateOrSetupUI(__instance);
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.OnTabCraftPressed))]
  static void OnTabCraftPressedPostfix(InventoryGui __instance) {
    CraftingManager.SetButtonInteractable(true);
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.OnTabUpgradePressed))]
  static void OnTabUpgradePressedPostfix(InventoryGui __instance) {
    CraftingManager.SetButtonInteractable(false);
    CraftingManager.SetCraftAmountToMin();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.OnCraftPressed))]
  static void OnCraftPressedPostfix(InventoryGui __instance) {
    CraftingManager.CraftsRemaining = CraftingManager.CountValue - 1;
  }

  [HarmonyTranspiler]
  [HarmonyPatch(nameof(InventoryGui.OnCraftPressed))]
  static IEnumerable<CodeInstruction> OnCraftPressedTranspiler(IEnumerable<CodeInstruction> instructions) {
    return new CodeMatcher(instructions)
        .Start()
        .MatchStartForward(
            new CodeMatch(OpCodes.Ldc_I4_0),
            new CodeMatch(
                OpCodes.Stfld,
                AccessTools.Field(typeof(InventoryGui), nameof(InventoryGui.m_multiCrafting))),
            new CodeMatch(OpCodes.Ldarg_0))
        .ThrowIfInvalid($"Could not patch InventoryGui.OnCraftPressed()! (get-button-alt-place)")
        .Advance(offset: 2)
        .InsertAndAdvance(
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(
                OpCodes.Call,
                AccessTools.Method(typeof(InventoryGuiPatch), nameof(SetIsMultiCraftingDelegate))),
            new CodeInstruction(
                OpCodes.Stfld,
                AccessTools.Field(typeof(InventoryGui), nameof(InventoryGui.m_multiCrafting))))
        .InstructionEnumeration();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.OnCraftCancelPressed))]
  static void OnCraftCancelPressedPostfix(InventoryGui __instance) {
    CraftingManager.CraftsRemaining = 0;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.Hide))]
  static void HidePostfix(InventoryGui __instance) {
    CraftingManager.CraftsRemaining = 0;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.UpdateRecipe))]
  static void UpdateRecipePostfix(InventoryGui __instance, Player player, float dt) {
    if (!__instance.m_selectedRecipe.Recipe) {
      return;
    }

    if (!player.GetInventory().CanAddItem(
        __instance.m_selectedRecipe.Recipe.m_item.m_itemData,
        __instance.m_selectedRecipe.Recipe.m_amount)) {
      CraftingManager.DecrementCraftAmount(1);
      CraftingManager.CraftsRemaining = 0;
      return;
    }

    if (__instance.m_craftTimer < 0 && CraftingManager.CraftsRemaining > 0) {
      CraftingManager.CraftsRemaining--;
      __instance.m_craftTimer = 0;

      CraftingManager.DecrementCraftAmount(1);
      CraftingManager.UpdateMaxAmount();
      CraftingManager.SetRequirementText();

      CraftingStation craftingStation = player.GetCurrentCraftingStation();

      EffectList craftItemEffects =
          craftingStation ? craftingStation.m_craftItemEffects : __instance.m_craftItemEffects;

      craftItemEffects?.Create(player.transform.position, Quaternion.identity);

      __instance.m_craftRecipe = __instance.m_selectedRecipe.Recipe;
    }
  }

  [HarmonyTranspiler]
  [HarmonyPatch(nameof(InventoryGui.UpdateRecipe))]
  static IEnumerable<CodeInstruction> UpdateRecipeTranspiler(IEnumerable<CodeInstruction> instructions) {
    return new CodeMatcher(instructions)
        .Start()
        .MatchStartForward(
            new CodeMatch(OpCodes.Ldc_I4_0),
            new CodeMatch(OpCodes.Stloc_S),
            new CodeMatch(OpCodes.Ldloc_2),
            new CodeMatch(OpCodes.Brtrue))
        .ThrowIfInvalid($"Could not patch InventoryGui.UpdateRecipe()! (get-button-alt-place)")
        .Advance(offset: 1)
        .SaveInstruction(out CodeInstruction stLocSv5Instruction)
        .Advance(offset: 1)
        .InsertAndAdvance(
            new CodeInstruction(
                OpCodes.Call,
                AccessTools.Method(typeof(InventoryGuiPatch), nameof(SetIsMultiCraftingDelegate))),
            stLocSv5Instruction)
        .InstructionEnumeration();
  }

  static bool SetIsMultiCraftingDelegate() {
    return false;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.SetRecipe))]
  static void SetRecipePostfix(InventoryGui __instance, int index, bool center) {
    if (CraftingManager.IsCraftingMultiple()) {
      return;
    }

    CraftingManager.RequirementAmountByName.Clear();
    CraftingManager.MaxAmountByName.Clear();
    CraftingManager.RequirementTransformByName.Clear();
    CraftingManager.SetMaxCraftAmount(__instance);

    if (CraftingManager.CountText) {
      CraftingManager.SetCraftAmountToMin();
    }
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.SetupRequirement))]
  static void SetupRequirementPostfix(
      InventoryGui __instance, Transform elementRoot, Piece.Requirement req, Player player, bool craft, int quality) {
    if (!InventoryGui.IsVisible()) {
      return;
    }

    string resourceItemName = req.m_resItem.m_itemData.m_shared.m_name;

    if (!CraftingManager.RequirementAmountByName.Keys.Contains(resourceItemName)) {
      CraftingManager.RequirementAmountByName.Add(resourceItemName, req.GetAmount(quality));
      CraftingManager.MaxAmountByName.Add(
          resourceItemName, Player.m_localPlayer.GetInventory().CountItems(resourceItemName));
      CraftingManager.RequirementTransformByName.Add(resourceItemName, elementRoot);
    }

    CraftingManager.SetRequirementText();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.UpdateCraftingPanel))]
  static void UpdateCraftingPanelPostfix(InventoryGui __instance) {
    CraftingManager.SetButtonInteractable(__instance.InCraftTab());
  }
}
