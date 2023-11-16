using System.Collections.Generic;

using TMPro;
using UnityEngine;

namespace HomeFurnishings {
  public class ComfortPanel {
    public GameObject Panel { get; private set; }

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

    private static int _labelSize = 28;
    private static int _titleSize = 48;
    private static int _maxComfortSearchRadius = 10;

    readonly List<TMP_Text> Labels = new();

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
    }

    private string FindNearestSimilarText(Piece piece) {
      Vector3 playerPosition = Player.m_localPlayer.transform.position;
      float shortestDistance = _maxComfortSearchRadius;

      foreach (Piece checkPiece in Piece.s_allComfortPieces) {
        if (piece.gameObject.layer == Piece.s_ghostLayer
              || piece.m_comfortGroup != checkPiece.m_comfortGroup
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

      _titleLabel.SetAnchorMin(new(0f, 0f))
          .SetAnchorMax(new(1, 1f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(0, parent.rect.height / 2 + parent.anchoredPosition.y - parent.rect.height / 6));

      _titleLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width);
      _titleLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height/3);

      _groupLabel.SetAnchorMin(new(0f, 0f))
          .SetAnchorMax(new(1, 1f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(-1 * parent.rect.width / 4, parent.rect.height / 2 + parent.anchoredPosition.y - parent.rect.height / 2));

      _groupLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width / 2);
      _groupLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);

      _valueLabel.SetAnchorMin(new(0f, 0f))
          .SetAnchorMax(new(1, 1f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(-1 * parent.rect.width / 4, parent.rect.height / 2 + parent.anchoredPosition.y - parent.rect.height * 4 / 6));

      _valueLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width/2);
      _valueLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);

      _nearestLabel.SetAnchorMin(new(0f, 0f))
          .SetAnchorMax(new(1, 1f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(-1 * parent.rect.width / 4, parent.rect.height / 2 - parent.rect.height * 5 / 6));

      _nearestLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width/2);
      _nearestLabel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);
    }

    private void ResizeValues() {
      RectTransform parent = Panel.RectTransform();
      _groupValue.SetAnchorMin(new(0f, 0f))
          .SetAnchorMax(new(1, 1f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(parent.rect.width / 4, parent.rect.height / 2 + parent.anchoredPosition.y - parent.rect.height / 2));

      _groupValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width / 2);
      _groupValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);

      _valueValue.SetAnchorMin(new(0f, 0f))
          .SetAnchorMax(new(1, 1f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(parent.rect.width / 4, parent.rect.height / 2 + parent.anchoredPosition.y - parent.rect.height * 4 / 6));

      _valueValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width / 2);
      _valueValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);

      _nearestValue.SetAnchorMin(new(0f, 0f))
          .SetAnchorMax(new(1, 1f))
          .SetPivot(new(0.5f, 0.5f))
          .SetPosition(new(parent.rect.width / 4, parent.rect.height / 2 - parent.rect.height * 5 / 6));

      _nearestValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.rect.width / 2);
      _nearestValue.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parent.rect.height / 6);
    }

    private void CreateTitleLabel() {
      _titleLabel = CreateLabel("comfortTitle", "Comfort Info", _titleSize, TextAlignmentOptions.Center);
    }

    private void CreateGroupLabel() {
      _groupLabel = CreateLabel("comfortGroup", "Comfort Group: ", _labelSize, TextAlignmentOptions.Left);
    }

    private void CreateValueLabel() {
      _valueLabel = CreateLabel("comfortValue", "Comfort Value: ", _labelSize, TextAlignmentOptions.Left);
    }

    private void CreateNearestLabel() {
      _nearestLabel = CreateLabel("nearestSimilar", "Nearest similar: ", _labelSize, TextAlignmentOptions.Left);
    }

    private void CreateGroupValue() {
      _groupValue = CreateLabel("groupValue", "", _labelSize, TextAlignmentOptions.Left);
      _groupValueText = _groupValue.GetComponent<TMP_Text>();
    }

    private void CreateValueValue() {
      _valueValue = CreateLabel("valueValue", "", _labelSize, TextAlignmentOptions.Left);
      _valueValueText = _valueValue.GetComponent<TMP_Text>();
    }

    private void CreateNearestValue() {
      _nearestValue = CreateLabel("nearestValue", "", _labelSize, TextAlignmentOptions.Left);
      _nearestValueText = _nearestValue.GetComponent<TMP_Text>();
    }

    private RectTransform CreateLabel(string objName, string text, int fontSize, TextAlignmentOptions alignment) {
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
