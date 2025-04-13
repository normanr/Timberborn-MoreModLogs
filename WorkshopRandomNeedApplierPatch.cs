using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Timberborn.BaseComponentSystem;
using Timberborn.PrefabSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class WorkshopRandomNeedApplierPatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.NeedApplication.WorkshopRandomNeedApplier").Method("Awake");
    }

    static void Postfix(BaseComponent __instance, BaseComponent ____workshopRandomNeedApplierSpec) {
      var workshopRandomNeedApplierSpec = Traverse.Create(____workshopRandomNeedApplierSpec);
      var effects = workshopRandomNeedApplierSpec.Field("_effects");
      var count = effects.Property<int>("Count").Value;
      if (count == 0) {
        var prefabSpec = __instance.GetComponentFast<PrefabSpec>();
        Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + prefabSpec.PrefabName + " WorkshopRandomNeedApplier has no Effects!?!");
      }
    }

  }
}
