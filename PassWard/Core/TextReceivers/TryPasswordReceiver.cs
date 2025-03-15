namespace PassWard;

public sealed class TryPasswordReceiver : TextReceiver {
  readonly PrivateArea _privateArea;

  public TryPasswordReceiver(PrivateArea privateArea) {
    _privateArea = privateArea;
  }

  public string GetText() {
    return string.Empty;
  }

  public void SetText(string enteredPassword) {
    int passwordHash = _privateArea.m_nview.GetZDO().GetInt(WardManager.PasswordHash, -1);

    if (passwordHash == -1) {
      WardManager.ShowMessage("No password on ward.");
      return;
    }

    if (enteredPassword.GetStableHashCode() != passwordHash) {
      WardManager.ShowMessage("Incorrect password.");
      return;
    }

    WardManager.OptPlayerIn(_privateArea);
  }
}
