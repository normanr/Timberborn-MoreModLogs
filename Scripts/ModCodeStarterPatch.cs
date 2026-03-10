using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using HarmonyLib;
using Timberborn.Modding;
using Timberborn.ModManagerScene;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(ModCodeStarter))]
  [HarmonyPatch("StartMod")]
  static class ModCodeStarterPatch {

    private static readonly string AssetBundleDirectory = "AssetBundles";

    private static readonly string WinAssetBundleSuffix = "_win";

    private static readonly string MacAssetBundleSuffix = "_mac";

    [Flags]
    private enum AssetBundleSuffix {
      Win = 1,
      Mac = 2,
    }

    private static bool initalized;

    private static string TrimSuffix(this string s, string suffix) => s.EndsWith(suffix) ? s[..^suffix.Length] : s;

    private static void FirstRun(ModRepository modRepository) {
      foreach (var grp in modRepository.EnabledMods.GroupBy(m => m.Manifest.Id)) {
        if (grp.Count() == 1) continue;
        Debug.LogWarning(grp.Key + " is loaded more than once:");
        foreach (var mod in grp) {
          Debug.Log($"- {UserDataSanitizer.Sanitize(mod.ModDirectory.Path)}");
        }
      }
      foreach (var mod in modRepository.EnabledMods) {
        var files = GetAllAssetBundleFiles(mod.ModDirectory.Directory).GroupBy(
          f => Path.GetFileNameWithoutExtension(f.Name).TrimSuffix(WinAssetBundleSuffix).TrimSuffix(MacAssetBundleSuffix),
          f => f.Name.EndsWith(WinAssetBundleSuffix) ? AssetBundleSuffix.Win : f.Name.EndsWith(MacAssetBundleSuffix) ? AssetBundleSuffix.Mac : 0);
        foreach (var file in files) {
          var flags = file.Aggregate((x, y) => x | y);
          if (flags == AssetBundleSuffix.Win) {
            Debug.LogWarning($"Not found: {Path.Combine(UserDataSanitizer.Sanitize(mod.ModDirectory.Path), AssetBundleDirectory, file.Key)}_mac");
          } else if (flags == AssetBundleSuffix.Mac) {
            Debug.LogWarning($"Not found: {Path.Combine(UserDataSanitizer.Sanitize(mod.ModDirectory.Path), AssetBundleDirectory, file.Key)}_win");
          }
        }
      }
      Debug.Log("Minimum Game Versions:");
      var comparer = Comparer<Timberborn.Versioning.Version>.Create(
        (x, y) => x.IsEqualOrHigherThan(y) ? y.IsEqualOrHigherThan(x) ? x.Numeric.Length.CompareTo(y.Numeric.Length) : 1 : -1
      );
      foreach (var mod in modRepository.EnabledMods
                          .OrderBy((mod) => mod.Manifest.MinimumGameVersion, comparer)) {
        Debug.Log($"- {mod.Manifest.MinimumGameVersion.Numeric} - {mod.DisplayName}");
      }
    }

    private static IEnumerable<FileInfo> GetAllAssetBundleFiles(DirectoryInfo modDirectory) {
      var directoryInfo = new DirectoryInfo(Path.Combine(modDirectory.FullName, AssetBundleDirectory));
      if (!directoryInfo.Exists) {
        yield break;
      }
      FileInfo[] files = directoryInfo.GetFiles();
      foreach (FileInfo fileInfo in files) {
        if (IsNotManifestFile(fileInfo)) {
          yield return fileInfo;
        }
      }
    }

    private static bool IsNotManifestFile(FileInfo fileInfo) {
      return fileInfo.Extension != ".manifest";
    }

    static void Prefix(ModRepository ____modRepository, out DateTime __state) {
      if (!initalized) {
        FirstRun(____modRepository);
        initalized = true;
      }
      __state = DateTime.Now;
    }

    static void Finalizer(Mod mod, DateTime __state, Exception __exception) {
      var displayName = mod?.DisplayName ?? "Unknown mod";
      var modPath = UserDataSanitizer.Sanitize(mod?.ModDirectory.Path ?? "an unknown location");
      var duration = DateTime.Now - __state;
      if (__exception == null) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + displayName + ": Started from " + modPath + " in " + duration);
      } else {
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + displayName + ": Failed to start from " + modPath + " after " + duration);
      }
    }
  }
}
