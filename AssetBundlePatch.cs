using System;
using System.IO;
using UnityEngine;
using HarmonyLib;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(AssetBundle))]
  [HarmonyPatch(nameof(AssetBundle.LoadFromFile))]
  [HarmonyPatch(new Type[] { typeof(string) } )]
  static class AssetBundlePatch {

    static void Prefix(string path, out DateTime __state) {
      __state = DateTime.Now;
    }

    static void Finalizer(string path, DateTime __state, AssetBundle __result, Exception __exception) {
      var duration = DateTime.Now - __state;
      if (__exception == null && __result != null) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + "AssetBundle.LoadFromFile(" + Path.GetFileName(path) + ") executed in " + duration);
      } else {
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + "AssetBundle.LoadFromFile(" + Path.GetFileName(path) + ") failed after " + duration);
      }
    }
  }
}
