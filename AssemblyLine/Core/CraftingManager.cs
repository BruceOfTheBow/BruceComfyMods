namespace AssemblyLine;

using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using static PluginConfig;

public static class CraftingManager {
  public static RectTransform IncrementButtonTransform { get; private set; }
  public static RectTransform DecrementButtonTransform { get; private set; }
  public static RectTransform CountText { get; private set; }

  public static Button CraftButton { get; private set; }
  public static Button IncrementButton { get; private set; }
  public static Button DecrementButton { get; private set; }

  public static int MaxCraftAmount = 1;

  public static Dictionary<string, int> RequirementAmountByName = [];
  public static Dictionary<string, int> MaxAmountByName = [];
  public static Dictionary<string, Transform> RequirementTransformByName = [];

  public static int CraftsRemaining = 0;

  public static TMP_Text CountLabel { get; private set; }
  public static int CountValue { get; private set; }

  public static void SetCountValue(int value) {
    CountLabel.SetText(value.ToString());
    CountValue = value;
  }

  public static void CreateOrSetupUI(InventoryGui inventoryGui) {
    if (!CraftButton) {
      CreateUI(inventoryGui);
    }
  }

  static void CreateUI(InventoryGui inventoryGui) {
    CraftButton = inventoryGui.m_craftButton;
    ScaleCraftButton(CraftButton);

    CreateIncrementDecrementButtons(CraftButton);
    CreateCountText(CraftButton);

    SetCraftAmountToMin();
  }

  static void CreateIncrementDecrementButtons(Button craftButton) {
    IncrementButtonTransform = CreateButton(craftButton, "Increment", "+");
    IncrementButtonTransform.anchoredPosition = new(0f, -7.5f);
    IncrementButtonTransform.anchorMin = Vector2.one;
    IncrementButtonTransform.anchorMax = Vector2.one;
    IncrementButtonTransform.pivot = Vector2.one;
    IncrementButtonTransform.sizeDelta = new(40f, 30f);

    IncrementButton = IncrementButtonTransform.GetComponent<Button>();
    IncrementButton.onClick.AddListener(new UnityAction(OnIncrementPressed));

    DecrementButtonTransform = CreateButton(craftButton, "Decrement", "-");
    DecrementButtonTransform.anchoredPosition = new(0f, 2.5f);
    DecrementButtonTransform.anchorMin = Vector2.right;
    DecrementButtonTransform.anchorMax = Vector2.right;
    DecrementButtonTransform.pivot = Vector2.right;
    DecrementButtonTransform.sizeDelta = new(40f, 30f);

    DecrementButton = DecrementButtonTransform.GetComponent<Button>();
    DecrementButton.onClick.AddListener(new UnityAction(OnDecrementPressed));
  }

  static void CreateCountText(Button craftButton) {
    RectTransform textTransform = (RectTransform) craftButton.transform.transform.Find("Text");
    CountText = Object.Instantiate(textTransform, textTransform.parent.transform.parent);
    CountText.name = "Count";

    CountText.anchoredPosition = new(-45f, 0f);
    CountText.anchorMin = Vector2.right;
    CountText.anchorMax = Vector2.one;
    CountText.pivot = Vector2.right;
    CountText.sizeDelta = new(50f, -5f);

    CountLabel = CountText.GetComponent<TMP_Text>();
  }

  static RectTransform CreateButton(Button craftButton, string name, string text) {
    Transform buttonTransform = Object.Instantiate(craftButton.transform, craftButton.transform.transform.parent);
    buttonTransform.name = name;

    RectTransform labelRectTransform = (RectTransform) buttonTransform.Find("Text");
    labelRectTransform.anchoredPosition = Vector3.zero;
    labelRectTransform.anchorMin = Vector2.zero;
    labelRectTransform.anchorMax = Vector2.one;
    labelRectTransform.pivot = new(0.5f, 0.5f);
    labelRectTransform.sizeDelta = Vector2.zero;

    TMP_Text label = labelRectTransform.GetComponent<TMP_Text>();
    label.text = text;
    label.enableAutoSizing = true;

    return (RectTransform) buttonTransform;
  }

