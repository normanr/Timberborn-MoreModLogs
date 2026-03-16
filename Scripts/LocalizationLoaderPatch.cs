using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Timberborn.Localization;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(LocalizationLoader))]
[HarmonyPatch(nameof(LocalizationLoader.GetLocalization))]
static class LocalizationLoaderPatch {

  static void Postfix(Dictionary<string, string> __result) {
    foreach (var (key, value) in __result) {
      if (value.Contains("<s>", StringComparison.InvariantCultureIgnoreCase))
      Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"*** Localization {key} contains \"<s>\": {value}");
    }
  }
}
