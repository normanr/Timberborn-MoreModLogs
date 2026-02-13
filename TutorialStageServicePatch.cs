using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Timberborn.TutorialSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch()]
  static class TutorialStageServicePatch {

    static List<string> _warnings;

    [HarmonyPatch("Timberborn.TutorialSystem.TutorialStageService", "GetStage")]
    [HarmonyPrefix()]
    static void GetStagePrefix() {
      _warnings = [];
    }

    static internal void AddWarning(string warning) {
      _warnings?.Add(warning);
    }

    [HarmonyPatch("Timberborn.TutorialSystem.TutorialStageService", "GetStage")]
    [HarmonyFinalizer()]
    static void GetStageFinalizer(string stageId, Exception __exception) {
      if (__exception == null && _warnings.Count == 0) return;
      if (__exception != null) {
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to deserialize building tutorial step for {stageId}");
      }
      if (_warnings.Count > 0) {
        Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"Building tutorial step deserializer for {stageId} generated warnings:");
      }
      foreach (var item in _warnings.GroupBy(k => k)) {
        Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"- {item.Count()} {item.Key}");
      }
      _warnings = null;
    }
  }
}
