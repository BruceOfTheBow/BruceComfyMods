using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HomieHeadcount.Extensions {
  public static class TameableExtensions {
    readonly static Dictionary<string, string> _itemNameConversions = new () {
      {"Dragur axe", "Sword" },
      {"$item_shield_wood", "Wood Shield" },
      {"$item_shield_bronzebuckler", "Bronze Buckler" }
    };
    public static string GetName(this Tameable tameable) {
      if (!tameable.m_nview 
        || !tameable.m_nview.IsValid()) {

        return "";
      }

      return tameable.m_nview.GetZDO().GetString(ZDOVars.s_tamedName, "");
    }

    public static ItemDrop.ItemData GetWeapon(this Tameable tameable) {
      if (!tameable
          || !tameable.gameObject
          || !tameable.m_character
          || ((Humanoid)tameable.m_character).GetCurrentWeapon() == null) {

        return null;
      }

      return ((Humanoid)tameable.m_character).GetCurrentWeapon();
    }

    public static ItemDrop.ItemData GetOffHand(this Tameable tameable) {
      if (!tameable.gameObject
          || !tameable.m_character
          || ((Humanoid)tameable.m_character).GetCurrentBlocker() == null) {

        return null;
      }

      return ((Humanoid)tameable.m_character).GetCurrentBlocker();
    }

    public static float GetHealth(this Tameable tameable) {
      if (!tameable.gameObject
        || !tameable.gameObject.TryGetComponent(out Character character)) {

        return 0;
      }

      return character.GetHealth();
    }

    public static float GetDistanceFromPlayer(this Tameable tameable) {
      if (!Player.m_localPlayer) {
        return 0f;
      }

      Vector3 position = tameable.GetPosition();
      if (position == Vector3.zero) {
        return 0f;
      }

      return Vector3.Distance(position, Player.m_localPlayer.transform.position);
    }

    public static string GetStringDistanceFromPlayer(this Tameable tameable) {
      if (!Player.m_localPlayer) {
        return "";
      }

      Vector3 position = tameable.GetPosition();
      if (position == Vector3.zero) {
        return "";
      }

      return Vector3.Distance(position, Player.m_localPlayer.transform.position).ToString("F1") + "m";
    }

    public static string GetLoadout(this Tameable tameable) {
      if (tameable == null) {
        return "";
      }

      ItemDrop.ItemData weapon = tameable.GetWeapon();
      ItemDrop.ItemData offHand = tameable.GetOffHand();

      if (weapon != null) {
        if (offHand != null && weapon.m_shared.m_name != offHand.m_shared.m_name) {
          return $"{ConvertItemName(weapon.m_shared.m_name)} + {ConvertItemName(offHand.m_shared.m_name)}";
        }
         
        return ConvertItemName(weapon.m_shared.m_name);
      }

      if (offHand != null) {
        return $"{ConvertItemName(offHand.m_shared.m_name)}";
      }

      return null;      
    }

    public static Vector3 GetPosition(this Tameable tameable) {
      if (!tameable.m_nview || !tameable.m_nview.gameObject) {
        return Vector3.zero;
      }

      return tameable.m_nview.gameObject.transform.position;
    }

    static string ConvertItemName(string itemName) {
      if (_itemNameConversions.ContainsKey(itemName)) {
        return _itemNameConversions[itemName];
      }

      return itemName;
    }
  }
}
