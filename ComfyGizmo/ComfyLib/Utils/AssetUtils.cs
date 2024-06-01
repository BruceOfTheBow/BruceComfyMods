namespace ComfyLib;

using System.Collections.Generic;
using System.IO;
using System.Reflection;

using SoftReferenceableAssets;

using UnityEngine;

public static class AssetUtils {
  public static readonly string StandardShader = "7e6bbee7a32b746cb9396cd890ce7189";

  public static readonly AssetCache<Shader> ShaderCache = new();
  public static Shader GetShader(string guid) => ShaderCache.GetAsset(StandardShader);

  public static T LoadAsset<T>(string resourceName, string assetName) where T : Object {
    AssetBundle bundle =
        AssetBundle.LoadFromMemory(LoadResourceFromAssembly(Assembly.GetExecutingAssembly(), resourceName));
    T asset = bundle.LoadAsset<T>(assetName);
    bundle.UnloadAsync(unloadAllLoadedObjects: false);

    return asset;
  }

  public static byte[] LoadResourceFromAssembly(Assembly assembly, string resourceName) {
    Stream stream = assembly.GetManifestResourceStream(resourceName);

    byte[] data = new byte[stream.Length];
    stream.Read(data, offset: 0, count: (int) stream.Length);

    return data;
  }
}

public sealed class AssetCache<T> where T : Object {
  readonly Dictionary<string, T> _cache = [];

  public T GetAsset(string guid) {
    if (!_cache.TryGetValue(guid, out T asset) && AssetID.TryParse(guid, out AssetID assetId)) {
      SoftReference<T> reference = new(assetId);
      reference.Load();
      asset = reference.Asset;
    }

    return asset;
  }
}
