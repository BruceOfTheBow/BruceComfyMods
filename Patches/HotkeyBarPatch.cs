using System.Collections.Generic;

using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using static ComfyQuickSlots.PluginConfig;

namespace ComfyQuickSlots {
  [HarmonyPatch(typeof(HotkeyBar))]
  public static class HotkeyBarPatch {
    public static GameObject QuickSlotsHotkeyBar;
    public static int SelectedHotkeyBarIndex = -1;
    public static int[] hotkeysIndices = { 37, 38, 39 };
    public static string[] hotkeyTexts = { KeyCodeUtils.ToShortString(QuickSlot1.Value), KeyCodeUtils.ToShortString(QuickSlot2.Value), KeyCodeUtils.ToShortString(QuickSlot3.Value) };

    [HarmonyPrefix]
    [HarmonyPatch(nameof(HotkeyBar.UpdateIcons))]
    public static bool HotkeyBarPrefix(HotkeyBar __instance, Player player) {
      if (__instance.name != "QuickSlotsHotkeyBar") {
        return true;
      }

      foreach (var element in __instance.m_elements) {
        UnityEngine.GameObject.Destroy(element.m_go);
      }
      __instance.m_elements.Clear();

      if (player == null || player.IsDead() || !EnableQuickslots.Value) {
        return false;
      }

      hotkeyTexts = new string[] { KeyCodeUtils.ToShortString(QuickSlot1.Value), KeyCodeUtils.ToShortString(QuickSlot2.Value), KeyCodeUtils.ToShortString(QuickSlot3.Value) };
      __instance.m_items = new List<ItemDrop.ItemData>();

      for (int i = 5; i < ComfyQuickSlots.columns; i++) {
        if (player.GetInventory().GetItemAt(i, 4) != null) {
          __instance.m_items.Add(player.GetInventory().GetItemAt(i, 4));
        }
      }

      for (var index = 0; index < ComfyQuickSlots.quickSlotsCount; ++index) {
        var elementData = new HotkeyBar.ElementData() { m_go = Object.Instantiate(__instance.m_elementPrefab, __instance.transform) };
        elementData.m_go.transform.localPosition = new Vector3(index * __instance.m_elementSpace, 0.0f, 0.0f);
        elementData.m_icon = elementData.m_go.transform.transform.Find("icon").GetComponent<Image>();
        elementData.m_durability = elementData.m_go.transform.Find("durability").GetComponent<GuiBar>();
        elementData.m_amount = elementData.m_go.transform.Find("amount").GetComponent<Text>();
        elementData.m_equiped = elementData.m_go.transform.Find("equiped").gameObject;
        elementData.m_queued = elementData.m_go.transform.Find("queued").gameObject;
        elementData.m_selection = elementData.m_go.transform.Find("selected").gameObject;

        var bindingText = elementData.m_go.transform.Find("binding").GetComponent<Text>();
        bindingText.enabled = true;
        bindingText.horizontalOverflow = HorizontalWrapMode.Overflow;
        bindingText.text = hotkeyTexts[index];

        __instance.m_elements.Add(elementData);
      }

      foreach (var element in __instance.m_elements) {
        element.m_used = false;
      }

      var isGamepadActive = ZInput.IsGamepadActive();

      foreach (var itemData in __instance.m_items) {
        var element = __instance.m_elements[itemData.m_gridPos.x - 5];
        element.m_used = true;
        element.m_icon.gameObject.SetActive(true);
        element.m_icon.sprite = itemData.GetIcon();
        element.m_durability.gameObject.SetActive(itemData.m_shared.m_useDurability);
        if (itemData.m_shared.m_useDurability) {
          if (itemData.m_durability <= 0.0) {
            element.m_durability.SetValue(1f);
            element.m_durability.SetColor((double)Mathf.Sin(Time.time * 10f) > 0.0 ? Color.red : new Color(0.0f, 0.0f, 0.0f, 0.0f));
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

      for (var index = 0; index < __instance.m_elements.Count; ++index) {
        var element = __instance.m_elements[index];
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
}