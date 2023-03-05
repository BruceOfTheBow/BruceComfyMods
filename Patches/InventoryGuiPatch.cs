using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using static ComfyBatchDeposit.ComfyBatchDeposit;
using static ComfyBatchDeposit.PluginConfig;

namespace ComfyBatchDeposit.Patches {
  [HarmonyPatch(typeof(InventoryGui))]
  public class InventoryGuiPatch {
    private static RectTransform _sortButton;
    private static RectTransform _dumpButton;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.Show))]
    public static void ShowPostfix(InventoryGui __instance, Container container) {
      // ResizeTakeAllButton(instance.m_takeAllButton.transform.parent);
      if (!__instance.IsContainerOpen()) {
        return;
      }
      if (_sortButton == null) {
        _sortButton = PrepareButton(__instance, "sort", "S");
        RelocateButtons(_sortButton, 0.3f);
        _sortButton.GetComponent<Button>().onClick.AddListener(() => {
          if (Player.m_localPlayer.IsTeleporting() || !(bool)__instance.m_containerGrid) return;

          SortInventory(__instance.m_containerGrid.GetInventory(), false);
        });
      }
     
     
      if (_dumpButton == null) {
        _dumpButton = PrepareButton(__instance, "dump", "D");
        RelocateButtons(_dumpButton, 1.5f);
        _dumpButton.GetComponent<Button>().onClick.AddListener(() => {
          if (Player.m_localPlayer.IsTeleporting() || !(bool)__instance.m_containerGrid) return;

          DumpItems(__instance.m_playerGrid.GetInventory(), __instance.m_containerGrid.GetInventory(), !Input.GetKey(KeyCode.LeftShift));

          if (SortOnDump.Value) {
            SortInventory(__instance.m_containerGrid.GetInventory(), false);
          }

        });
      }
    }

    private static RectTransform PrepareButton(InventoryGui instance, string name, string text) {
      RectTransform targetTransform = (RectTransform)instance.transform.parent.Find(name);
      if (targetTransform != null) {
        return targetTransform;
      }

      Transform buttonTransform = instance.m_takeAllButton.transform;
      Transform additionalTransform = Object.Instantiate(buttonTransform, buttonTransform.transform.parent);
      additionalTransform.name = name;
      var resultTransform = additionalTransform.transform;
      // resultTransform.SetAsFirstSibling();

      targetTransform = (RectTransform)resultTransform.transform;
      targetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 45f);
      targetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);

      RectTransform textTransform = (RectTransform)targetTransform.transform.Find("Text");
      textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 45f);
      textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30f);

      var component = textTransform.GetComponent<Text>();
      component.text = text;
      component.resizeTextForBestFit = true;
      return targetTransform;
    }

    private static void RelocateButtons(RectTransform transform, float vertical) {
      if (!(bool)transform) return;

      transform.pivot = new Vector2(-10f, vertical);
    }
  }
}
