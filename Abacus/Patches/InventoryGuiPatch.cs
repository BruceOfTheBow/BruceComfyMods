using System;
using System.Runtime.CompilerServices;
using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using static Abacus.PluginConfig;

namespace Abacus.Patches {
  [HarmonyPatch(typeof(InventoryGui))]
  public class InventoryGuiPatch {
    private static bool _isInitialized = false;
    private static RectTransform _smallValueSelect;
    private static RectTransform _largeValueSelect;

    private static TMPro.TMP_Text _smallText;
    private static TMPro.TMP_Text _largeText;

    private static RectTransform _smallValueDecrement;
    private static RectTransform _smallValueIncrement;

    private static RectTransform _largeValueDecrement;
    private static RectTransform _largeValueIncrement;

    public static int _currentSmallIncrement = 0;
    public static int _currentLargeIncrement = 0;

    private static readonly string _okButtonName = "Button_ok";

    [HarmonyPrefix]
    [HarmonyPatch(nameof(InventoryGui.ShowSplitDialog))]
    public static void ShowSplitDialogPrefix(InventoryGui __instance, ItemDrop.ItemData item, Inventory fromIventory) {
      if (!IsModEnabled.Value
            || !__instance
            || !__instance.m_splitDialog.m_splitSlider) {
        return;
      }

      if (!_isInitialized) {
        InitializeAbacusUI(__instance);
        _isInitialized = true;
      }

      if (_isInitialized) {
        UpdateAbacusUI(item);
      }
    }

    public static void InitializeAbacusUI(InventoryGui inventoryGui) {
      _smallValueDecrement = CreateButton(inventoryGui, "smallDecrement", "-", new Vector2(50, 40), new Vector3(70, 60, 0));
      _smallValueSelect = CreateButton(inventoryGui, "smallSelect", _currentSmallIncrement.ToString(), new Vector2(50, 40), new Vector3(120, 60, 0));
      _smallValueIncrement = CreateButton(inventoryGui, "smallIncrement", "+", new Vector2(50, 40), new Vector3(170, 60, 0));

      _largeValueDecrement = CreateButton(inventoryGui, "largeDecrement", "-", new Vector2(50, 40), new Vector3(70, 20, 0));
      _largeValueSelect = CreateButton(inventoryGui, "largeSelect", _currentLargeIncrement.ToString(), new Vector2(50, 40), new Vector3(120, 20, 0));
      _largeValueIncrement = CreateButton(inventoryGui, "largeIncrement", "+", new Vector2(50, 40), new Vector3(170, 20, 0));

      SplitDialog splitDialog = inventoryGui.m_splitDialog;
      if (_smallValueDecrement.TryGetComponent(out Button svdButton)) {
        svdButton.onClick.AddListener(() => {
          splitDialog.m_splitSlider.value
              = Mathf.Clamp(((int)splitDialog.m_splitSlider.value) - _currentSmallIncrement,
                  splitDialog.m_splitSlider.minValue,
                  splitDialog.m_splitSlider.maxValue);

          splitDialog.SliderChanged(splitDialog.m_splitSlider.value);
        });
      }

      if (_smallValueSelect.TryGetComponent(out Button svsButton)) {
        svsButton.onClick.AddListener(() => {
          splitDialog.m_splitSlider.value
              = Mathf.Clamp(_currentSmallIncrement,
                  splitDialog.m_splitSlider.minValue,
                  splitDialog.m_splitSlider.maxValue);

          splitDialog.SliderChanged(splitDialog.m_splitSlider.value);
          inventoryGui.OnSplitOk();
        });
      }

      if (_smallValueIncrement.TryGetComponent(out Button sviButton)) {
        sviButton.onClick.AddListener(() => {
          splitDialog.m_splitSlider.value
              = Mathf.Clamp(((int)splitDialog.m_splitSlider.value) + _currentSmallIncrement,
                  splitDialog.m_splitSlider.minValue,
                  splitDialog.m_splitSlider.maxValue);

          splitDialog.SliderChanged(splitDialog.m_splitSlider.value);
        });
      }

      if (_largeValueDecrement.TryGetComponent(out Button lvdButton)) {
        lvdButton.onClick.AddListener(() => {
          splitDialog.m_splitSlider.value
              = Mathf.Clamp(((int)splitDialog.m_splitSlider.value) - _currentLargeIncrement,
                  splitDialog.m_splitSlider.minValue,
                  splitDialog.m_splitSlider.maxValue);

          splitDialog.SliderChanged(splitDialog.m_splitSlider.value);
        });
      }

      if (_largeValueSelect.TryGetComponent(out Button lvsButton)) {
        lvsButton.onClick.AddListener(() => {
          splitDialog.m_splitSlider.value
              = Mathf.Clamp(_currentLargeIncrement,
                  splitDialog.m_splitSlider.minValue,
                  splitDialog.m_splitSlider.maxValue);

          splitDialog.SliderChanged(splitDialog.m_splitSlider.value);
          inventoryGui.OnSplitOk();
        });
      }

      if (_largeValueIncrement.TryGetComponent(out Button lviButton)) {
        lviButton.onClick.AddListener(() => {
          splitDialog.m_splitSlider.value
              = Mathf.Clamp(((int)splitDialog.m_splitSlider.value) + _currentLargeIncrement,
                  splitDialog.m_splitSlider.minValue,
                  splitDialog.m_splitSlider.maxValue);

          splitDialog.SliderChanged(splitDialog.m_splitSlider.value);
        });
      }
    }

