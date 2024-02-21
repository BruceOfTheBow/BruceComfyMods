using System.IO;

namespace Pintervention {
  public static class FileUtils {
    public static string GetPath() {
      return Path.Combine(Utils.GetSaveDataPath(FileHelpers.FileSource.Local), "characters", "pinNames");
    }

    public static string GetFilename() {
      return Path.Combine(GetPath(), $"{ZNet.instance.GetWorldUID()}".GetStableHashCode().ToString() + ".csv");
    }

    public static string GetFilteredFilename() {
      return Path.Combine(GetPath(), $"{ZNet.instance.GetWorldUID()}".GetStableHashCode().ToString() + "_filtered.csv");
    }

    public static int HashPid(long pid) {
      return $"{pid}".GetStableHashCode();
    }
  }
}
