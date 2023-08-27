using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using static ComfyQuickSlots.ComfyQuickSlots;
using static ComfyQuickSlots.PluginConfig;

namespace ComfyQuickSlots.Patches {
  [HarmonyPatch(typeof(InventoryGrid))]
  public static class InventoryGridPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGrid.UpdateGui))]
    public static void UpdateGuiPostfix(ref InventoryGrid __instance, Player player, ItemDrop.ItemData dragItem) {
      if (__instance.name == "PlayerGrid") {

        float addedRows = rows - 4;
        float offset = -35 * addedRows;
        RectTransform gridBkg = GetOrCreateBackground(__instance, "ExtInvGrid");
        gridBkg.anchoredPosition = new Vector2(0f, offset);
        gridBkg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 590f);
        gridBkg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 300f + 75 * addedRows);

        //Add Quick slots and equipment overlays
        //for(int i = 36; i < rows*columns - 1; i++) {
        Text bindingTextHead = __instance.m_elements[32].m_go.transform.Find("binding").GetComponent<Text>();
        bindingTextHead.text = "Head";
        bindingTextHead.enabled = true;
        bindingTextHead.fontSize = 12;
        bindingTextHead.horizontalOverflow = HorizontalWrapMode.Overflow;
        bindingTextHead.alignment = TextAnchor.UpperLeft;
        Text bindingTextChest = __instance.m_elements[33].m_go.transform.Find("binding").GetComponent<Text>();
        bindingTextChest.text = "Chest";
        bindingTextChest.enabled = true;
        bindingTextChest.fontSize = 12;
        bindingTextChest.alignment = TextAnchor.UpperLeft;
        bindingTextChest.horizontalOverflow = HorizontalWrapMode.Overflow;
        Text bindingTextLegs = __instance.m_elements[34].m_go.transform.Find("binding").GetComponent<Text>();
        bindingTextLegs.text = "Legs";
        bindingTextLegs.enabled = true;
        bindingTextLegs.horizontalOverflow = HorizontalWrapMode.Overflow;
        bindingTextLegs.fontSize = 12;
        bindingTextLegs.alignment = TextAnchor.UpperLeft;
        Text bindingTextCape = __instance.m_elements[35].m_go.transform.Find("binding").GetComponent<Text>();
        bindingTextCape.text = "Cape";
        bindingTextCape.enabled = true;
        bindingTextCape.fontSize = 12;
        bindingTextLegs.alignment = TextAnchor.UpperLeft;
        bindingTextCape.horizontalOverflow = HorizontalWrapMode.Overflow;
        Text bindingTextUtility = __instance.m_elements[36].m_go.transform.Find("binding").GetComponent<Text>();
        bindingTextUtility.text = "Util";
        bindingTextUtility.enabled = true;
        bindingTextUtility.fontSize = 12;
        bindingTextUtility.alignment = TextAnchor.UpperLeft;
        bindingTextUtility.horizontalOverflow = HorizontalWrapMode.Overflow;

        Text bindingText1 = __instance.m_elements[37].m_go.transform.Find("binding").GetComponent<Text>();
        Text bindingText2 = __instance.m_elements[38].m_go.transform.Find("binding").GetComponent<Text>();
        Text bindingText3 = __instance.m_elements[39].m_go.transform.Find("binding").GetComponent<Text>();

        if (!EnableQuickslots.Value) {
          bindingText1.enabled = false;
          bindingText2.enabled = false;
          bindingText3.enabled = false;
          return;
        }

       
        bindingText1.text = KeyCodeUtils.ToShortString(QuickSlot1.Value);
        bindingText1.enabled = true;
        bindingText1.horizontalOverflow = HorizontalWrapMode.Overflow;

        bindingText2.text = KeyCodeUtils.ToShortString(QuickSlot2.Value);
        bindingText2.enabled = true;
        bindingText2.horizontalOverflow = HorizontalWrapMode.Overflow;
       
        bindingText3.text = KeyCodeUtils.ToShortString(QuickSlot3.Value);
        bindingText3.enabled = true;
        bindingText3.horizontalOverflow = HorizontalWrapMode.Overflow;
      }
    }

    private static RectTransform GetOrCreateBackground(InventoryGrid __instance, string name) {
      var existingBkg = __instance.transform.parent.Find(name);
      if (existingBkg == null) {
        var bkg = __instance.transform.parent.Find("Bkg").gameObject;
        var background = GameObject.Instantiate(bkg, bkg.transform.parent);
        background.name = name;
        background.transform.SetSiblingIndex(bkg.transform.GetSiblingIndex() + 1);
        existingBkg = background.transform;
      }

      return existingBkg as RectTransform;
    }
  }
}
