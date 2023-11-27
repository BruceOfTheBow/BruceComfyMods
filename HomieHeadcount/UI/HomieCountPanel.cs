using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace HomieHeadcount {
  public  class HomieCountPanel {
    public GameObject Panel { get; private set; }
    readonly RectTransform panelTransform;

    readonly HomieHeaderRow _homieHeader;
    readonly List<HomieRow> HomieRows = new();

    public HomieCountPanel(Transform parentTransform) {
      Panel = CreateChildPanel(parentTransform);
      panelTransform = Panel.GetComponent<RectTransform>();
      _homieHeader = new (panelTransform);

      AddNewHomies();
    }

    public void SetInitialPosition() {
      panelTransform.anchorMin = new(0f, 0.5f);
      panelTransform.anchorMax = new(0f, 0.5f);
      panelTransform.pivot = new(0f, 0.5f);
      panelTransform.position = GetDefaultPosition();
      panelTransform.sizeDelta = GetSize();
    }

    public void Update() {
      UpdateHomieInfo();
      UpdateHeader();
      UpdateHeight();
      UpdatePosition();
    }

    void UpdateHeader() {
      _homieHeader.Update();
    }

    void UpdateHomieInfo() {
      foreach (HomieRow homieRow in HomieRows.ToList()) {
        if (homieRow.GetHomie() == null) {
          RemoveHomieRow(homieRow);

          continue;
        }
        
        homieRow.Update();
      }
    }

    void UpdatePosition() {

    }

    void AddNewHomies() {
      foreach (Tameable homie in HomieCounter.GetHomies()) {
        AddHomie(homie);
      }      
    }

    public void AddHomie(Tameable homie) {
      HomieRows.Add(new HomieRow(panelTransform, homie));
    }

    public void RemoveHomieRow(HomieRow homieRow) {
      UnityEngine.GameObject.Destroy(homieRow.Row);
      HomieRows.Remove(homieRow);
    }

    GameObject CreateChildPanel(Transform parentTransform) {
      GameObject panel = new("Panel", typeof(RectTransform));
      panel.transform.SetParent(parentTransform);

      VerticalLayoutGroup vLayoutGroup = panel.AddComponent<VerticalLayoutGroup>();
      vLayoutGroup.childControlWidth = true;
      vLayoutGroup.childControlHeight = true;

      vLayoutGroup.childForceExpandWidth = true;
      vLayoutGroup.childForceExpandHeight = true;

      vLayoutGroup.padding.left = 2;
      vLayoutGroup.padding.right = 2;
      vLayoutGroup.padding.top = 2;
      vLayoutGroup.padding.bottom = 2;
      vLayoutGroup.spacing = 4f;

      Image image = panel.AddComponent<Image>();
      image.type = Image.Type.Sliced;
      image.color = new(0f, 0f, 0f, 0.8f);

      CanvasGroup canvasGroup = panel.AddComponent<CanvasGroup>();
      canvasGroup.blocksRaycasts = true;

      return panel;
    }

    void UpdateHeight() {
      panelTransform.sizeDelta = new(GetDefaultWidth(), GetConditionalHeight());
    }

    float GetConditionalHeight() {
      if (HomieRows.Count() <= 2) {
        return GetDefaultHeight();
      }

      return HomieRows.Count() * GetDefaultHeight()/3f;
    }

    Vector2 GetDefaultPosition() {
      return new Vector2(Screen.width*0.795f, Screen.height / 2);
    }

    Vector2 GetSize() {
      return new Vector2(GetDefaultWidth(), GetConditionalHeight());
    }

    float GetDefaultHeight() {
      return Screen.height / 8;
    }

    float GetDefaultWidth() {
      return Screen.width / 5;
    }
  }
}
