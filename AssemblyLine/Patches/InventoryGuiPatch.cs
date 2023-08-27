using System;

using System.Collections.Generic;
using System.Linq;

using HarmonyLib;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using static AssemblyLine.PluginConfig;

namespace AssemblyLine.Patches {
  [HarmonyPatch(typeof(InventoryGui))]
  public class InventoryGuiPatch {
    private static RectTransform _incrementButtonTransform;
    private static RectTransform _decrementButtonTransform;
    private static RectTransform _countText;

    private static Button _craftButton;
    private static Button _incrementButton;
    private static Button _decrementButton;

    private static int _maxCraftAmount = 1;

    private static Dictionary<string, int> _requirementAmountByName = new Dictionary<string, int>();
    private static Dictionary<string, int> _maxAmountByName = new Dictionary<string, int>();
    private static Dictionary<string, Transform> _requirementTransformByName = new Dictionary<string, Transform>();

    private static int _craftsRemaining = 0;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.Show))]
    public static void ShowPostfix(InventoryGui __instance, Container container) {
      if (_incrementButtonTransform != null || _decrementButtonTransform != null || _craftButton != null) {
        return;
      }
      _craftButton = __instance.m_craftButton;

      CreateCountText();

      SetCraftAmountToMin();

      ScaleCraftButton();      

      _incrementButtonTransform = CreateButton(__instance, "incrementButton", "+");
      _decrementButtonTransform = CreateButton(__instance, "decrementButton", "-");

      _incrementButton = _incrementButtonTransform.GetComponent<Button>();
      _decrementButton = _decrementButtonTransform.GetComponent<Button>();

      _incrementButtonTransform.pivot = new Vector2(1f, 0.75f);
      _decrementButtonTransform.pivot = new Vector2(1f, 0f);
      
      _incrementButtonTransform.GetComponent<Button>().onClick.AddListener(new UnityAction(OnIncrementPressed));
      _decrementButtonTransform.GetComponent<Button>().onClick.AddListener(new UnityAction(OnDecrementPressed));
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.OnTabCraftPressed))]
    public static void OnTabCraftPressed(InventoryGui __instance) {
      SetButtonInteractable(true);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.OnTabUpgradePressed))]
    public static void OnTabUpgradePressed(InventoryGui __instance) {
      SetButtonInteractable(false);
      SetCraftAmountToMin();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.OnCraftPressed))]
    public static void OnCraftPressedPrefix(InventoryGui __instance) {
      _craftsRemaining =  int.Parse(_countText.GetComponent<Text>().text) - 1;      
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.OnCraftCancelPressed))]
    public static void OnCraftCancelPressedPostfix(InventoryGui __instance) {
      _craftsRemaining = 0;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.UpdateRecipe))]
    public static void OnUpdateRecipepostfix(InventoryGui __instance, Player player, float dt) {
      if (__instance == null || player == null) {
        return;
      }

      if (!player.GetInventory().CanAddItem(__instance.m_selectedRecipe.Value, __instance.m_selectedRecipe.Key.m_amount)) {
        _craftsRemaining = 0;
        return;
      }

      if (__instance.m_craftTimer < 0 && _craftsRemaining > 0) {
        UpdateMaxAmount();

        _craftsRemaining--;
        __instance.m_craftTimer = 0;

        DecrementCraftAmount(1);
        SetRequirementText();

        player.GetCurrentCraftingStation().m_craftItemEffects.Create(player.transform.position, Quaternion.identity, null, 1f, -1);
        __instance.m_craftItemEffects.Create(player.transform.position, Quaternion.identity, null, 1f, -1);
        __instance.m_craftRecipe = __instance.m_selectedRecipe.Key;
      }
    }


    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.SetRecipe))]
    public static void OnSetRecipePostfix(InventoryGui __instance, int index, bool center) {
      if (IsCraftingMultiple()) {
        return;
      }

      _requirementAmountByName.Clear();
      _maxAmountByName.Clear();
      _requirementTransformByName.Clear();

      SetMaxCraftAmount(__instance);

      if (_countText) {
        SetCraftAmountToMin();
      }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.SetupRequirement))]
    public static void SetupRequirementPostfix(InventoryGui __instance, Transform elementRoot, Piece.Requirement req, Player player, bool craft, int quality) {
      if (!_requirementAmountByName.Keys.Contains(req.m_resItem.m_itemData.m_shared.m_name)) {
        _requirementAmountByName.Add(req.m_resItem.m_itemData.m_shared.m_name, req.GetAmount(quality));
        _maxAmountByName.Add(req.m_resItem.m_itemData.m_shared.m_name, Player.m_localPlayer.GetInventory().CountItems(req.m_resItem.m_itemData.m_shared.m_name));
        _requirementTransformByName.Add(req.m_resItem.m_itemData.m_shared.m_name, elementRoot);
      }

      SetRequirementText();
    }

    public static RectTransform CreateButton(InventoryGui inventoryGui, string name, string text) {
      Transform additionalTransform = UnityEngine.Object.Instantiate(_craftButton.transform, _craftButton.transform.transform.parent);
      additionalTransform.name = name;
      Transform resultTransform = additionalTransform.transform;

      RectTransform targetTransform = (RectTransform)resultTransform.transform;
      targetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 40f);
      targetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

      RectTransform textTransform = (RectTransform)targetTransform.transform.Find("Text");
      textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 40f);
      textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);

      var component = textTransform.GetComponent<Text>();
      component.text = text;
      component.resizeTextForBestFit = true;
      return targetTransform;
    }

    public static void CreateCountText() {
      RectTransform textTransform = (RectTransform)_craftButton.transform.transform.Find("Text");
      _countText = UnityEngine.Object.Instantiate(textTransform, textTransform.parent.transform.parent);
      _countText.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50f);
      _countText.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100f);
      _countText.pivot = new Vector2(0.975f, 0f);
      _countText.GetComponent<Text>().text = "1";
    }

    private static void ScaleCraftButton() {
      _craftButton.transform.localScale = new Vector3(0.75f, 1f, 1f);
      ((RectTransform)_craftButton.transform).pivot = new Vector2(0, 0);
    }

    public static void SetRequirementText() {
      foreach (KeyValuePair<string, Transform> kvp in _requirementTransformByName) {
        _maxAmountByName[kvp.Key] = Player.m_localPlayer.GetInventory().CountItems(kvp.Key);
        kvp.Value.transform.Find("res_amount").GetComponent<Text>().text = (_requirementAmountByName[kvp.Key] * int.Parse(_countText.GetComponent<Text>().text)).ToString() + "/" + _maxAmountByName[kvp.Key].ToString();
      }
    }

    private static void OnIncrementPressed() {
      if (MaxAmountChangeModifier.Value.IsPressed()) {
        SetCraftAmountToMax();
        SetRequirementText();
        return;
      }

      if (AmountChangeModifier.Value.IsPressed()) {
        IncrementCraftAmount(10);
        SetRequirementText();
        return;
      }

      IncrementCraftAmount(1);
      SetRequirementText();
    }

    private static void OnDecrementPressed() {
      if (MaxAmountChangeModifier.Value.IsPressed()) {
        SetCraftAmountToMin();
        SetRequirementText();
        return;
      }

      if (AmountChangeModifier.Value.IsPressed()) {
        DecrementCraftAmount(10);
        SetRequirementText();
        return;
      }

      DecrementCraftAmount(1);
      SetRequirementText();
    }

    private static void IncrementCraftAmount(int amount) {
      int currentCount = int.Parse(_countText.GetComponent<Text>().text);
      int newCount = currentCount + amount; 

      if (RoundToStackSize.Value && HaveCraftRequirements(10) && currentCount == 1 && amount == 10) {
        _countText.GetComponent<Text>().text = 10.ToString();
        return;
      }

      if (HaveCraftRequirements(newCount) && amount != -1) {
        _countText.GetComponent<Text>().text = newCount.ToString();
      }
    }

    private static void DecrementCraftAmount(int amount) {
      int currentCount = int.Parse(_countText.GetComponent<Text>().text);
      int newCount = currentCount - amount;

      if (newCount < 1) {
        SetCraftAmountToMin();
        return;
      }
      
      _countText.GetComponent<Text>().text = newCount.ToString();
    }

    private static void SetButtonInteractable(bool interactable) {
      if (_incrementButton != null) {
        _incrementButton.interactable = interactable;
      }

      if (_decrementButton != null) {
        _decrementButton.interactable = interactable;
      }
    }

    private static void SetCraftAmountToMax() {
      _countText.GetComponent<Text>().text = _maxCraftAmount.ToString();
    }

    private static void SetCraftAmountToMin() {
      _countText.GetComponent<Text>().text = 1.ToString();
    }

    private static void SetMaxCraftAmount(InventoryGui inventoryGui) {
      ZLog.Log("Setting max craft amount.");
      _maxCraftAmount = GetMaxCraftAmount(inventoryGui);
    }

    private static bool IsCraftingMultiple() {
      if (_craftsRemaining > 0) {
        return true;
      }
      return false;
    }

    private static void UpdateMaxAmount() {
      foreach (KeyValuePair<string, int> kvp in _requirementAmountByName) {
        _maxAmountByName[kvp.Key] = _maxAmountByName[kvp.Key] - _requirementAmountByName[kvp.Key];
      }
    }

    private static int GetMaxCraftAmount(InventoryGui inventoryGui) {
      if (Player.m_localPlayer == null || inventoryGui.m_selectedRecipe.Key == null) {
        ZLog.Log("Local player or selected recipe key is null.");
        return 1;
      }

      if (Player.m_localPlayer.NoCostCheat()) {
        ZLog.Log("No cost enabled. Setting max to 500.");
        return 500;
      }

      List<int> craftableAmounts = new ();
      foreach (Piece.Requirement requirement in inventoryGui.m_selectedRecipe.Key.m_resources) {
        if (requirement.m_resItem && requirement.m_amount > 0) {
          int totalAmount = Player.m_localPlayer.GetInventory().CountItems(requirement.m_resItem.m_itemData.m_shared.m_name);
          int craftableAmount = totalAmount / requirement.m_amount; // Integer division rounds down by default
          craftableAmounts.Add(craftableAmount);
          ZLog.Log($"{requirement.m_resItem.m_itemData.m_shared.m_name} requires {totalAmount} with {craftableAmount} per item.");
        }
      }
      ZLog.Log($"Max amount {craftableAmounts.Min()}");
      return craftableAmounts.Min();
    }

    private static bool HaveCraftRequirements(int newCount) {
      if (_maxCraftAmount >= newCount) {
        return true;
      }

      return false;
    }
  }
}