  public static void DecrementCraftAmount(int amount) {
    int currentCount = CountValue;
    int newCount = currentCount - amount;

    if (newCount < 1) {
      SetCraftAmountToMin();
      return;
    }

    SetCountValue(newCount);
  }

  public static int GetMaxCraftAmount(InventoryGui inventoryGui) {
    if (Player.m_localPlayer == null || inventoryGui.m_selectedRecipe.Recipe == null) {
      return 1;
    }

    if (Player.m_localPlayer.NoCostCheat()) {
      return 500;
    }

    List<int> craftableAmounts = [];

    foreach (Piece.Requirement requirement in inventoryGui.m_selectedRecipe.Recipe.m_resources) {
      if (requirement.m_resItem && requirement.m_amount > 0) {
        int totalAmount = Player.m_localPlayer.GetInventory().CountItems(
            requirement.m_resItem.m_itemData.m_shared.m_name);
        int craftableAmount = totalAmount / requirement.m_amount; // Integer division rounds down by default
        craftableAmounts.Add(craftableAmount);
      }
    }

    return craftableAmounts.Min() == 0 ? 1 : craftableAmounts.Min();
  }

  public static bool HaveCraftRequirements(int newCount) {
    if (MaxCraftAmount >= newCount) {
      return true;
    }

    return false;
  }

  public static void IncrementCraftAmount(int amount) {
    int currentCount = CountValue;
    int newCount = currentCount + amount;

    if (RoundToStackSize.Value && HaveCraftRequirements(10) && currentCount == 1 && amount == 10) {
      SetCountValue(10);
      return;
    }

    if (HaveCraftRequirements(newCount) && amount != -1) {
      SetCountValue(newCount);
    }
  }

  public static bool IsCraftingMultiple() {
    if (CraftsRemaining > 0) {
      return true;
    }
    return false;
  }

  public static void OnDecrementPressed() {
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

  public static void OnIncrementPressed() {
    SetMaxCraftAmount(InventoryGui.instance);

    if (MaxAmountChangeModifier.Value.IsPressed()) {
      SetCraftAmountToMax();
      SetRequirementText();
      return;
    }

    if (AmountChangeModifier.Value.IsPressed()) {
      if (!HaveCraftRequirements(10)) {
        SetCraftAmountToMax();
      }


      IncrementCraftAmount(10);
      SetRequirementText();
      return;
    }

    IncrementCraftAmount(1);
    SetRequirementText();
  }

  static void ScaleCraftButton(Button craftButton) {
    if (craftButton.TryGetComponent(out RectTransform rectTransform)) {
      rectTransform.anchoredPosition = new(0f, 2.5f);
      rectTransform.pivot = Vector2.zero;
      rectTransform.sizeDelta = new(-100f, -10f);
    }
  }

  public static void SetButtonInteractable(bool interactable) {
    if (IncrementButton != null) {
      IncrementButton.interactable = interactable;
    }

    if (DecrementButton != null) {
      DecrementButton.interactable = interactable;
    }
  }

  public static void SetCraftAmountToMax() {
    SetCountValue(MaxCraftAmount);
  }

  public static void SetCraftAmountToMin() {
    SetCountValue(1);
  }

  public static void SetMaxCraftAmount(InventoryGui inventoryGui) {
    MaxCraftAmount = GetMaxCraftAmount(inventoryGui);
  }

  public static void SetRequirementText() {
    foreach (KeyValuePair<string, Transform> kvp in RequirementTransformByName) {
      MaxAmountByName[kvp.Key] = Player.m_localPlayer.GetInventory().CountItems(kvp.Key);

      kvp.Value.transform.Find("res_amount").GetComponent<TMPro.TMP_Text>().text =
          (RequirementAmountByName[kvp.Key] * CountValue).ToString()
              + "/" + MaxAmountByName[kvp.Key].ToString();
    }
  }

  public static void UpdateMaxAmount() {
    foreach (KeyValuePair<string, int> kvp in RequirementAmountByName) {
      MaxAmountByName[kvp.Key] = MaxAmountByName[kvp.Key] - RequirementAmountByName[kvp.Key];
    }
  }
}
