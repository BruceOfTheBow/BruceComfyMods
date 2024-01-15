using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomieHeadcount {
  public static  class PanelManager {
    public static HomieCountPanel HomieCountPanel { get; set; }

    public static void Toggle() {
      if(!HomieCountPanel?.Panel) {
        HomieCountPanel = new(Hud.instance.transform);

        HomieCountPanel.SetInitialPosition();
        HomieCountPanel.Panel.SetActive(true);
      }

      Update();
      HomieCountPanel.Panel.SetActive(!HomieCountPanel.Panel.activeSelf);
    }

    public static void Hide() {
      if (!HomieCountPanel?.Panel) {
        return;
      }

      HomieCountPanel.Panel.SetActive(false);
    }

    public static void Destroy() {
      if (!HomieCountPanel?.Panel) {
        return;
      }

      UnityEngine.GameObject.Destroy(HomieCountPanel.Panel);
      HomieCountPanel = null;
    }

    public static bool IsHomiePanelActive() {
      return HomieCountPanel?.Panel && HomieCountPanel.Panel.activeSelf;
    }

    public static void Update() {
      if (!HomieCountPanel?.Panel) {
        return;
      }

      HomieCountPanel.Update();
    }

    public static void AddHomie(Tameable tameable) {
      if (!HomieCountPanel?.Panel) {
        return;
      }
       
      HomieCountPanel.AddHomie(tameable);
    }
  }
}
