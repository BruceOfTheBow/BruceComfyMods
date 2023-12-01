using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddAllFuel.Extensions {
  public static class SmelterExtensions {
    public static string GetFuelName(this Smelter smelter) {
      if (smelter.m_fuelItem == null) {
        return null;
      }

      return smelter.m_fuelItem.m_itemData.m_shared.m_name;
    }
  }
}
