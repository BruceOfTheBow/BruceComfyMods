using HarmonyLib;

using static PassWard.PluginConfig;
using static PassWard.PassWard;
using BepInEx.Logging;

namespace PassWard {
  [HarmonyPatch(typeof(Hud))]
  public class HudPatch {
    static readonly string _hoverOwnerPasswordTemplate = "{0}\n<size=18>[<color=yellow>{1}</color>] Remove password.</size>\n<size=18>[<color=yellow>{2}</color>] Change password.</size>";
    static readonly string _hoverOwnerNoneTemplate = "{0}\n<size=18>[<color=yellow>{1}</color>] Add password.</size>";
    static readonly string _hoverNonownerTemplatePassword = "{0}\n\n<size=18><color=green>Password enabled</color>\n[<color=yellow>{1}</color>] Enter password.</size>";
    static readonly string _hoverNonownerTemplateNone = "{0}\n\n<size=18><color=red>No password</color>";

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
        UpdateOwnerCrosshair(__instance, zNetView);
      } else {
        UpdateNonownerCrosshair(__instance, zNetView);
      }

      if (EnterPasswordKey.Value.IsDown()) {
        EnterPassword(zNetView);
      }

      if (RemovePasswordKey.Value.IsDown()) {
        RemovePassword(zNetView);
      }
    }

    static void UpdateOwnerCrosshair(Hud hud, ZNetView zNetView) {
      if (hud == null) {
        return;
      }

      if (HasPassword(zNetView)) {
        hud.m_hoverName.text = BuildOwnerHoverTextWithPassword(hud);
        return;
      }

      hud.m_hoverName.text = BuildOwnerHoverTextNoPassword(hud);
    }

    static string BuildOwnerHoverTextWithPassword(Hud hud) {
      return string.Format(
          _hoverOwnerPasswordTemplate,
          hud.m_hoverName.text,
          RemovePasswordKey.Value,
          EnterPasswordKey.Value);
    }

    static string BuildOwnerHoverTextNoPassword(Hud hud) {
      return string.Format(
          _hoverOwnerNoneTemplate,
          hud.m_hoverName.text,
          EnterPasswordKey.Value);
    }

    static void UpdateNonownerCrosshair(Hud hud, ZNetView zNetView) {
      if (hud == null) {
        return;
      }

      if (!HasPassword(zNetView)) {
        hud.m_hoverName.text = BuildNonownerHoverTextNone(hud);
        return;
      }

      hud.m_hoverName.text = BuildNonownerHoverTextPassword(hud);
    }

    static string BuildNonownerHoverTextPassword(Hud hud) {
      return string.Format(
          _hoverNonownerTemplatePassword,
          hud.m_hoverName.text,
          EnterPasswordKey.Value);
    }

    static string BuildNonownerHoverTextNone(Hud hud) {
      return string.Format(
          _hoverNonownerTemplateNone,
          hud.m_hoverName.text);
    }

    static void EnterPassword(ZNetView zNetView) {
      if (IsPlayerOwned(zNetView)) {
        AddPassword(zNetView);
        return;
      }

      int passwordHash = zNetView.GetZDO().GetInt(PasswordZdoFieldHash, -1);
        
      if (passwordHash == -1) {
        ShowMessage("No password assigned to this ward. Cannot opt in with password.");
        return;
      }

      TryPassword tryPassword = new TryPassword(zNetView);
      TextInput.instance.RequestText(tryPassword, EnterPasswordInputText, 32);
    }

    static void RemovePassword(ZNetView zNetView) {
      if (!IsPlayerOwned(zNetView)) {
        ShowMessage("You do not own this ward. Cannot remove password.");
        return;
      }

      zNetView.GetZDO().RemoveInt(PasswordZdoFieldHash);
    }

    static void AddPassword(ZNetView zNetView) {
      WardPassword passwordReceiver = new WardPassword(zNetView);

      if (HasPassword(zNetView)) {
        TextInput.instance.RequestText(passwordReceiver, ChangePasswordInputText, 32);
        return;
      }

      TextInput.instance.RequestText(passwordReceiver, SetPasswordInputText, 32);
    }

    static bool HasPassword(ZNetView zNetView) {
      if (zNetView.GetZDO().GetInt(PasswordZdoFieldHash, -1) == -1) {
        return false;
      }

      return true;
    }

    static bool IsPlayerOwned(ZNetView zNetView) {
      long creatorId = zNetView.GetZDO().GetLong(ZDOVars.s_creator, 0L);

      if (Player.m_localPlayer.GetPlayerID() != creatorId) {
        return false;
      }

      return true;
    }
  }
}
