using static PassWard.PassWard;

namespace PassWard {
  public class WardPassword : TextReceiver {
    ZNetView zNetView;
    public WardPassword(ZNetView zNetView) {
      this.zNetView = zNetView;
    }

    string TextReceiver.GetText() {
      return "";
    }

    void TextReceiver.SetText(string password) {
      zNetView.GetZDO().Set(PasswordZdoFieldHash, password.GetStableHashCode());
      ShowMessage("Password set.");
    }

    public string GetText(TextReceiver tr) {
      return tr.GetText();
    }
  }
}
