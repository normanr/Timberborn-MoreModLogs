using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class LocalizationLoaderPatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.Localization.LocalizationLoader").Method("GetLocalization");
    }

    static void Postfix(Dictionary<string, string> __result) {
      foreach (var (key, value) in __result) {
        if (value.Contains("<s>", StringComparison.InvariantCultureIgnoreCase))
        Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"*** Localization {key} contains \"<s>\": {value}");
      }
    }
  }
}
