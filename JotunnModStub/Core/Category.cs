using System.Collections.Generic;
using System.Linq;

namespace PlantThings.Core {
  static class Category {
    static readonly string _misc = "Misc.";
    static readonly string _hanging = "Hanging";
    static readonly string _noPot = "No Pot";
    static readonly string _potted = "Potted";

    public static readonly string[] ShovelCategories = new string[] {
      _misc,
      _hanging,
      _noPot,
      _potted
    };

  public static string GetCategory(string prefabName) {
      if (MiscPrefabs.Contains(prefabName)) {
        return _misc;
      }

      if (HangingPrefabs.Contains(prefabName)) {
        return _hanging;
      }

      if (NoPotPrefabs.Contains(prefabName)) {
        return _noPot;
      }

      if (PottedPrefabs.Contains(prefabName)) {
        return _potted;
      }

      return null;
    }

  public static readonly string[] MiscPrefabs = new string[] {
      "custompiece_fancypot",
      "custompiece_fancypotl",
      "custompiece_hangingpot_large",
      "custompiece_hangingpot_small",
      "custompiece_tcpot1",
      "custompiece_tcpot1l",
      "custompiece_tcpot2",
      "custompiece_tcpot2l",
      "custompiece_tcpot3",
      "custompiece_tcpot3l",
      "custompiece_tcpotrectangle",
      "custompiece_tcpotrectanglel",
      "custompiece_tcpotsquare",
      "custompiece_tcpotsquarel",
      "$custompiece_stump1",
      "$custompiece_stump2",
      "$custompiece_stump3",
      "$custompiece_stump4",
      "$custompiece_stump5",
      "$custompiece_stump6",
      "$custompiece_stump7",
      "$custompiece_log1",
      "$custompiece_log2",
      "$custompiece_log3",
      "$custompiece_log4",
      "$custompiece_log5",
      "$custompiece_stumptable",
      "$custompiece_grassblock_small",
      "$custompiece_grassblock_medium",
      "$custompiece_grassblock_large",
      "$custompiece_dirtblock_small",
      "$custompiece_dirtblock_medium",
      "$custompiece_dirtblock_large",
      "$custompiece_blackpine",
      "$custompiece_grassblock_small_test",
      "$custompiece_grassblock_small_test2",
    };

    public static readonly string[] HangingPrefabs = new string[] {
      "custompiece_vines3",
      "custompiece_vines4",
      "custompiece_vines5",
      "custompiece_vines6",
      "custompiece_vines7",
      "custompiece_vines10",
      "custompiece_vines11",
      "custompiece_vines12",
      "custompiece_vinesvanilla",
      "custompiece_vinesvanillabig",
      "custompiece_vinesvanillastretched",
      "custompiece_vines1.2",
      "custompiece_vines1",
      "custompiece_hangingplant",
      "custompiece_hangingplant1.2",
    };

    public static readonly string[] NoPotPrefabs = new string[] {
      "custompiece_vines8",
      "custompiece_vines9",
      "custompiece_barrelcactus",
      "custompiece_cactus1",
      "custompiece_cactus1.2",
      "custompiece_cactus2",
      "custompiece_desertlily",
      "custompiece_desertmarigold",
      "custompiece_evergladespalm",
      "custompiece_hibiscusflower",
      "custompiece_ivorycanepalm",
      "custompiece_ladypalm",
      "custompiece_pricklypear",
      "custompiece_venusflytrap",
      "custompiece_aeonium",
      "custompiece_aloe",
      "custompiece_bromeliad",
      "custompiece_ivy1",
      "custompiece_ivy1.2",
      "custompiece_cattail",
      "custompiece_kelp",
      "custompiece_kelpbig",
      "custompiece_rosevine",
      "custompiece_tallflowerbush",
      "custompiece_vines2",
      "$custompiece_blackpine",
    };

    public static readonly string[] PottedPrefabs = new string[] {
      "custompiece_birdofparadise",
      "custompiece_bonsai",
      "custompiece_coffeeplant",
      "custompiece_fiddleleaf",
      "custompiece_littlehouseplant",
      "custompiece_orchid",
      "custompiece_spikyplant",
      "custompiece_umbrellapalm",
      "custompiece_rubberfig",
      "custompiece_tallhouseplant",
      "custompiece_aeoniums",
      "custompiece_bamboo",
      "custompiece_coolpot",
      "custompiece_miscplant1",
      "custompiece_miscplant2",
      "custompiece_orchid2",
      "custompiece_pottedcactus1",
      "custompiece_pottedcactus2",
      "custompiece_smallpottedplant",
      "$custompiece_s1_roundcactuspinkflowers",
      "$custompiece_s2_tallcactusredflower",
      "$custompiece_s3_pottedpricklypear",
      "$custompiece_s4_snakeplant",
      "$custompiece_s5_aloe",
      "$custompiece_s6_pachy",
      "$custompiece_s7_calisunset",
      "$custompiece_s8_towercactus",
      "$custompiece_s9_whiteflowers",
      "$custompiece_s10_fatboi",
      "$custompiece_s11_cLehmannii",
      "$custompiece_s12_splitrocks",
      "$custompiece_s13_roundcactusyellowflowers",
      "$custompiece_s14_roseaeonium",
      "$custompiece_s15_euphorbia",
    };
  }
}
