using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.BlockObjectTools;
using Timberborn.TemplateSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(BlockObjectTool))]
  [HarmonyPatch(nameof(BlockObjectTool.DescribeTool))]
  static class BlockObjectToolPatch {

    static void Finalizer(BlockObjectTool __instance, Exception __exception) {
      if (__exception == null) return;
      Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to describe {__instance?.Template?.GetSpec<TemplateSpec>()?.TemplateName}");
    }

  }
}