using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddAllFuel.Extensions {
  public static class CookingStationExtensions {
    public static string GetFuelName(this CookingStation cookingStation) {
      if (cookingStation.m_fuelItem == null) {
        return null;
      }

      return cookingStation.m_fuelItem.m_itemData.m_shared.m_name;
    }

    public static int GetFreeSlots(this CookingStation cookingStation) {
      int freeSlots = 0;

      for (int i = 0; i < cookingStation.m_slots.Length; i++) {
        if (cookingStation.m_nview.GetZDO().GetString("slot" + i.ToString(), "") != "") {
          continue;
        }

        freeSlots++;
      }

      return freeSlots;
    }

    public static int GetDoneItemCount(this CookingStation cookingStation) {
      int doneItemCount = 0;

      for (int i = 0; i < cookingStation.m_slots.Length; i++) {
        cookingStation.GetSlot(i, out string text, out float num, out CookingStation.Status status);
        if (!cookingStation.IsItemDone(text)) {
          continue;
        }

        doneItemCount++;
      }

      return doneItemCount;
    }
  }
}
