namespace PassWard;

using System.Text;

using static PluginConfig;

public static class WardManager {
  public const int GuardStoneHash = -1024209535;  // "guard_stone".GetStableHashCode();
  public const int PasswordHash = 1595635768;     // "passward.password".GetStableHashCode();

  public static void OptPlayerIn(PrivateArea privateArea) {
    if (!privateArea || !Player.m_localPlayer) {
      PassWard.LogError("Error opting-in after password success.");
      return;
    }

    long playerId = Player.m_localPlayer.GetPlayerID();

    if (privateArea.IsPermitted(playerId)) {
      ShowMessage("Already opted-in.");
      return;
    }

    privateArea.AddPermitted(playerId, Player.m_localPlayer.GetPlayerName());
    ShowMessage("Password accepted.");

    if (Player.m_localPlayer.TryGetComponent(out Talker talker)) {
      talker.Say(Talker.Type.Normal, $"[PassWard] {privateArea.GetCreatorName()}'s Ward: password accepted.");
    }
  }

  public static void AddPasswordHoverText(PrivateArea privateArea, StringBuilder hoverText) {
    bool hasPassword = HasPassword(privateArea);

    if (privateArea.m_piece.IsCreator()) {
      if (hasPassword) {
        hoverText
            .Append("\n<size=18>[<color=yellow>")
            .Append(RemovePasswordKey.Value)
            .Append("</color>] Remove password.</size>");
      }

      hoverText
          .Append("\n<size=18>[<color=yellow>")
          .Append(EnterPasswordKey.Value)
          .Append("</color>] ")
          .Append(hasPassword ? "Change password.</size>" : "Add password.</size>");
    } else {
      if (hasPassword) {
        hoverText
            .Append("\n\n<size=18><color=green>Password enabled</color>\n[<color=yellow>")
            .Append(EnterPasswordKey.Value)
            .Append("</color>] Enter password.</size>");
      } else {
        hoverText.Append("\n\n<size=18><color=red>No password</color></size>");
      }
    }
  }

  public static void EnterPassword(PrivateArea privateArea) {
    bool hasPassword = HasPassword(privateArea);

    if (privateArea.m_piece.IsCreator()) {
      WardPasswordReceiver passwordReceiver = new WardPasswordReceiver(privateArea);
      TextInput.instance.RequestText(passwordReceiver, hasPassword ? "Change password" : "Set password", 32);
      return;
    }
    
    if (!hasPassword) {
      ShowMessage("No password assigned to this ward. Cannot opt-in using password.");
      return;
    }

    TryPasswordReceiver tryPassword = new TryPasswordReceiver(privateArea);
    TextInput.instance.RequestText(tryPassword, "Enter password", 32);
  }

  public static void RemovePassword(PrivateArea privateArea) {
    if (!privateArea.m_piece.IsCreator()) {
      ShowMessage("You do not own this ward. Cannot remove password.");
      return;
    }

    if (!HasPassword(privateArea)) {
      ShowMessage($"No password on this ward to remove.");
      return;
    }

    privateArea.m_nview.m_zdo.Set(PasswordHash, -1);

    ShowMessage($"Password removed.");
  }

  public static bool HasPassword(PrivateArea privateArea) {
    return privateArea.m_nview.m_zdo.GetInt(PasswordHash, out int password) && password != -1;
  }

  public static void ShowMessage(string message) {
    if (MessageHud.instance) {
      MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, message);
    }
  }
}
