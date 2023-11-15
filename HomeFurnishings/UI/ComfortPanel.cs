using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HomeFurnishings {
  public class ComfortPanel {
    public GameObject Panel { get; private set; }

    readonly List<TMPro.TMP_Text> Labels = new();

    public ComfortPanel(Transform parentTransform) {
      Panel = CreateChildPanel(parentTransform);
    }

    GameObject CreateChildPanel(Transform parentTransform) {
      GameObject panel = new("ComfortPanel", typeof(RectTransform));
      panel.transform.SetParent(parentTransform, false);

      CanvasGroup canvasGroup = panel.AddComponent<CanvasGroup>();
      canvasGroup.blocksRaycasts = true;

      return panel;
    }

    public void Update(Piece piece) {

    }
  }
}
