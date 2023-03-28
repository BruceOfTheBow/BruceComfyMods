using System;

using HarmonyLib;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using static AssemblyLine.PluginConfig;

namespace AssemblyLine.Patches {
  [HarmonyPatch(typeof(InventoryGui))]
  public class InventoryGuiPatch {
    private static RectTransform _incrementButton;
    private static RectTransform _decrementButton;
    private static RectTransform _countText;
    private static Button _craftButton;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.Show))]
    public static void ShowPostfix(InventoryGui __instance, Container container) {
      if (_incrementButton != null || _decrementButton != null || _craftButton != null) {
        return;
      }
      _craftButton = __instance.m_craftButton;

      CreateCountText();

      ScaleCraftButton();      

      _incrementButton = CreateButton(__instance, "incrementButton", "+");
      _decrementButton = CreateButton(__instance, "decrementButton", "-");

      _incrementButton.pivot = new Vector2(1f, 0.75f);
      _decrementButton.pivot = new Vector2(1f, 0f);

      _incrementButton.GetComponent<Button>().onClick.AddListener(new UnityAction(OnIncrementPressed));
      _decrementButton.GetComponent<Button>().onClick.AddListener(new UnityAction(OnDecrementPressed));

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

    private static void OnIncrementPressed() {
      if (MaxAmountChangeModifier.Value.IsPressed()) {
        SetCraftAmountToMax();
        return;
      }

      if (AmountChangeModifier.Value.IsPressed()) {
        IncrementCraftAmount(10);
        return;
      }

      IncrementCraftAmount(1);
    }

    private static void OnDecrementPressed() {
      if (MaxAmountChangeModifier.Value.IsPressed()) {
        SetCraftAmountToMin();
        return;
      }

      if (AmountChangeModifier.Value.IsPressed()) {
        DecrementCraftAmount(10);
        return;
      }

      DecrementCraftAmount(1);
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

    private static void SetCraftAmountToMax() {
      _countText.GetComponent<Text>().text = 500.ToString();
    }

    private static void SetCraftAmountToMin() {
      _countText.GetComponent<Text>().text = 1.ToString();
    }

    private static bool HaveCraftRequirements(int newCount) {
      return true;
    }

  }
}