    public static void UpdateSmallIncrementText() {
      if (!_smallValueSelect) {
        return;
      }

      if (!_smallText) {
        RectTransform textTransform = (RectTransform)_smallValueSelect.transform.Find("Text");

        if (textTransform == null || !textTransform.TryGetComponent(out TMPro.TMP_Text tmpText)) {
          return;
        }

        _smallText = tmpText;
      }

      _smallText.text = _currentSmallIncrement.ToString();
    }

    public static void UpdateLargeIncrementText() {
      if (!_largeValueSelect) {
        return;
      }

      if (!_largeText) {
        RectTransform textTransform = (RectTransform)_largeValueSelect.transform.Find("Text");

        if (textTransform == null || !textTransform.TryGetComponent(out TMPro.TMP_Text tmpText)) {
          return;
        }

        _largeText = tmpText;
      }

      _largeText.text = _currentLargeIncrement.ToString();
    }


    private static RectTransform CreateButton(InventoryGui inventoryGui, string name, string text, Vector2 size, Vector3 position) {
      RectTransform okButtonTransform = FindOkButton(inventoryGui.m_splitDialog.m_panel);

      if (okButtonTransform == null) {
        ZLog.LogWarning("Ok button not found.");
        return null;
      }

      RectTransform newButtonTransform = UnityEngine.Object.Instantiate(okButtonTransform, okButtonTransform.transform.parent);
      newButtonTransform.name = name;
      newButtonTransform.localPosition = position;
      newButtonTransform.sizeDelta = size;

      RectTransform textTransform = (RectTransform)newButtonTransform.transform.Find("Text");

      if (textTransform == null) {
        ZLog.LogWarning("Text transform not found. Text not set.");
        return newButtonTransform;
      }

      if (!textTransform.TryGetComponent(out TMPro.TMP_Text tmpText)) {
        ZLog.LogWarning("TMP Text component not found on Text");
        return newButtonTransform;
      }

      TMPro.TMP_Text component = textTransform.GetComponent<TMPro.TMP_Text>();
      component.text = text;
      component.m_enableAutoSizing = true;

      return newButtonTransform;
    }

    private static RectTransform FindOkButton(Transform splitPanel) {
      return (RectTransform)splitPanel.transform.Find(_okButtonName);
    }

    public static void UpdateAbacusUI(ItemDrop.ItemData item) {
      if (item.m_shared.m_maxStackSize == 10) {
        _currentSmallIncrement = SmallIncrement10.Value;
        _currentLargeIncrement = LargeIncrement10.Value;
      }

      if (item.m_shared.m_maxStackSize == 20) {
        _currentSmallIncrement = SmallIncrement20.Value;
        _currentLargeIncrement = LargeIncrement20.Value;
      }

      if (item.m_shared.m_maxStackSize == 30) {
        _currentSmallIncrement = SmallIncrement30.Value;
        _currentLargeIncrement = LargeIncrement30.Value;
      }

      if (item.m_shared.m_maxStackSize == 50) {
        _currentSmallIncrement = SmallIncrement50.Value;
        _currentLargeIncrement = LargeIncrement50.Value;
      }

      if (item.m_shared.m_maxStackSize == 100) {
        _currentSmallIncrement = SmallIncrement100.Value;
        _currentLargeIncrement = LargeIncrement100.Value;
      }

      if (item.m_shared.m_maxStackSize == 999) {
        _currentSmallIncrement = SmallIncrement999.Value;
        _currentLargeIncrement = LargeIncrement999.Value;
      }

      UpdateSmallIncrementText();
      UpdateLargeIncrementText();
    }
  }
}
