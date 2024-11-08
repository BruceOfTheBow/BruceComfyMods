namespace AssemblyLine;

using System.Linq;

using HarmonyLib;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[HarmonyPatch(typeof(InventoryGui))]
static class InventoryGuiPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.Show))]
  public static void ShowPostfix(InventoryGui __instance, Container container) {
    if (CraftingManager.IncrementButtonTransform != null
        || CraftingManager.DecrementButtonTransform != null
        || CraftingManager.CraftButton != null) {
      return;
    }

    CraftingManager.CraftButton = __instance.m_craftButton;

    CraftingManager.CreateCountText();
    CraftingManager.SetCraftAmountToMin();
    CraftingManager.ScaleCraftButton(__instance.m_craftButton);

    CraftingManager.IncrementButtonTransform = CraftingManager.CreateButton(__instance, "incrementButton", "+");
    CraftingManager.DecrementButtonTransform = CraftingManager.CreateButton(__instance, "decrementButton", "-");

    CraftingManager.IncrementButton = CraftingManager.IncrementButtonTransform.GetComponent<Button>();
    CraftingManager.DecrementButton = CraftingManager.DecrementButtonTransform.GetComponent<Button>();

    CraftingManager.IncrementButtonTransform.pivot = new Vector2(1f, 0.75f);
    CraftingManager.DecrementButtonTransform.pivot = new Vector2(1f, 0f);

    CraftingManager.IncrementButtonTransform.GetComponent<Button>()
        .onClick.AddListener(new UnityAction(CraftingManager.OnIncrementPressed));
    CraftingManager.DecrementButtonTransform.GetComponent<Button>()
        .onClick.AddListener(new UnityAction(CraftingManager.OnDecrementPressed));
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.OnTabCraftPressed))]
  public static void OnTabCraftPressedPostfix(InventoryGui __instance) {
    CraftingManager.SetButtonInteractable(true);
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.OnTabUpgradePressed))]
  public static void OnTabUpgradePressedPostfix(InventoryGui __instance) {
    CraftingManager.SetButtonInteractable(false);
    CraftingManager.SetCraftAmountToMin();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.OnCraftPressed))]
  public static void OnCraftPressedPrefix(InventoryGui __instance) {
    CraftingManager.CraftsRemaining = CraftingManager.CountValue - 1;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.OnCraftCancelPressed))]
  public static void OnCraftCancelPressedPostfix(InventoryGui __instance) {
    CraftingManager.CraftsRemaining = 0;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.Hide))]
  public static void HidePostfix(InventoryGui __instance) {
    CraftingManager.CraftsRemaining = 0;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.UpdateRecipe))]
  public static void UpdateRecipePostfix(InventoryGui __instance, Player player, float dt) {
    if (__instance == null
        || Player.m_localPlayer == null
        || Player.m_localPlayer != player
        || Player.m_localPlayer.GetInventory() == null
        || __instance.m_selectedRecipe.Recipe == null) {

      return;
    }

    if (__instance.InCraftTab()
        && CraftingManager.IncrementButton != null
        && CraftingManager.DecrementButton != null) {
      CraftingManager.
              IncrementButton.interactable = true;
      CraftingManager.DecrementButton.interactable = true;
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

      if (Player.m_localPlayer.GetCurrentCraftingStation() != null) {
        player.GetCurrentCraftingStation().m_craftItemEffects.Create(
            player.transform.position, Quaternion.identity, null, 1f, -1);
      }

      __instance.m_craftItemEffects.Create(player.transform.position, Quaternion.identity, null, 1f, -1);
      __instance.m_craftRecipe = __instance.m_selectedRecipe.Recipe;
    }
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
  public static void SetupRequirementPostfix(
      InventoryGui __instance, Transform elementRoot, Piece.Requirement req, Player player, bool craft, int quality) {
    if (!InventoryGui.IsVisible()) {
      return;
    }

    if (!CraftingManager.RequirementAmountByName.Keys.Contains(req.m_resItem.m_itemData.m_shared.m_name)) {
      CraftingManager.RequirementAmountByName.Add(req.m_resItem.m_itemData.m_shared.m_name, req.GetAmount(quality));
      CraftingManager.MaxAmountByName.Add(
          req.m_resItem.m_itemData.m_shared.m_name,
          Player.m_localPlayer.GetInventory().CountItems(req.m_resItem.m_itemData.m_shared.m_name));
      CraftingManager.RequirementTransformByName.Add(req.m_resItem.m_itemData.m_shared.m_name, elementRoot);
    }

    CraftingManager.SetRequirementText();
  }
}
