using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;

using UnityEngine;

namespace BatchDeposit {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled = default!;
    public static ConfigEntry<KeyCode> Modifier = default!;

    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");
      Modifier = config.Bind("DepositAllModifier", "depositAllModifier", KeyCode.LeftAlt, "Hot key modifier for deposit all on left click.");
    }
  }
}
