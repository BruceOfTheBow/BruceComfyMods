using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using static Pintervention.Pintervention;

namespace Pintervention {
  public class PlayerListRow {
    public GameObject Row { get; private set; }
    public Image PinIcon { get; private set; }
    public TMP_Text PinName { get; private set; }
    public TMP_Text PinCount { get; private set; }
    public TMP_Text FilterStatus { get; private set; }

    long _pid;

    public PlayerListRow(Transform parentTransform) {
      Row = CreateChildRow(parentTransform);

      PinIcon = CreateChildPinIcon(Row.transform).Image();
      PinName = CreateChildPinName(Row.transform);

      UIBuilder.CreateRowSpacer(Row.transform);

      PinCount = CreatePinCountValue(Row.transform);
      PinCount.color = new(0.565f, 0.792f, 0.976f);

      FilterStatus = CreatePinCountValue(Row.transform);
      FilterStatus.fontSize = 14f;
    }

    public PlayerListRow SetRowContent(long pid) {
      _pid = pid;
      PinIcon.SetSprite(Minimap.m_instance.GetSprite(Minimap.PinType.Player));

      UpdateName();
      UpdateCount();
      UpdateFilterStatus();

      return this;
    }

    public void UpdateName() {
      PinName.SetText(NameManager.GetPlayerNameById(_pid));
    }

    public void UpdateCount() {
      PinCount.SetText($"{PinOwnerManager.GetPinsByPid(_pid).Count}");
    }

    public void UpdateFilterStatus() {
      if (PinOwnerManager.IsFiltered(_pid)) {
        FilterStatus.SetText("<color=red>X</color>");
        return;
      }

      FilterStatus.SetText("<color=green><b>\u2713</b></color>");
    }

    GameObject CreateChildRow(Transform parentTransform) {
      GameObject row = new("PinList.Row", typeof(RectTransform));
      row.SetParent(parentTransform);
     
      row.AddComponent<HorizontalLayoutGroup>()
          .SetChildControl(width: true, height: true)
          .SetChildForceExpand(width: false, height: false)
          .SetChildAlignment(TextAnchor.MiddleCenter)
          .SetPadding(left: 5, right: 10, top: 5, bottom: 5)
          .SetSpacing(5f);

      row.AddComponent<Image>()
          .SetType(Image.Type.Sliced)
          .SetSprite(UIBuilder.CreateRoundedCornerSprite(400, 400, 5));

      row.AddComponent<Button>()
          .SetOnClickListener(ToggleFilter)
          .SetNavigationMode(Navigation.Mode.None)
          .SetTargetGraphic(row.Image())
          .SetColors(ButtonColorBlock.Value);


      row.AddComponent<ContentSizeFitter>()
          .SetHorizontalFit(ContentSizeFitter.FitMode.Unconstrained)
          .SetVerticalFit(ContentSizeFitter.FitMode.PreferredSize);

      row.AddComponent<ParentSizeFitter>();

      return row;
    }

    public void ToggleFilter() {
      PinOwnerManager.ToggleFilter(_pid);
      UpdateFilterStatus();
    }

    GameObject CreateChildPinIcon(Transform parentTransform) {
      GameObject icon = new("Pin.Icon", typeof(RectTransform));
      icon.SetParent(parentTransform);

      icon.AddComponent<LayoutElement>()
          .SetPreferred(width: 20f, height: 20f);

      icon.AddComponent<Image>()
          .SetType(Image.Type.Simple);

      return icon;
    }

    TMP_Text CreateChildPinName(Transform parentTransform) {
      TMP_Text name = UIBuilder.CreateTMPLabel(parentTransform);
      name.SetName("Pin.Name");

      name.alignment = TextAlignmentOptions.Left;
      name.text = "Blankname1234567891011121314";

      name.gameObject.AddComponent<LayoutElement>()
          .SetPreferred(width: name.GetPreferredValues().x);

      return name;
    }

    TMP_Text CreatePinCountValue(Transform parentTransform) {
      TMP_Text value = UIBuilder.CreateTMPLabel(parentTransform);
      value.SetName("Pin.Count.Value");

      value.alignment = TextAlignmentOptions.Right;
      value.text = "-12X\u2713";

      value.gameObject.AddComponent<LayoutElement>()
          .SetPreferred(width: value.GetPreferredValues().x, height: value.GetPreferredValues().y);

      return value;
    }

    static readonly Lazy<ColorBlock> ButtonColorBlock =
        new(() =>
          new() {
            normalColor = new Color(0f, 0f, 0f, 0.01f),
            highlightedColor = new Color32(50, 161, 217, 128),
            disabledColor = new Color(0f, 0f, 0f, 0.1f),
            pressedColor = new Color32(50, 161, 217, 192),
            selectedColor = new Color32(50, 161, 217, 248),
            colorMultiplier = 1f,
            fadeDuration = 0f,
          });
  }
}
