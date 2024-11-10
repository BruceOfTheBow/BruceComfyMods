namespace AddAllFuel;

using System.Collections.Generic;

public static class SmelterManager {
  public static readonly HashSet<string> ExcludeCookableItems = [];

  public static void SetExcludeCookableItems(string[] itemNames) {
    ExcludeCookableItems.Clear();
    ExcludeCookableItems.UnionWith(itemNames);
  }
}
