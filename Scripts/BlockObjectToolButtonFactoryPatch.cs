using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.BlockObjectToolsUI;
using Timberborn.BlockSystem;
using Timberborn.TemplateSystem;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(BlockObjectToolButtonFactory))]
[HarmonyPatch(nameof(BlockObjectToolButtonFactory.CreateTool))]
static class BlockObjectToolButtonFactoryPatch {

  static void Finalizer(PlaceableBlockObjectSpec template, Exception __exception) {
    if (__exception == null) return;
    try {
      if (template == null) {
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + "Failed to create BlockObjectTool for null PlaceableBlockObjectSpec");
        return;
      }
      var bundle = Singletons.BlueprintSourceService.Get(template.Blueprint);
      Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to create BlockObjectTool for {template?.GetSpec<TemplateSpec>()?.TemplateName} in {bundle.Path} ({string.Join(", ", bundle.Sources)})");
    }
    catch {
    }
    Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to create BlockObjectTool for {template?.GetSpec<TemplateSpec>()?.TemplateName} in unknown location");
  }

}
