namespace PassWard;

public sealed class WardPasswordReceiver : TextReceiver {
  readonly PrivateArea _privateArea;

  public WardPasswordReceiver(PrivateArea privateArea) {
    _privateArea = privateArea;
  }

  public string GetText() {
    return string.Empty;
  }

  public void SetText(string password) {
    _privateArea.m_nview.GetZDO().Set(WardManager.PasswordHash, password.GetStableHashCode());
    WardManager.ShowMessage("Password set.");
  }
}
