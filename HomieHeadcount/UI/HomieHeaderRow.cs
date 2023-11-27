using HomieHeadcount.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HomieHeadcount {
  public class HomieHeaderRow {
    static readonly Dictionary<string, Font> FontCache = new();
    static Font AveriaSerifLibre { get => FindFont("AveriaSerifLibre-Regular"); }

    public GameObject Row { get; private set; }
    public Transform RowTransform { get; private set; }

    readonly Tameable _homie;
    readonly Text _homieNameText;
    readonly Text _homieHealthText;
    readonly Text _homiePositionText;
    readonly string _baseHeaderText = "Homies Summoned: ";


    public HomieHeaderRow(Transform parentTransform) {
      Row = CreateChildRow(parentTransform);

      _homieNameText = CreateChildLabel(Row.transform, GetHeaderText());
    }

    public void Update() {
      _homieNameText.text = GetHeaderText();
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

    public Text CreateChildLabel(Transform parentTransform, string displaytext) {
      GameObject label = new($"{parentTransform.name}.Label", typeof(RectTransform));
      label.transform.SetParent(parentTransform);

      Text text = label.AddComponent<Text>();
      text.supportRichText = true;
      text.font = AveriaSerifLibre;
      text.fontSize = 48;
      text.alignment = TextAnchor.MiddleCenter;
      text.color = Color.white;
      text.resizeTextForBestFit = false;
      text.text = displaytext;

      Outline outline = label.AddComponent<Outline>();
      outline.effectColor = Color.black;

      label.AddComponent<LayoutElement>();

      return text;
    }

    public static Font FindFont(string name) {
      if (!FontCache.TryGetValue(name, out Font font)) {
        font = Resources.FindObjectsOfTypeAll<Font>().First(f => f.name == name);
        FontCache[name] = font;
      }

      return font;
    }

    string GetHeaderText() {
      return $"{_baseHeaderText}{HomieCounter.Count()}";
    }
  }
}
