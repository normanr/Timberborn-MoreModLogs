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
      var duration = DateTime.Now - __state;
      if (__exception == null) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + (mod?.DisplayName ?? "Unknown mod") + ": Started from " + (mod?.ModDirectory.Path ?? "an unknown location") + " in " + duration);
      } else {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + (mod?.DisplayName ?? "Unknown mod") + ": Failed to start from " + (mod?.ModDirectory.Path ?? "an unknown location") + " after " + duration);
      }
    }
  }
}
