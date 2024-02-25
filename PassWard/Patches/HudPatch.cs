using HarmonyLib;

using static PassWard.PluginConfig;
using static PassWard.PassWard;

namespace PassWard {
  [HarmonyPatch(typeof(Hud))]
  public class HudPatch {
    static readonly string _hoverNameTextTemplate = "{0}{1}<size=18>[{2}] Regenerate Dungeon</size>";
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Hud.UpdateCrosshair))]
    public static void HudUpdateCrosshairPostfix(ref Hud __instance, ref Player player) {
      if (!IsModEnabled.Value
        || !Player.m_localPlayer
        || !Player.m_localPlayer.m_hovering
        || !Player.m_localPlayer.m_hovering.transform.root.TryGetComponent(out ZNetView zNetView)
        || !zNetView.IsValid()
        || zNetView.GetPrefabName().GetStableHashCode() != WardHash) {

        return;
      }

      if (IsPlayerOwned(zNetView)) {
        UpdateOwnerCrosshair();
      } else {
        UpdateNonownerCrosshair();
      }

      if (ChangePasswordKey.Value.IsDown()) {
        ChangePassword(zNetView);
      }

      if (EnterPasswordKey.Value.IsDown()) {
        EnterPassword(zNetView);
      }

      if (RemovePasswordKey.Value.IsDown()) {
        RemovePassword(zNetView);
      }

      
    }

    static void UpdateOwnerCrosshair(Hud hud) {
      if (hud == null) {
        return;
      }

      hud.m_hoverName.text = string.Format(_hoverNameTextTemplate, hud.m_hoverName.text, (hud.m_hoverName.text.Length > 0) ? "\n" : string.Empty, RegenKey.Value);
    }

    static void UpdateNonownerCrosshair(Hud hud, ZNetView zNetView) {
      if (hud == null) {
        return;
      }

      if (!HasPassword(zNetView)) {
        hud.m_hoverName.text = string.Format(_hoverNameTextTemplate, hud.m_hoverName.text, (hud.m_hoverName.text.Length > 0) ? "\n" : string.Empty, RegenKey.Value);
        return;
      }

      hud.m_hoverName.text = string.Format(_hoverNameTextTemplate, hud.m_hoverName.text, (hud.m_hoverName.text.Length > 0) ? "\n" : string.Empty, RegenKey.Value);
    }

    static void ChangePassword(ZNetView zNetView) {
      if (!IsPlayerOwned(zNetView)) {
        ShowMessage("You do not own this ward. Cannot change password.");
        return;
      }

      if (!HasPassword(zNetView)) {
        ShowMessage("Ward does not have password");
      }
    }

    static void EnterPassword(ZNetView zNetView) {
      if (!IsPlayerOwned(zNetView)) {
        int passwordHash = zNetView.GetZDO().GetInt(PasswordZdoFieldHash, -1);
        
        if (passwordHash == -1) {
          ShowMessage("No password assigned to this ward. Cannot opt in with password.");
          return;
        }

        WardPassword passwordReceiver = new WardPassword();
        TextInput.instance.RequestText(passwordReceiver, SetPasswordInputText, 32);

        if (passwordHash == passwordReceiver.GetText(passwordReceiver).GetStableHashCode()) {
          OptPlayerIn(zNetView);
        } else {
          ShowMessage("Incorrect password.");
        }

        return;
      }

      AddPassword(zNetView);
    }

    static void OptPlayerIn(ZNetView zNetView) {

    }

    static void RemovePassword(ZNetView zNetView) {
      if (!IsPlayerOwned(zNetView)) {
        ShowMessage("You do not own this ward. Cannot remove password.");
        return;
      }

      zNetView.GetZDO().RemoveInt(PasswordZdoFieldHash);
    }

    static void AddPassword(ZNetView zNetView) {
      if (HasPassword(zNetView)) {
        ShowMessage("Ward already has password. Change or remove password.");
        return;
      }

      WardPassword passwordReceiver = new WardPassword();
      TextInput.instance.RequestText(passwordReceiver, SetPasswordInputText, 32);
      zNetView.GetZDO().Set(PasswordZdoFieldHash, passwordReceiver.GetText(passwordReceiver).GetStableHashCode());
      ShowMessage("Password set.");
    }

    static bool HasPassword(ZNetView zNetView) {
      if (zNetView.GetZDO().GetInt(PasswordZdoFieldHash, -1) == -1) {
        return false;
      }

      return true;
    }

    static bool IsPlayerOwned(ZNetView zNetView) {
      if (!zNetView.gameObject.TryGetComponent(out PrivateArea privateArea)
            || !privateArea.m_piece.IsCreator()) {
        return false;
      }

      return true;
    }
  }
}
