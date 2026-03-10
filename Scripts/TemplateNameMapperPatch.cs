using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.TemplateSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(TemplateNameMapper), "TryAddTemplate")]
  static class TemplateNameMapperPatch {

    public static void Finalizer(TemplateNameMapper __instance, string templateName, TemplateSpec templateSpec, Exception __exception) {
      if (__exception == null) return;
      if (__instance.TryGetTemplate(templateName, out var templateSpec2)) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"TemplateNameMapper.TryAddTemplate({templateName}) failed with duplicate templates from:");
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"- {SpecServicePatch.BlueprintSourceService?.Get(templateSpec2.Blueprint)?.Path}");
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"- {SpecServicePatch.BlueprintSourceService?.Get(templateSpec.Blueprint)?.Path}");
      }
    }

  }
}