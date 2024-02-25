using static PassWard.PassWard;

namespace PassWard {
  public class WardPassword : TextReceiver {
    string password;
    public WardPassword() {
    }

    string TextReceiver.GetText() {
      return password;
    }

    void TextReceiver.SetText(string password) {
      this.password = password;
    }

    public string GetText(TextReceiver tr) {
      return tr.GetText();
    }
  }
}
