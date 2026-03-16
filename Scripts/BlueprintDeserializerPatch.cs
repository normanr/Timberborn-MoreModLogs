using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Timberborn.BlueprintSystem;
using Timberborn.SerializationSystem;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(BlueprintDeserializer))]
static class BlueprintDeserializerPatch {

  static List<string> _warnings;

  [HarmonyPatch(nameof(BlueprintDeserializer.DeserializeUnsafe))]
  [HarmonyPrefix()]
  static void DeserializeUnsafePrefix() {
    _warnings = [];
  }

  static internal void AddWarning(string warning) {
    _warnings?.Add(warning);
  }

  [HarmonyPatch(nameof(BlueprintDeserializer.DeserializeUnsafe))]
  [HarmonyFinalizer()]
  static void DeserializeUnsafeFinalizer(BlueprintFileBundle blueprintFileBundle, Exception __exception) {
    if (__exception == null && _warnings.Count == 0) return;
    var bundleName = blueprintFileBundle.Path + " (" + string.Join(", ", blueprintFileBundle.Sources) + ")";
    if (__exception != null) {
      Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to deserialize blueprint for {bundleName}");
    }
    if (_warnings.Count > 0) {
      Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"Blueprint deserializer for {bundleName} generated warnings:");
    }
    foreach (var item in _warnings.GroupBy(k => k)) {
      Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"- {item.Count()} {item.Key}");
    }
    _warnings = null;
  }

  [HarmonyPatch(nameof(BlueprintDeserializer.DeserializeSpecs))]
  [HarmonyPrefix()]
  static void DeserializeSpecsPrefix(SerializedObject serializedObject) {
    foreach (string item in serializedObject.Properties())
    {
      var obj = serializedObject.GetSerialized(item);
      if (obj is not SerializedObject) {
        AddWarning($"*** {item} has the wrong type: {obj.GetType()} != SerializedObject");
      }
    }
  }
}
