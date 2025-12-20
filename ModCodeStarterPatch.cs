using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Timberborn.Modding;
using Timberborn.ModManagerScene;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(ModCodeStarter))]
  [HarmonyPatch("StartMod")]
  static class ModCodeStarterPatch {

    static bool initalized;

    private static void FirstRun(ModRepository modRepository) {
      Debug.Log("Minimum Game Versions:");
      var comparer = Comparer<Timberborn.Versioning.Version>.Create(
        (x, y) => x.IsEqualOrHigherThan(y) ? y.IsEqualOrHigherThan(x) ? x.Numeric.Length.CompareTo(y.Numeric.Length) : 1 : -1
      );
      foreach (var mod in modRepository.EnabledMods
                          .OrderBy((mod) => mod.Manifest.MinimumGameVersion, comparer)) {
        Debug.Log("- " + mod.Manifest.MinimumGameVersion.Numeric + " - " + mod.Manifest.Name);
      }
    }

    static void Prefix(Mod mod, ModRepository ____modRepository, out DateTime __state) {
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
