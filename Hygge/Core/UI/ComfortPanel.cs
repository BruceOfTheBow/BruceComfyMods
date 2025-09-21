namespace Hygge;

using ComfyLib;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public sealed class ComfortPanel {
  public GameObject Panel { get; private set; }
  public RectTransform RectTransform { get; private set; }

  public TextMeshProUGUI Title { get; private set; }
  public LabelValueRow ComfortGroupRow { get; private set; }
  public LabelValueRow ComfortValueRow { get; private set; }
  public LabelValueRow NearestSimilarRow { get; private set; }

  public const int MaxComfortSearchRadius = 10;

  public static ComfortPanel CreateComfortPanel() {
    Transform selectedInfo = Hud.instance.m_buildHud.transform.Find("SelectedInfo");

    if (!selectedInfo) {
      Hygge.LogWarning("Failed to find 'SelectedInfo' transform in build hud.");
      return null;
    }

    ComfortPanel comfortPanel = new(selectedInfo);
    return comfortPanel;
  }

  public ComfortPanel(Transform parentTransform) {
    Panel = CreatePanel(parentTransform);
    RectTransform = Panel.GetComponent<RectTransform>();

    Title = CreateTitle(RectTransform);
    ComfortGroupRow = CreateComfortGroupRow(RectTransform);
    ComfortValueRow = CreateComfortValueRow(RectTransform);
    NearestSimilarRow = CreateNearestSimilarRow(RectTransform);
  }

  static GameObject CreatePanel(Transform parentTransform) {
    GameObject panel = new("ComfortPanel", typeof(RectTransform));
    panel.transform.SetParent(parentTransform, worldPositionStays: false);

    panel.AddComponent<Image>()
        .SetSprite(UIResources.GetSprite("Background"))
        .SetType(Image.Type.Sliced)
        .SetColor(new Color(0f, 0f, 0f, 0.5f));

    panel.GetComponent<RectTransform>()
        .SetAnchorMin(Vector2.right)
        .SetAnchorMax(Vector2.one)
        .SetPivot(Vector2.zero)
        .SetPosition(new(2.5f, 0f))
        .SetSizeDelta(new(175f, 0f));


    panel.AddComponent<CanvasGroup>()
        .SetBlocksRaycasts(false)
        .SetAlpha(1f);

    return panel;
  }

  static TextMeshProUGUI CreateTitle(Transform parentTransform) {
    TextMeshProUGUI title = UIBuilder.CreateTMPHeaderLabel(parentTransform);
    title.name = "Title";

    title
        .SetAlignment(TextAlignmentOptions.Top)
        .SetFontSize(24f)
        .SetText("Comfort");

    title.rectTransform
        .SetAnchorMin(Vector2.up)
        .SetAnchorMax(Vector2.one)
        .SetPivot(new(0.5f, 1f))
        .SetPosition(new(0f, -2.5f))
        .SetSizeDelta(new(-20f, 40f));

    return title;
  }

  static LabelValueRow CreateComfortGroupRow(Transform parentTransform) {
    LabelValueRow row = new(parentTransform);
    row.Container.name = "ComfortGroup";

    row.RectTransform.SetPosition(new(0f, -40f));
    row.Label.SetText("Group");

    return row;
  }

  static LabelValueRow CreateComfortValueRow(Transform parentTransform) {
    LabelValueRow row = new(parentTransform);
    row.Container.name = "ComfortValue";

    row.RectTransform.SetPosition(new(0f, -70f));
    row.Label.SetText("Value");

    return row;
  }

  static LabelValueRow CreateNearestSimilarRow(Transform parentTransform) {
    LabelValueRow row = new(parentTransform);
    row.Container.name = "NearestSimilar";

    row.RectTransform.SetPosition(new(0f, -100f));
    row.Label.SetText("Nearest");

    return row;
  }

  public void Update(Piece piece) {
    ComfortGroupRow.Value
        .SetColor(GetColorForComfortGroup(piece.m_comfortGroup))
        .SetText($"{piece.m_comfortGroup:F}");

    ComfortValueRow.Value
        .SetColor(GetColorForComfortValue(piece.m_comfortGroup, piece.m_comfort))
        .SetText($"{piece.m_comfort}");

    if (TryGetNearestSimilarPiece(piece, out Piece _, out float nearestDistance)) {
      NearestSimilarRow.Value.SetText($"{nearestDistance:F1}m");
    } else {
      NearestSimilarRow.Value.SetText($"None");
    }
  }

  bool TryGetNearestSimilarPiece(Piece piece, out Piece nearestSimilarPiece, out float nearestDistance) {
    Vector3 playerPosition = Player.m_localPlayer.transform.position;
    GameObject placementGhost = Player.m_localPlayer.m_placementGhost;
    string pieceName = piece.m_name;
    Piece.ComfortGroup comfortGroup = piece.m_comfortGroup;

    nearestSimilarPiece = default;
    nearestDistance = MaxComfortSearchRadius;

    foreach (Piece checkPiece in Piece.s_allComfortPieces) {
      if (checkPiece.m_comfortGroup != comfortGroup) {
        continue;
      }

      if (checkPiece.gameObject == placementGhost) {
        continue;
      }

      if (checkPiece.m_comfortGroup == Piece.ComfortGroup.None && checkPiece.m_name != pieceName) {
        continue;
      }

      float distance = Vector3.Distance(playerPosition, checkPiece.transform.position);

      if (distance < nearestDistance) {
        nearestDistance = distance;
        nearestSimilarPiece = checkPiece;
      }
    }

    return nearestSimilarPiece;
  }

  public static Color GetColorForComfortGroup(Piece.ComfortGroup comfortGroup) {
    switch (comfortGroup) {
      case Piece.ComfortGroup.Fire:
        return new Color(0.545f, 0f, 0f); // Dark Red

      case Piece.ComfortGroup.Carpet:
        return Color.blue; // Blue

      case Piece.ComfortGroup.Banner:
        return new Color(0.5f, 0f, 0.5f); // Purple

      case Piece.ComfortGroup.Table:
        return Color.yellow; // Yellow

      case Piece.ComfortGroup.Bed:
        return new Color(0.855f, 0.647f, 0.125f); // Goldenrod

      case Piece.ComfortGroup.Chair:
        return new Color(0.663f, 0.663f, 0.663f); // Dark Grey

      case Piece.ComfortGroup.None:
        return Color.cyan; // Cyan

      default:
        return Color.white; // White
    }
  }

  public static Color GetColorForComfortValue(Piece.ComfortGroup comfortGroup, int comfort) {
    if (comfortGroup == Piece.ComfortGroup.None) {
      return Color.white;
    }

    int comfortMax = comfortGroup switch {
      Piece.ComfortGroup.Fire => 2,
      Piece.ComfortGroup.Carpet => 1,
      Piece.ComfortGroup.Banner => 1,
      Piece.ComfortGroup.Table => 2,
      Piece.ComfortGroup.Bed => 2,
      Piece.ComfortGroup.Chair => 3,
      Piece.ComfortGroup.None => 6,
      _ => 0
    };

    if (comfortMax <= 0) {
      return Color.white;
    }

    if (comfort >= comfortMax) {
      return Color.green;
    }

    if (comfort == 1) {
      return Color.red;
    }

    return new Color(1f, 0.55f, 0f); // Orange
  }
}
