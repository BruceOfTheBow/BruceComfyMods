namespace ComfyQuickSlots;

using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using static PluginConfig;

[HarmonyPatch(typeof(HotkeyBar))]
static class HotkeyBarPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(HotkeyBar.UpdateIcons))]
  static bool HotkeyBarPrefix(HotkeyBar __instance, Player player) {
    if (__instance.name != HotkeyBarManager.QuickSlotsHotKeyBarName) {
      return true;
    }

    if (__instance.m_elements == null) {
      return false;
    }

    HotkeyBarManager.ResetHotkeyTexts();

    if (player == null
    || player.IsDead()
    || !EnableQuickslots.Value
    || __instance.m_items == null) {
      return false;
    }

    HotkeyBarManager.RemoveElements(__instance);
    HotkeyBarManager.ResetItems(__instance, player);


    for (int index = 0; index < QuickSlotsManager.QuickSlotsCount; ++index) {
      HotkeyBar.ElementData elementData =
          new HotkeyBar.ElementData() { m_go = Object.Instantiate(__instance.m_elementPrefab, __instance.transform) };
      elementData.m_go.transform.localPosition = new Vector3(index * __instance.m_elementSpace, 0.0f, 0.0f);
      elementData.m_icon = elementData.m_go.transform.transform.Find("icon").GetComponent<Image>();
      elementData.m_durability = elementData.m_go.transform.Find("durability").GetComponent<GuiBar>();
      elementData.m_amount = elementData.m_go.transform.Find("amount").GetComponent<TMP_Text>();
      elementData.m_equiped = elementData.m_go.transform.Find("equiped").gameObject;
      elementData.m_queued = elementData.m_go.transform.Find("queued").gameObject;
      elementData.m_selection = elementData.m_go.transform.Find("selected").gameObject;

      TMP_Text bindingText = elementData.m_go.transform.Find("binding").GetComponent<TMP_Text>();
      bindingText.enabled = true;
      bindingText.text = HotkeyBarManager.HotkeyTexts[index];

      __instance.m_elements.Add(elementData);
    }

    foreach (HotkeyBar.ElementData element in __instance.m_elements) {
      element.m_used = false;
    }

    bool isGamepadActive = ZInput.IsGamepadActive();

    foreach (ItemDrop.ItemData itemData in __instance.m_items) {
      HotkeyBar.ElementData element = __instance.m_elements[itemData.m_gridPos.x - 5];
      element.m_used = true;
      element.m_icon.gameObject.SetActive(true);
      element.m_icon.sprite = itemData.GetIcon();
      element.m_durability.gameObject.SetActive(itemData.m_shared.m_useDurability);

      if (itemData.m_shared.m_useDurability) {
        if (itemData.m_durability <= 0.0) {
          element.m_durability.SetValue(1f);
          element.m_durability.SetColor(
              (double) Mathf.Sin(Time.time * 10f) > 0.0 ? Color.red : new Color(0.0f, 0.0f, 0.0f, 0.0f));
        } else {
          element.m_durability.SetValue(itemData.GetDurabilityPercentage());
          element.m_durability.ResetColor();
        }
      }

      element.m_equiped.SetActive(itemData.m_equipped);
      element.m_queued.SetActive(player.IsEquipActionQueued(itemData));

      if (itemData.m_shared.m_maxStackSize > 1) {
        element.m_amount.gameObject.SetActive(true);
        element.m_amount.text = itemData.m_stack.ToString() + "/" + itemData.m_shared.m_maxStackSize.ToString();
      } else {
        element.m_amount.gameObject.SetActive(false);
      }
    }

    for (int index = 0; index < __instance.m_elements.Count; ++index) {
      HotkeyBar.ElementData element = __instance.m_elements[index];
      element.m_selection.SetActive(isGamepadActive && index == __instance.m_selected);

      if (!element.m_used) {
        element.m_icon.gameObject.SetActive(false);
        element.m_durability.gameObject.SetActive(false);
        element.m_equiped.SetActive(false);
        element.m_queued.SetActive(false);
        element.m_amount.gameObject.SetActive(false);
      }
    }
    return false;
  }
}
