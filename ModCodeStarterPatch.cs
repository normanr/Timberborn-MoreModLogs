using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.Modding;
using Timberborn.ModManagerScene;
using System.Linq;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(ModCodeStarter))]
  [HarmonyPatch("StartMod")]
  static class ModCodeStarterPatch {

    static bool initalized;

    private static void FirstRun(ModRepository modRepository) {
      Debug.Log("Minimum Game Versions:");
      foreach (var mod in modRepository.EnabledMods
                          .OrderBy((mod) => mod.Manifest.MinimumGameVersion.Numeric)) {
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
