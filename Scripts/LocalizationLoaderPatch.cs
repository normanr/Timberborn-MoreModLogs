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

  static void Finalizer(LocalizationLoader __instance, Exception __exception, string localizationKey) {
    if (__exception == null) return;
    foreach (LocalizationRecord item in __instance.GetDefaultLocalization()) {
      if (item.Text is null) {
        Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"*** Null localization key {item.Id} in {localizationKey}");
      }
    }
  }
}
