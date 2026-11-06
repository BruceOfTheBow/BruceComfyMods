namespace ComfyBatchDeposit;

using UnityEngine;
using UnityEngine.UI;

using static ComfyBatchDeposit;
using static PluginConfig;

public sealed class ButtonManager {
  private static RectTransform _sortButton;
  private static RectTransform _dumpButton;

  public static bool IsInitialized() {
    return IsSortButtonCreated() && IsDumpButtonCreated();
  }

  public static void Initialize(InventoryGui inventoryGui) {
    if (!IsSortButtonCreated()) {
      CreateSortButton(inventoryGui);
    }

    if (!IsDumpButtonCreated()) {
      CreateDumpButton(inventoryGui);
    }

    RemovePlaceStacksButton(inventoryGui);
  }

  public static bool IsSortButtonCreated() {
    return _sortButton != null;
  }

  public static bool IsDumpButtonCreated() {
    return _dumpButton != null;
  }

  public static void CreateSortButton(InventoryGui inventoryGui) {
    _sortButton = PrepareButton(inventoryGui, "sort", "Sort");
    Translate(_sortButton, -6.25f, 0.55f);

    _sortButton.GetComponent<Button>().onClick.AddListener(() => {
      if (Player.m_localPlayer.IsTeleporting() || !(bool) inventoryGui.m_containerGrid)
        return;

      SortInventory(inventoryGui.m_containerGrid.GetInventory(), false);
    });
  }

  public static void RemovePlaceStacksButton(InventoryGui inventoryGui) {
    if (inventoryGui.m_stackAllButton == null) {
      return;
    }

    Object.Destroy(inventoryGui.m_stackAllButton.gameObject);
  }

  public static void CreateDumpButton(InventoryGui inventoryGui) {
    _dumpButton = PrepareButton(inventoryGui, "dump", "Dump");
    Translate(_dumpButton, -7.25f, 0.55f);
    _dumpButton.GetComponent<Button>().onClick.AddListener(() => {
      if (Player.m_localPlayer.IsTeleporting() || !(bool) inventoryGui.m_containerGrid)
        return;

      DumpItems(inventoryGui.m_playerGrid.GetInventory(), inventoryGui.m_containerGrid.GetInventory(), !Input.GetKey(KeyCode.LeftShift));

      if (SortOnDump.Value) {
        SortInventory(inventoryGui.m_containerGrid.GetInventory(), false);
      }

    });
  }

  private static RectTransform PrepareButton(InventoryGui instance, string name, string text) {
    RectTransform targetTransform = (RectTransform) instance.transform.parent.Find(name);
    if (targetTransform != null) {
      return targetTransform;
    }

    Transform buttonTransform = instance.m_takeAllButton.transform;
    Transform additionalTransform = Object.Instantiate(buttonTransform, buttonTransform.transform.parent);
    additionalTransform.name = name;
    var resultTransform = additionalTransform.transform;
    // resultTransform.SetAsFirstSibling();

    targetTransform = (RectTransform) resultTransform.transform;
    targetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 60f);
    targetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40f);

    RectTransform textTransform = (RectTransform) targetTransform.transform.Find("Text");
    textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 60f);
    textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40f);

    TMPro.TMP_Text component = textTransform.GetComponent<TMPro.TMP_Text>();
    component.text = text;
    component.m_enableAutoSizing = true;
    return targetTransform;
  }

  private static void Translate(RectTransform transform, float horizontal, float vertical) {
    if (transform == null)
      return;

    transform.pivot = new Vector2(horizontal, vertical);
  }
}
