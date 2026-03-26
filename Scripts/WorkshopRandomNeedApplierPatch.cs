using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.NeedApplication;
using Timberborn.TemplateSystem;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(WorkshopRandomNeedApplier))]
[HarmonyPatch(nameof(WorkshopRandomNeedApplier.Awake))]
static class WorkshopRandomNeedApplierPatch {

  static void Postfix(WorkshopRandomNeedApplier __instance) {
    if (__instance._workshopRandomNeedApplierSpec.Effects.IsEmpty) {
      var name = __instance?.GetComponent<TemplateSpec>()?.TemplateName ?? "unknown";
      Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"{name} WorkshopRandomNeedApplier has no Effects!?!");
    }
  }

}
