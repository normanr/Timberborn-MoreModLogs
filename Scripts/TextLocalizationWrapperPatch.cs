using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using Timberborn.Localization;

namespace Mods.MoreModLogs;

[HarmonyPatch]
static class TextLocalizationWrapperPatch {

  public static IEnumerable<MethodBase> TargetMethods() {
    yield return SymbolExtensions.GetMethodInfo(() => default(TextLocalizationWrapper).WarnIfUpdating<int, string, object>);
    yield return SymbolExtensions.GetMethodInfo(() => default(TextLocalizationWrapper).WarnIfUpdating<int, int, object>);
  }

  static void Prefix(TextLocalizationWrapper __instance) {
    if (__instance._textLocalization == null) return;
    var args = __instance._textLocalization.GetType().GenericTypeArguments;
    Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"TextLocalizationWrapper: TextTemplate = {__instance._textTemplate}, Previous types: {args[0]} {args[1]}");
  }

}
