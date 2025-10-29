using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Timberborn.NeedApplication;
using Timberborn.TemplateSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class WorkshopRandomNeedApplierPatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.NeedApplication.WorkshopRandomNeedApplier").Method("Awake");
    }

    static void Postfix(WorkshopRandomNeedApplier __instance, INeedEffectsSpec ____workshopRandomNeedApplierSpec) {
      if (____workshopRandomNeedApplierSpec.Effects.IsEmpty) {
        var name = __instance?.GetComponent<TemplateSpec>()?.TemplateName ?? "unknown";
        Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + name + " WorkshopRandomNeedApplier has no Effects!?!");
      }
    }

  }
}
