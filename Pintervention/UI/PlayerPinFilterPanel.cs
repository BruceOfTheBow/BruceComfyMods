using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Pintervention {
  public class PlayerPinFilterPanel {
    public GameObject Panel { get; private set; }

    public ValueCell PinNameFilter { get; private set; }

    public GameObject Viewport { get; private set; }
    public GameObject Content { get; private set; }

    public ScrollRect ScrollRect { get; private set; }
    public LabelCell PinStats { get; private set; }

    public PanelDragger PanelDragger { get; private set; }
    public PanelResizer PanelResizer { get; private set; }

    readonly PointerState _pointerState;

    public PlayerPinFilterPanel(Transform parentTransform) {
      Panel = CreateChildPanel(parentTransform);

      PanelDragger = CreateChildPanelDragger(Panel).AddComponent<PanelDragger>();
      PanelDragger.TargetRectTransform = Panel.RectTransform();

      PanelResizer = CreateChildPanelResizer(Panel).AddComponent<PanelResizer>();
      PanelResizer.TargetRectTransform = Panel.RectTransform();

      PinNameFilter = new(Panel.transform);
      PinNameFilter.Cell.LayoutElement().SetFlexible(width: 1f).SetPreferred(height: 30f);
      //PinNameFilter.Cell.GetComponent<HorizontalLayoutGroup>().SetPadding(left: 8, right: 8, top: 5, bottom: 5);

      Viewport = CreateChildViewport(Panel.transform);
      Content = CreateChildContent(Viewport.transform);
      ScrollRect = CreateChildScrollRect(Panel, Viewport, Content);

      PinStats = new(Panel.transform);
      PinStats.Cell.GetComponent<HorizontalLayoutGroup>().SetPadding(left: 8, right: 8, top: 5, bottom: 5);
      PinStats.Cell.Image().SetColor(new(0.5f, 0.5f, 0.5f, 0.1f));
      PinStats.Cell.AddComponent<Outline>().SetEffectDistance(new(2f, -2f));

      _pointerState = Panel.AddComponent<PointerState>();

      UpdatePanel();
    }

    public bool HasFocus() {
      return Panel && Panel.activeInHierarchy && _pointerState.IsPointerHovered;
    }

    public void UpdatePinCounts() {
      if (Panel == null) {
        return;
      }

      foreach (PlayerListRow row in _rowCache) {
        row.UpdateCount();
      }
    }

    public void UpdatePanel() {
      RefreshPinListRows();
      PinStats.Label.SetText($"{PinOwnerManager.ForeignPinOwners.Count} players with pins.");
    }

    readonly List<PlayerListRow> _rowCache = new();
    int _visibleRows = 0;
    float _rowPreferredHeight = 0f;
    LayoutElement _bufferBlock;
    bool _isRefreshing = false;
    int _previousRowIndex = -1;

    void BuildPinListRows() {

    }

    void RefreshPinListRows() {
      //_isRefreshing = true;

      ScrollRect.onValueChanged.RemoveAllListeners();
      Content.RectTransform().SetPosition(Vector2.zero);
      ScrollRect.SetVerticalScrollPosition(1f);
      _previousRowIndex = -1;

      _rowCache.Clear();

      foreach (GameObject child in Content.Children()) {
        UnityEngine.Object.Destroy(child);
      }

      GameObject block = new("ppl.Block", typeof(RectTransform));
      block.SetParent(Content.transform);

      _bufferBlock = block.AddComponent<LayoutElement>();
      _bufferBlock.SetPreferred(height: 0f);

      PlayerListRow row = new(Content.transform);

      LayoutRebuilder.ForceRebuildLayoutImmediate(Panel.RectTransform());
      _rowPreferredHeight = LayoutUtility.GetPreferredHeight(row.Row.RectTransform());

      _visibleRows = Mathf.CeilToInt(Viewport.RectTransform().sizeDelta.y / _rowPreferredHeight);

      UnityEngine.Object.Destroy(row.Row);

      Content.RectTransform().SetSizeDelta(
          new(Viewport.RectTransform().sizeDelta.x, _rowPreferredHeight * PinOwnerManager.ForeignPinOwners.Count));

      for (int i = 0; i < PinOwnerManager.ForeignPinOwners.Count; i++) {
        row = new(Content.transform);
        row.SetRowContent(PinOwnerManager.GetForeignPinOwnerAtIndex(i));
        _rowCache.Add(row);
      }

      _previousRowIndex = -1;
      ScrollRect.SetVerticalScrollPosition(1f);

      //ScrollRect.onValueChanged.AddListener(OnVerticalScroll);
      //_isRefreshing = false;
    }

    //void OnVerticalScroll(Vector2 scroll) {
    //  if (_isRefreshing || ForeignPinManager.ForeignPinOwners.Count == 0 || _rowCache.Count == 0) {
    //    return;
    //  }

    //  float scrolledY = Content.RectTransform().anchoredPosition.y;

    //  int rowIndex =
    //      Mathf.Clamp(Mathf.CeilToInt(scrolledY / _rowPreferredHeight), 0, ForeignPinManager.ForeignPinOwners.Count - _rowCache.Count);

    //  if (rowIndex == _previousRowIndex) {
    //    return;
    //  }

    //  if (rowIndex > _previousRowIndex) {
    //    PlayerListRow row = _rowCache[0];
    //    _rowCache.RemoveAt(0);
    //    row.Row.RectTransform().SetAsLastSibling();

    //    int index = Mathf.Clamp(rowIndex + _rowCache.Count, 0, ForeignPinManager.ForeignPinOwners.Count - 1);
    //    row.SetRowContent(ForeignPinManager.GetForeignPinOwnerAtIndex(index));
    //    _rowCache.Add(row);
    //  } else {
    //    PlayerListRow row = _rowCache[_rowCache.Count - 1];
    //    _rowCache.RemoveAt(_rowCache.Count - 1);
    //    row.Row.RectTransform().SetSiblingIndex(1);
    //    row.SetRowContent(ForeignPinManager.GetForeignPinOwnerAtIndex(rowIndex));
    //    _rowCache.Insert(0, row);
    //  }

    //  _bufferBlock.SetPreferred(height: rowIndex * _rowPreferredHeight);
    //  _previousRowIndex = rowIndex;
    //}

    GameObject CreateChildPanel(Transform parentTransform) {
      GameObject panel = new("PlayerPinList.Panel", typeof(RectTransform));
      panel.SetParent(parentTransform);

      panel.AddComponent<VerticalLayoutGroup>()
          .SetChildControl(width: true, height: true)
          .SetChildForceExpand(width: false, height: false)
          .SetPadding(left: 8, right: 8, top: 8, bottom: 8)
          .SetSpacing(8);

      panel.AddComponent<Image>()
          .SetType(Image.Type.Sliced)
          .SetSprite(UIBuilder.CreateSuperellipse(400, 400, 15))
          .SetColor(new(0f, 0f, 0f, 0.9f));

      return panel;
    }

    GameObject CreateChildViewport(Transform parentTransform) {
      GameObject viewport = new("PlayerPinList.Viewport", typeof(RectTransform));
      viewport.SetParent(parentTransform);

      viewport.AddComponent<RectMask2D>();

      viewport.AddComponent<LayoutElement>()
          .SetFlexible(width: 1f, height: 1f);

      viewport.AddComponent<Image>()
          .SetType(Image.Type.Sliced)
          .SetSprite(UIBuilder.CreateRoundedCornerSprite(128, 128, 8))
          .SetColor(new(0.5f, 0.5f, 0.5f, 0.1f));

      viewport.AddComponent<Outline>()
          .SetEffectDistance(new(2f, -2f));

      return viewport;
    }

    GameObject CreateChildContent(Transform parentTransform) {
      GameObject content = new("PlayerPinList.Content", typeof(RectTransform));
      content.SetParent(parentTransform);

      content.RectTransform()
          .SetAnchorMin(Vector2.up)
          .SetAnchorMax(Vector2.up)
          .SetPivot(Vector2.up);

      content.AddComponent<VerticalLayoutGroup>()
          .SetChildControl(width: true, height: true)
          .SetChildForceExpand(width: false, height: false)
          .SetSpacing(0f);

      content.AddComponent<Image>()
          .SetColor(Color.clear)
          .SetRaycastTarget(true);

      return content;
    }

    ScrollRect CreateChildScrollRect(GameObject panel, GameObject viewport, GameObject content) {
      return panel.AddComponent<ScrollRect>()
          .SetViewport(viewport.RectTransform())
          .SetContent(content.RectTransform())
          .SetHorizontal(false)
          .SetVertical(true)
          .SetScrollSensitivity(30f);
    }

    GameObject CreateChildPanelDragger(GameObject panel) {
      GameObject dragger = new("ppl.Dragger", typeof(RectTransform));
      dragger.SetParent(panel.transform);

      dragger.AddComponent<LayoutElement>()
          .SetIgnoreLayout(true);

      dragger.RectTransform()
          .SetAnchorMin(Vector2.zero)
          .SetAnchorMax(Vector2.one)
          .SetPivot(new(0.5f, 0.5f))
          .SetSizeDelta(Vector2.zero);

      dragger.AddComponent<Image>()
          .SetColor(Color.clear);

      return dragger;
    }

    GameObject CreateChildPanelResizer(GameObject panel) {
      GameObject resizer = new("ppl.Resizer", typeof(RectTransform));
      resizer.SetParent(panel.transform);

      resizer.AddComponent<LayoutElement>()
          .SetIgnoreLayout(true);

      resizer.RectTransform()
          .SetAnchorMin(Vector2.right)
          .SetAnchorMax(Vector2.right)
          .SetPivot(Vector2.right)
          .SetSizeDelta(new(40f, 40f))
          .SetPosition(new(15f, -15f));

      resizer.AddComponent<Image>()
          .SetType(Image.Type.Sliced)
          .SetSprite(UIBuilder.CreateRoundedCornerSprite(128, 128, 12))
          .SetColor(new(0.565f, 0.792f, 0.976f, 0.849f));

      resizer.AddComponent<Shadow>()
          .SetEffectDistance(new(2f, -2f));

      resizer.AddComponent<CanvasGroup>()
          .SetAlpha(0f);

      TMP_Text icon = UIBuilder.CreateTMPLabel(resizer.transform);
      icon.SetName("Resizer.Icon");

      icon.gameObject.AddComponent<LayoutElement>()
          .SetIgnoreLayout(true);

      icon.gameObject.RectTransform()
          .SetAnchorMin(Vector2.zero)
          .SetAnchorMax(Vector2.one)
          .SetPivot(new(0.5f, 0.5f))
          .SetSizeDelta(Vector2.zero);

      icon.alignment = TextAlignmentOptions.Center;
      icon.fontSize = 24f;
      icon.text = "\u21C6\u21C5";

      return resizer;
    }
  }
}
