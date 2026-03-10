using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.TemplateSystem;
using Timberborn.Workshops;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(Manufactory))]
  [HarmonyPatch(nameof(Manufactory.Load))]
  static class ManufactoryPatch {

    static void Finalizer(Exception __exception, Manufactory __instance) {
      if (__exception == null) return;
      var name = __instance?.GetComponent<TemplateSpec>()?.TemplateName ?? "unknown";
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"Manufactory.Load({name}) failed with an exception");
      foreach (var recipe in __instance.ProductionRecipes) {
        if (recipe.BackwardCompatibleIds.IsDefault) {
          Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + "  " + recipe.Id + " is missing BackwardCompatibleIds");
        }
      }
    }

  }
}
