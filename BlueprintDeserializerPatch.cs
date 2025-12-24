using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.BlueprintSystem;
using System.Collections.Generic;
using System.Linq;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(BlueprintDeserializer))]
  [HarmonyPatch(nameof(BlueprintDeserializer.DeserializeUnsafe))]
  static class BlueprintDeserializerPatch {

    static List<string> _warnings;

    static void Prefix() {
      _warnings = [];
    }

    static internal void AddWarning(string warning) {
      _warnings?.Add(warning);
    }

    static void Finalizer(BlueprintFileBundle blueprintFileBundle, Exception __exception) {
      if (__exception == null && _warnings.Count == 0) return;
      var bundleName = blueprintFileBundle.Path + " (" + string.Join(", ", blueprintFileBundle.Sources) + ")";
      if (__exception != null) {
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to deserialize blueprint for {bundleName}");
      } else if (_warnings.Count > 0) {
        Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"Blueprint deserializer for {bundleName} generated warnings:");
      }
      foreach (var item in _warnings.GroupBy(k => k)) {
        Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"- {item.Count()} {item.Key}");
      }
      _warnings = null;
    }
  }
}
