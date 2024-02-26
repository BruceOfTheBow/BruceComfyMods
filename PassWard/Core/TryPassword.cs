using UnityEngine;

using static PassWard.PassWard;

namespace PassWard {
  public class TryPassword : TextReceiver {
    ZNetView zNetView;
    public TryPassword(ZNetView zNetView) {
      this.zNetView = zNetView;
    }

    string TextReceiver.GetText() {
      return "";
    }

    void TextReceiver.SetText(string enteredPassword) {
      int passwordHash = zNetView.GetZDO().GetInt(PasswordZdoFieldHash, -1);
      if (passwordHash == -1) {
        ShowMessage("No password on ward.");
        return;
      }

      if (enteredPassword.GetStableHashCode() != passwordHash) {
        ShowMessage("Incorrect password.");
        return;
      }

      OptPlayerIn();
    }

    public string GetText(TextReceiver tr) {
      return tr.GetText();
    }

    void OptPlayerIn() {
      if (!zNetView.gameObject.TryGetComponent(out PrivateArea privateArea)
            || !Player.m_localPlayer) {
        LogError("Error opting-in after password success.");
        return;
      }

      if (privateArea.IsPermitted(Player.m_localPlayer.GetPlayerID())) {
        ShowMessage("Already opted-in.");
        return;
      }

      privateArea.AddPermitted(Player.m_localPlayer.GetPlayerID(), Player.m_localPlayer.GetPlayerName());
      ShowMessage("Password accepted.");

      if (!Player.m_localPlayer.TryGetComponent(out Talker talker)) {
        return;
      }

      UserInfo userInfo = new UserInfo();
      userInfo.Name = $"{privateArea.GetCreatorName()}'s Ward";
      userInfo.Gamertag = UserInfo.GetLocalPlayerGamertag();
      userInfo.NetworkUserId = PrivilegeManager.GetNetworkUserId();

      talker.m_nview.InvokeRPC(ZNetView.Everybody, "Say", new object[] {
        (int)Talker.Type.Normal,
        userInfo,
        "Password accepted.",
        PrivilegeManager.GetNetworkUserId()
      });
    }
  }
}
