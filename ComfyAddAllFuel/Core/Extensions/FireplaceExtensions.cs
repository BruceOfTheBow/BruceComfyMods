namespace AddAllFuel;

public static class FireplaceExtensions {
  public static string GetFuelItemName(this Fireplace fireplace) {
    return fireplace.m_fuelItem ? fireplace.m_fuelItem.m_itemData.m_shared.m_name : string.Empty;
  }

  public static float GetFuel(this Fireplace fireplace) {
    ZNetView netView = fireplace.m_nview;

    if (netView && netView.IsValid()) {
      return netView.m_zdo.GetFloat(ZDOVars.s_fuel);
    }

    return 0f;
  }
}
