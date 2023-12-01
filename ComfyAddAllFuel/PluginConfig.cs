using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using BepInEx.Configuration;

namespace AddAllFuel {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<bool> ExcludeFinewood { get; private set; }
    public static ConfigEntry<KeyCode> AddAllModifier { get; private set; }
    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("Global", "isModEnabled", true, "Globally enable or disable this mod.");
      ExcludeFinewood = config.Bind("ExcludeFinewood", "excludeFinewood", true, "Filter finewood out of items to add to kilns");

      AddAllModifier = 
        config.Bind(
          "ModifierKey", 
          "ModifierKey", 
          KeyCode.LeftShift, 
          "Modifier key to hold for using add all feature.");

    }
  }
}
