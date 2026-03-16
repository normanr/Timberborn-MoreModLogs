using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.TemplateCollectionSystem;
using Timberborn.TemplateSystem;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(TemplateNameMapper))]
[HarmonyPatch(nameof(TemplateNameMapper.TryAddTemplate))]
static class TemplateNameMapperPatch {

  public static void Finalizer(TemplateNameMapper __instance, string templateName, TemplateSpec templateSpec, Exception __exception) {
    if (__exception == null) return;
    if (__instance.TryGetTemplate(templateName, out var templateSpec2)) {
      Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"TemplateNameMapper.TryAddTemplate({templateName}) failed with duplicate templates from:");
      var specService = __instance._templateService._templateCollectionService._specService;
      try {
        var bp2 = Singletons.BlueprintSourceService?.Get(templateSpec2.Blueprint);
        if (bp2 != null) {
          Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"- {bp2?.Path} ({string.Join(", ", bp2?.Sources ?? new())})");
          foreach (var spec in specService.GetSpecs<TemplateCollectionSpec>()) {
            foreach (var blueprint in spec.Blueprints) {
              if (blueprint.Path.Equals(bp2?.Path, StringComparison.InvariantCultureIgnoreCase)) {
                var bp3 = Singletons.BlueprintSourceService?.Get(spec.Blueprint);
                Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"  - {bp3?.Path} ({string.Join(", ", bp3?.Sources ?? new())})");
              }
            }
          }
        } else {
          Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"- mystery location");
        }
      }
      catch {
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"- unknown location");
      }
      try {
        var bp = Singletons.BlueprintSourceService?.Get(templateSpec.Blueprint);
        if (bp != null) {
          Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"- {bp?.Path} ({string.Join(", ", bp?.Sources ?? new())})");
          foreach (var spec in specService.GetSpecs<TemplateCollectionSpec>()) {
            foreach (var blueprint in spec.Blueprints) {
              if (blueprint.Path.Equals(bp?.Path, StringComparison.InvariantCultureIgnoreCase)) {
                var bp3 = Singletons.BlueprintSourceService?.Get(spec.Blueprint);
                Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"  - {bp3?.Path} ({string.Join(", ", bp3?.Sources ?? new())})");
              }
            }
          }
        } else {
          Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"- mystery location");
        }
      }
      catch {
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"- unknown location");
      }
    }
  }

}
