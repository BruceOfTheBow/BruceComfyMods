using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantThings.Core {
  static class Requirements {
    public static Dictionary<string, int> WoodRequired = new Dictionary<string, int>() {
      { "$custompiece_log1", 5 },
      { "$custompiece_log2", 5 },
      { "$custompiece_log3", 5 },
      { "$custompiece_log4", 5 },
      { "$custompiece_log5", 5 },
      { "$custompiece_stumptable", 10 },
      { "$custompiece_grassblock_medium", 5 },
      { "$custompiece_grassblock_large", 10 },
      { "$custompiece_dirtblock_medium", 5 },
      { "$custompiece_dirtblock_large", 10 },
      { "$custompiece_blackpine", 10 }
    };
  }
}
