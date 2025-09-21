namespace Hygge;

using ComfyLib;

using TMPro;

using UnityEngine;

public sealed class LabelValueRow {
  public GameObject Container { get; private set; }
  public RectTransform RectTransform { get; private set; }

  public TextMeshProUGUI Label { get; private set; }
  public TextMeshProUGUI Value { get; private set; }

  public LabelValueRow(Transform parentTransform) {
    Container = CreateContainer(parentTransform);
    RectTransform = Container.GetComponent<RectTransform>();

    Label = CreateLabel(RectTransform);
    Value = CreateValue(RectTransform);
  }

  static GameObject CreateContainer(Transform parentTransform) {
    GameObject container = new("LabelValueRow", typeof(RectTransform));
    container.transform.SetParent(parentTransform, worldPositionStays: false);

    container.GetComponent<RectTransform>()
        .SetAnchorMin(Vector2.up)
        .SetAnchorMax(Vector2.one)
        .SetPivot(new(0.5f, 1f))
        .SetPosition(Vector2.zero)
        .SetSizeDelta(new(-20f, 30f));

    return container;
  }

  static TextMeshProUGUI CreateLabel(Transform parentTransform) {
    TextMeshProUGUI label = UIBuilder.CreateTMPLabel(parentTransform);

    label.rectTransform
        .SetAnchorMin(Vector2.zero)
        .SetAnchorMax(Vector2.one)
        .SetPivot(new(0f, 0.5f))
        .SetPosition(Vector2.zero)
        .SetSizeDelta(Vector2.zero);

    label
        .SetAlignment(TextAlignmentOptions.Left)
        .SetFontSize(18f)
        .SetText("Label");

    return label;
  }

  static TextMeshProUGUI CreateValue(Transform parentTransform) {
    TextMeshProUGUI value = UIBuilder.CreateTMPLabel(parentTransform);

    value.rectTransform
        .SetAnchorMin(Vector2.zero)
        .SetAnchorMax(Vector2.one)
        .SetPivot(new(0f, 0.5f))
        .SetPosition(Vector2.zero)
        .SetSizeDelta(Vector2.zero);

    value
        .SetAlignment(TextAlignmentOptions.Right)
        .SetFontSize(18f)
        .SetText("Value");

    return value;
  }
}
