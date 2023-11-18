using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace Hygge {
  public class ComfortPanel {
    public GameObject Panel { get; private set; }

    private Piece _currentPiece;

    private RectTransform _titleLabel;
    private RectTransform _groupLabel;
    private RectTransform _valueLabel;
    private RectTransform _nearestLabel;

    private RectTransform _groupValue;
    private RectTransform _valueValue;
    private RectTransform _nearestValue;

    private TMP_Text _groupValueText;
    private TMP_Text _valueValueText;
    private TMP_Text _nearestValueText;

    private static float _fontSize = 0f;
    private static float _labelSize = 24f;
    private static float _titleSize = 48f;

    private static int _maxComfortSearchRadius = 10;

    private static readonly int _comfortMin = 1;
    private static readonly float _alpha = 0.7f;

    private static readonly Dictionary<Piece.ComfortGroup, Color> _groupColors = new() {
      { Piece.ComfortGroup.Fire, new UnityEngine.Color(139/255f, 0, 0, _alpha)}, // Dark Red
      { Piece.ComfortGroup.Carpet, Color.blue},
      { Piece.ComfortGroup.Banner, new UnityEngine.Color(128/255f,0,128/255f, _alpha)}, // Purple
      { Piece.ComfortGroup.Table, Color.yellow},
      { Piece.ComfortGroup.Bed, new UnityEngine.Color(218/255f,165/255f,32/255f, _alpha)}, // Goldenrod
      { Piece.ComfortGroup.Chair, new UnityEngine.Color(169/255f,169/255f,169/255f, _alpha)}, // Dark Grey
      { Piece.ComfortGroup.None, Color.cyan}
    };

    private static readonly Dictionary<Piece.ComfortGroup, int> _groupMaxValue = new() {
      { Piece.ComfortGroup.Fire, 2},
      { Piece.ComfortGroup.Carpet, 1},
      { Piece.ComfortGroup.Banner, 1},
      { Piece.ComfortGroup.Table, 2},
      { Piece.ComfortGroup.Bed, 2},
      { Piece.ComfortGroup.Chair, 3},
      { Piece.ComfortGroup.None, 6 }
    };

    public static ComfortPanel CreateComfortPanel() {
      Transform selectedInfo = FindSelectedInfo();

      if (selectedInfo == null) {
        ZLog.LogWarning("Failed to find 'SelectedInfo' transform in build hud.");
        return null;
      }

      ComfortPanel comfortPanel = new(selectedInfo);
      return comfortPanel;
    }

    public ComfortPanel(Transform parentTransform) {
      CreateChildPanel(parentTransform);
    }

    void CreateChildPanel(Transform parentTransform) {
      Panel = new("ComfortPanel", typeof(RectTransform));
      Panel.transform.SetParent(parentTransform, false);

      FindFontSize();

      ResizePanel();

      CreateTitleLabel();
      CreateGroupLabel();
      CreateValueLabel();
      CreateNearestLabel();

      ResizeLabels();

      CreateGroupValue();
      CreateValueValue();
      CreateNearestValue();

      ResizeValues();

      CanvasGroup canvasGroup = Panel.AddComponent<CanvasGroup>();
      canvasGroup.blocksRaycasts = true;
    }

    public void Update(Piece piece) {
      _groupValueText.text = piece.m_comfortGroup.ToString();
      _valueValueText.text = piece.m_comfort.ToString();
      _nearestValueText.text = FindNearestSimilarText(piece);

      UpdateColor(piece);
    }

    private string FindNearestSimilarText(Piece piece) {
      Vector3 playerPosition = Player.m_localPlayer.transform.position;
      float shortestDistance = _maxComfortSearchRadius;

      foreach (Piece checkPiece in Piece.s_allComfortPieces) {
        if (piece.m_comfortGroup != checkPiece.m_comfortGroup
              || ReferenceEquals(checkPiece.gameObject, Player.m_localPlayer.m_placementGhost)) {

          continue;
        }

        if (checkPiece.m_comfortGroup == Piece.ComfortGroup.None && checkPiece.name.Replace("(Clone)", "") != piece.name) {
          continue;
        }

        shortestDistance = Mathf.Min(Vector3.Distance(playerPosition, checkPiece.transform.position), shortestDistance);
      }

      if (shortestDistance == _maxComfortSearchRadius) {
        return "None nearby";
      }

      return shortestDistance.ToString("0.0") + "m";
    }

    private void ResizePanel() {
      RectTransform bkg2 = FindBkg();

      if (bkg2 == null) {
        ZLog.LogWarning("Unable to resize Hygge comfort panel. bkg2 UI element not found.");
        return;
      }

      Panel.RectTransform()
        .SetAnchorMin(new(0f, 0f))
        .SetAnchorMax(new(1, 1f))
        .SetPivot(new(0.5f, 0.5f))
        .SetPosition(new(bkg2.anchoredPosition.x + bkg2.rect.width / 2 + bkg2.rect.width / 4 + 5f, bkg2.anchoredPosition.y))
        .SetImage(new Color(0, 0, 0, 0.5f));

      Panel.RectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bkg2.rect.width / 2);
      Panel.RectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bkg2.rect.height);

      Panel.SetActive(false);
    }

    private void ResizeLabels() {
      RectTransform parent = Panel.RectTransform();

      _titleLabel.SetAnchorMin(new(0.5f, 0.5f))
          .SetAnchorMax(new(0.5f, 0.5f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(0, parent.rect.height / 2 + parent.anchoredPosition.y - parent.rect.height / 6));

      _titleLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width);
      _titleLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height/3);

      _groupLabel.SetAnchorMin(new(0.5f, 0.5f))
          .SetAnchorMax(new(0.5f, 0.5f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(-1 * parent.rect.width / 4, parent.rect.height / 2 + parent.anchoredPosition.y - parent.rect.height / 2));

      _groupLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width / 2);
      _groupLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);

      _valueLabel.SetAnchorMin(new(0.5f, 0.5f))
          .SetAnchorMax(new(0.5f, 0.5f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(-1 * parent.rect.width / 4, parent.rect.height / 2 + parent.anchoredPosition.y - parent.rect.height * 4 / 6));

      _valueLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width/2);
      _valueLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);

      _nearestLabel.SetAnchorMin(new(0.5f, 0.5f))
          .SetAnchorMax(new(0.5f, 0.5f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(-1 * parent.rect.width / 4, parent.rect.height / 2 - parent.rect.height * 5 / 6));

      _nearestLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width/2);
      _nearestLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);
    }

    private void ResizeValues() {
      RectTransform parent = Panel.RectTransform();
      _groupValue.SetAnchorMin(new(0.5f, 0.5f))
          .SetAnchorMax(new(0.5f, 0.5f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(parent.rect.width / 4, parent.rect.height / 2 + parent.anchoredPosition.y - parent.rect.height / 2));

      _groupValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width / 2);
      _groupValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);

      _valueValue.SetAnchorMin(new(0.5f, 0.5f))
          .SetAnchorMax(new(0.5f, 0.5f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(parent.rect.width / 4, parent.rect.height / 2 + parent.anchoredPosition.y - parent.rect.height * 4 / 6));

      _valueValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width / 2);
      _valueValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);

      _nearestValue.SetAnchorMin(new(0.5f, 0.5f))
          .SetAnchorMax(new(0.5f, 0.5f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(parent.rect.width / 4, parent.rect.height / 2 - parent.rect.height * 5 / 6));

      _nearestValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width / 2);
      _nearestValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);
    }

    private void UpdateColor(Piece piece) {
      _groupValueText.color = GetColorByGroup(piece.m_comfortGroup);
      _valueValueText.color = GetColorByPercentMax(piece);
    }

    private Color GetColorByGroup(Piece.ComfortGroup comfortGroup) {
      if (_groupColors.TryGetValue(comfortGroup, out Color color)) {
        return color;
      }

      return Color.white;
    }

    private Color GetColorByPercentMax(Piece piece) {
      if (!_groupMaxValue.TryGetValue(piece.m_comfortGroup, out int max)) {
        return Color.white;
      }

      if (piece.m_comfortGroup == Piece.ComfortGroup.None) {
        return Color.white;
      }

      if (piece.m_comfort == max) {
        return Color.green;
      }

      if (piece.m_comfort == _comfortMin) {
        return Color.red;
      }
      
      // Orange
      return new Color(255, 140, 0, _alpha);

    }

    private void CreateTitleLabel() {
      _titleLabel = CreateLabel("comfortTitle", "Comfort Info", GetTitleFontSize(), TextAlignmentOptions.Center);
    }

    private void CreateGroupLabel() {
      _groupLabel = CreateLabel("comfortGroup", "Comfort Group: ", GetLabelFontSize(), TextAlignmentOptions.Left);
    }

    private void CreateValueLabel() {
      _valueLabel = CreateLabel("comfortValue", "Comfort Value: ", GetLabelFontSize(), TextAlignmentOptions.Left);
    }

    private void CreateNearestLabel() {
      _nearestLabel = CreateLabel("nearestSimilar", "Nearest similar: ", GetLabelFontSize(), TextAlignmentOptions.Left);
    }

    private void CreateGroupValue() {
      _groupValue = CreateLabel("groupValue", "", GetLabelFontSize(), TextAlignmentOptions.Left);
      _groupValueText = _groupValue.GetComponent<TMP_Text>();
    }

    private void CreateValueValue() {
      _valueValue = CreateLabel("valueValue", "", GetLabelFontSize(), TextAlignmentOptions.Left);
      _valueValueText = _valueValue.GetComponent<TMP_Text>();
    }

    private void CreateNearestValue() {
      _nearestValue = CreateLabel("nearestValue", "", GetLabelFontSize(), TextAlignmentOptions.Left);
      _nearestValueText = _nearestValue.GetComponent<TMP_Text>();
    }

    private RectTransform CreateLabel(string objName, string text, float fontSize, TextAlignmentOptions alignment) {
      GameObject gameObj = new(objName, typeof(RectTransform));
      gameObj.transform.SetParent(Panel.transform);
     
      TMP_Text tmpText = gameObj.AddComponent<TextMeshProUGUI>();
      tmpText.text = text;
      tmpText.fontSize = fontSize;
      tmpText.alignment = alignment;

      return (RectTransform)gameObj.transform;
    }

    private static Transform FindSelectedInfo() {
      return Hud.instance.m_buildHud.transform.Find("SelectedInfo");
    }

    private static void FindFontSize() {
      _titleSize = 0.0124f * Screen.currentResolution.width;
      _labelSize = 0.0062f * Screen.currentResolution.width;
    }

    private static float GetLabelFontSize() {
      return _labelSize;
    }

    private static float GetTitleFontSize() {
      return _titleSize;
    }

    private static RectTransform FindBkg() {
      Transform selectedInfo = Hud.instance.m_buildHud.transform.Find("SelectedInfo");
      if (selectedInfo == null) {
        ZLog.LogWarning("SelectedInfo transform not found building comfort panel.");
        return null;
      }

      Transform bkg2 = selectedInfo.Find("Bkg2");

      if (bkg2 == null) {
        ZLog.LogWarning("Bkg2 not found building comfort panel.");
        return null;
      }

      return (RectTransform)bkg2;
    }
  }
}
