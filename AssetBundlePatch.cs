using System;
using System.IO;
using UnityEngine;
using HarmonyLib;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(AssetBundle))]
  [HarmonyPatch("LoadFromFile")]
  [HarmonyPatch(new Type[] { typeof(string) } )]
  static class AssetBundlePatch {

    static void Prefix(string path, out DateTime __state) {
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + "AssetBundle.LoadFromFile: Loading " + Path.GetFileName(path));
      __state = DateTime.Now;
    }

    static void Postfix(string path, DateTime __state) {
      var duration = DateTime.Now - __state;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + "AssetBundle.LoadFromFile: Loaded in " + duration);
    }
  }
}
