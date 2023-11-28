using HomieHeadcount.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HomieHeadcount {
  public class HomieRow {
    static readonly Dictionary<string, Font> FontCache = new();
    static Font AveriaSerifLibre { get => FindFont("AveriaSerifLibre-Regular"); }

    public GameObject Row { get; private set; }
    public Transform RowTransform { get; private set; }

    readonly Tameable _homie;
    readonly Text _homieNameText;
    readonly Text _homieLoadoutText;
    readonly Text _homieHealthText;
    readonly Text _homiePositionText;

    
    public HomieRow(Transform parentTransform, Tameable homie) {
      Row = CreateChildRow(parentTransform);

      _homie = homie;

      RectTransform rectTransform = parentTransform.GetComponent<RectTransform>();

      _homieNameText = CreateChildLabel(Row.transform, "name", GetNameWidth(rectTransform));
      _homieLoadoutText = CreateChildLabel(Row.transform, "loadout", GetLoadoutWidth(rectTransform));
      _homieHealthText = CreateChildLabel(Row.transform, "health", GetHealthWidth(rectTransform));
      _homiePositionText = CreateChildLabel(Row.transform, "position", GetPositionWidth(rectTransform));

      Update();
    }

    public void Update() {
      _homieNameText.text = _homie.GetName();
      _homieLoadoutText.text = _homie.GetLoadout();
      _homieHealthText.text = _homie.GetHealth().ToString("F1");
      _homiePositionText.text = _homie.GetStringDistanceFromPlayer();
    }

    GameObject CreateChildRow(Transform parentTransform) {
      GameObject row = new("Row", typeof(RectTransform));
      row.transform.SetParent(parentTransform);

      HorizontalLayoutGroup hlg = row.AddComponent<HorizontalLayoutGroup>();
      hlg.childControlWidth = true;
      hlg.childControlHeight = true;
      hlg.childForceExpandWidth = true;
      hlg.childForceExpandHeight = true;

      hlg.padding.left = 8;
      hlg.padding.right = 8;
      hlg.padding.top = 2;
      hlg.padding.bottom = 2;
      hlg.spacing = 8f;
      hlg.childAlignment = TextAnchor.MiddleCenter;

      return row;
    }

    public Text CreateChildLabel(Transform parentTransform, string name, float width) {
      GameObject label = new($"{parentTransform.name}.{name}", typeof(RectTransform));
      label.transform.SetParent(parentTransform);

      Text text = label.AddComponent<Text>();
      text.supportRichText = true;
      text.font = AveriaSerifLibre;
      text.fontSize = 28;
      text.alignment = TextAnchor.MiddleCenter;
      text.color = Color.white;
      text.resizeTextForBestFit = false;
      text.text = "";

      Outline outline = label.AddComponent<Outline>();
      outline.effectColor = Color.black;

      LayoutElement layoutElement = label.AddComponent<LayoutElement>();
      layoutElement.minWidth = width/2;
      layoutElement.preferredWidth = width;

      return text;
    }

    public Tameable GetHomie() {
      return _homie;
    }

    public static Font FindFont(string name) {
      if (!FontCache.TryGetValue(name, out Font font)) {
        font = Resources.FindObjectsOfTypeAll<Font>().First(f => f.name == name);
        FontCache[name] = font;
      }

      return font;
    }

    float GetNameWidth(RectTransform parentTransform) {
      return parentTransform.rect.width * 0.5f;
    }

    float GetLoadoutWidth(RectTransform parentTransform) {
      return parentTransform.rect.width * 0.8f;
    }

    float GetHealthWidth(RectTransform parentTransform) {
      return parentTransform.rect.width * 0.25f;
    }

    float GetPositionWidth(RectTransform parentTransform) {
      return parentTransform.rect.width * 0.25f;
    }
  }
}
