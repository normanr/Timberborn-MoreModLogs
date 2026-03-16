using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.Planting;
using Timberborn.PlantingUI;
using Timberborn.TemplateSystem;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(PlantingToolButtonFactory))]
[HarmonyPatch(nameof(PlantingToolButtonFactory.GetPlanterBuildingName))]
static class PlantingToolPatch {

  static void Finalizer(Exception __exception, PlantableSpec plantableSpec) {
    if (__exception == null) return;
    var templateSpec = plantableSpec.GetSpec<TemplateSpec>();
    Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"PlantingToolButtonFactory.GetPlanterBuildingName({templateSpec.TemplateName} looking for {plantableSpec.ResourceGroup}) failed with an exception");
  }

}
