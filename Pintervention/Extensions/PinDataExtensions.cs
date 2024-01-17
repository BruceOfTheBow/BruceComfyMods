using UnityEngine;

namespace Pintervention.Extensions {
  public static class PinDataExtensions {
    public static Minimap.PinData SetType(this Minimap.PinData pin, Minimap.PinType pinType) {
      pin.m_type = pinType;
      return pin;
    }

    public static Minimap.PinData SetName(this Minimap.PinData pin, string name) {
      pin.m_name = name;
      return pin;
    }

    public static Minimap.PinData SetPosition(this Minimap.PinData pin, Vector3 position) {
      pin.m_pos = position;
      return pin;
    }

    public static Minimap.PinData SetSave(this Minimap.PinData pin, bool save) {
      pin.m_save = save;
      return pin;
    }

    public static Minimap.PinData SetChecked(this Minimap.PinData pin, bool isChecked) {
      pin.m_checked = isChecked;
      return pin;
    }

    public static Minimap.PinData SetOwnerID(this Minimap.PinData pin, long ownerID) {
      pin.m_ownerID = ownerID;
      return pin;
    }

    public static Minimap.PinData SetAuthor(this Minimap.PinData pin, string author) {
      pin.m_author = author;
      return pin;
    }

    public static Minimap.PinData SetIcon(this Minimap.PinData pin, Sprite icon) {
      pin.m_icon = icon;
      return pin;
    }
  }
}
