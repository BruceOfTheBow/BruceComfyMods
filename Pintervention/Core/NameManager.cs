using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Pintervention.Pintervention;
using static WorldGenerator;

namespace Pintervention {
  public class NameManager {
    public static Dictionary<long, string> PlayerNamesById { get; private set; } = new();

    static readonly string _mapPinsName = "World & Player Pins";
    static int _unknownPlayerCount = 0;

    public static void LoadPlayerNames() {
      ReadNamesFromFile();
    }

    static void AddLocalPlayerName() {
      if (!Player.m_localPlayer
          || PlayerNamesById.ContainsKey(Player.m_localPlayer.GetPlayerID())) {
        return;
      }

      PlayerNamesById.Add(Player.m_localPlayer.GetPlayerID(), Player.m_localPlayer.GetPlayerName());
    }

    public static string GetPlayerNameById(long pid) {
      if (pid == 0L) {
        return _mapPinsName;
      }

      if (pid == Player.m_localPlayer.GetPlayerID()) {
        return Player.m_localPlayer.GetPlayerName();
      }

      if (PlayerNamesById.ContainsKey(pid)) {
        return PlayerNamesById[pid];
      }

      return AssignNextUnknownPlayer(pid);
    }

    static string AssignNextUnknownPlayer(long pid) {
      _unknownPlayerCount++;
      string newName = $"Unknown Player {_unknownPlayerCount}";

      if (!PlayerNamesById.ContainsKey(pid)) {
        PlayerNamesById.Add(pid, newName);
      }

      return newName;
    }

    public static void AddPlayerName(long pid, string name) {
      if (PlayerNamesById.ContainsKey(pid) && !PlayerNamesById[pid].Contains("Unknown")) {
        return;
      }

      if (PlayerNamesById.ContainsKey(pid)) {
        PlayerNamesById[pid] = name;
        return;
      }

      PlayerNamesById.Add(pid, name);
    }

    public static string PidNameMapToRow(int hashedPid, string name) {
      return string.Join(
          ",",
          hashedPid.ToString(),
          name
      );
    }

    public static int HashPid(long pid) {
      return $"{pid}".GetStableHashCode();
    }

    public static string GetPath() {
      return Path.Combine(Utils.GetSaveDataPath(FileHelpers.FileSource.Local), "characters", "pinNames");
    }

    public static string GetFilename() {
      return Path.Combine(GetPath(), $"{ZNet.instance.GetWorldUID()}".GetStableHashCode().ToString() + ".csv");
    }

    public static void WriteNamesToFile() {
      if (!ZNet.instance) {
        return;
      }

      Dictionary<long, string> namesToWrite = GetNamesToWrite();

      if (namesToWrite.Count == 0) {
        return;
      }

      if (!Directory.Exists(GetPath())) {
        Directory.CreateDirectory(GetPath());
      }

      using StreamWriter writer = File.CreateText(GetFilename());

      writer.AutoFlush = true;

      foreach (KeyValuePair<long, string> nameByPid in namesToWrite) {
        if (nameByPid.Key == Player.m_localPlayer.GetPlayerID()
            || nameByPid.Value.StartsWith("Unknown")) {
          continue;
        }

        writer.WriteLine(PidNameMapToRow(HashPid(nameByPid.Key), nameByPid.Value));
      }
    }

    public static Dictionary<long, string> GetNamesToWrite() {
      Dictionary<long, string> namesToWrite = new();

      foreach (KeyValuePair<long, string> nameByPid in PlayerNamesById) {
        if (nameByPid.Key == Player.m_localPlayer.GetPlayerID()
            || nameByPid.Value.StartsWith("Unknown")) {
          continue;
        }

        namesToWrite.Add(nameByPid.Key, nameByPid.Value);
      }

      return namesToWrite;
    }

    public static void ReadNamesFromFile() {
      if (!ZNet.instance || ZNet.instance.GetWorldUID().Equals(default)) {
        LogWarning("Could not read saved player names from file as ZNet instance is null.");
        return;
      }

      if (!File.Exists(GetFilename())) {
        Log($"No saved names to load from {GetFilename()}.");
        return;
      }

      Log($"Loading saved player names from {GetFilename()}");

      Dictionary<int, string> loadedNames = new();

      using (var reader = new StreamReader(GetFilename())) {
        while (!reader.EndOfStream) {
          var line = reader.ReadLine();
          var values = line.Split(',');

          int hashedPid = Int32.Parse(values[0]);
          string name = values[1];

          loadedNames.Add(hashedPid, name);
        }
      }

      foreach (long pid in PinOwnerManager.ForeignPinOwners) {
        if (!loadedNames.ContainsKey(HashPid(pid))) {
          continue;
        }

        if (PlayerNamesById.ContainsKey(pid)) {
          continue;
        }

        PlayerNamesById.Add(pid, loadedNames[HashPid(pid)]);
      }
    }
  }
}
