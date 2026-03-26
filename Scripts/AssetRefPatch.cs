using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.BlueprintSystem;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(AssetRef<UnityEngine.Object>))]
public static class AssetRefPatch {

  [HarmonyPatch(MethodType.Constructor)]
  [HarmonyPatch([typeof(string), typeof(Lazy<UnityEngine.Object>)])]
  public static void Postfix(string path, ref Lazy<UnityEngine.Object> ____lazyAsset) {
    var lazyAsset = ____lazyAsset;
    ____lazyAsset = new(() => {
      var startTime = DateTime.Now;
      try {
        return lazyAsset.Value;
      }
      catch (Exception) {
        var duration = DateTime.Now - startTime;
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"AssetRef({path}).Value failed after {duration}");
        throw;
      }
    });
  }
}
