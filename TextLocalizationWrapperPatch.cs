using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class TextLocalizationWrapperPatch {

    public static IEnumerable<MethodBase> TargetMethods() {
      var tlw = AccessTools.TypeByName("Timberborn.Localization.TextLocalizationWrapper");
      yield return tlw.Method("WarnIfUpdating", generics: [typeof(int), typeof(string), typeof(object)]);
      yield return tlw.Method("WarnIfUpdating", generics: [typeof(int), typeof(int), typeof(object)]);
    }

    static void Prefix(string ____textTemplate, object ____textLocalization) {
      if (____textLocalization != null) {
        var args = ____textLocalization.GetType().GenericTypeArguments;
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"TextLocalizationWrapper: TextTemplate = {____textTemplate}, Previous types: {args[0]} {args[1]}");
      }
    }

  }
}