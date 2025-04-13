using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.Modding;
using Timberborn.ModManagerScene;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(ModCodeStarter))]
  [HarmonyPatch("StartMod")]
  static class ModCodeStarterPatch {

    static void Prefix(Mod mod, out DateTime __state) {
      __state = DateTime.Now;
    }

    static void Finalizer(Mod mod, DateTime __state, Exception __exception) {
      var displayName = mod?.DisplayName ?? "Unknown mod";
      var modPath = UserDataSanitizer.Sanitize(mod?.ModDirectory.Path ?? "an unknown location");
      var duration = DateTime.Now - __state;
      if (__exception == null) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + displayName + ": Started from " + modPath + " in " + duration);
      } else {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + displayName + ": Failed to start from " + modPath + " after " + duration);
      }
    }
  }
}
