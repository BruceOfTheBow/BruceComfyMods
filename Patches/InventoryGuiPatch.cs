
using HarmonyLib;

using UnityEngine;
using UnityEngine.UI;

using static DumpsterFire.DumpsterFire;
using static DumpsterFire.PluginConfig;

namespace DumpsterFire.Patches {
  [HarmonyPatch(typeof(InventoryGui))]
  public class InventoryGuiPatch {
    private static RectTransform _dumpsterTransform;
    private static GameObject _buttonObject;
    private static RectTransform _buttonTransform;
    private static Canvas _buttonCanvas;
    private static GraphicRaycaster _buttonGraphicRaycaster;


    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.Show))]
    public static void ShowPostfix(InventoryGui __instance, Container container, int activeGroup) {
      if (!IsModEnabled.Value || __instance == null || _dumpsterTransform != null) {
        return;
      }

      Transform armorTransform = __instance.m_player.Find("Armor");
      if (armorTransform == null) {
        ZLog.Log("Armor transform not found.");
      }
      Transform dumpsterTransform = UnityEngine.Object.Instantiate(armorTransform, armorTransform.transform.parent);
      dumpsterTransform.name = "Dumpster";
      dumpsterTransform.SetAsFirstSibling();

      _dumpsterTransform = (RectTransform)dumpsterTransform.transform;
      _dumpsterTransform.anchoredPosition -= new Vector2(0, 80);

      Transform iconTransform = _dumpsterTransform.Find("armor_icon");
      UnityEngine.GameObject.Destroy(_dumpsterTransform.Find("ac_text").gameObject);

      iconTransform.gameObject.GetComponent<Image>().sprite = DumpsterSprite;
      RectTransform iconRectTransform = iconTransform.GetComponent<RectTransform>();
      iconRectTransform.anchoredPosition = Vector2.zero;
      iconRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75f);
      iconRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75f);

      _buttonObject = new GameObject("ButtonCanvas");
      _buttonTransform = _buttonObject.AddComponent<RectTransform>();
      _buttonTransform.transform.SetParent(dumpsterTransform.transform);
      _buttonTransform.anchoredPosition = Vector2.zero;
      _buttonTransform.sizeDelta = new Vector2(70, 74);
      _buttonCanvas = _buttonObject.AddComponent<Canvas>();
      _buttonGraphicRaycaster= _buttonObject.AddComponent<GraphicRaycaster>();

      Button button = _buttonObject.AddComponent<Button>();
      button.onClick.AddListener(new UnityEngine.Events.UnityAction(DestroyItem));
      Image buttonImage = _buttonObject.AddComponent<Image>();
      buttonImage.color = new Color(0, 0, 0, 0);

      InventoryAudioSource = __instance.gameObject.AddComponent<AudioSource>();

    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(InventoryGui.UpdateItemDrag))]
    public static void UpdateItemDragPostfix(InventoryGui __instance) {
      if (!IsModEnabled.Value || !DeleteKey.Value.IsDown()) {
        return;
      }
      DestroyItem();
    }
  }
}
