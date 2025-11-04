using System;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Timberborn.BlueprintSystem;
using System.Collections.Generic;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(BlueprintDeserializer))]
  [HarmonyPatch(nameof(BlueprintDeserializer.DeserializeUnsafe))]
  static class BlueprintDeserializerPatch {

    static readonly List<string> _warnings = [];

    static internal void AddWarning(string warning) {
      _warnings.Add(warning);
    }

    static void Finalizer(BlueprintFileBundle blueprintFileBundle, Exception __exception) {
      if (__exception == null && _warnings.Count == 0) return;
      if (__exception != null) {
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to deserialize blueprint for {blueprintFileBundle.Path}");
      } else if (_warnings.Count > 0) {
        Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"Blueprint deserializer for {blueprintFileBundle.Path} generated warnings:");
      }
      foreach (var item in _warnings) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + item);
      }
      _warnings.Clear();
      foreach (var item in blueprintFileBundle.Sources.Zip(blueprintFileBundle.Jsons, (name, text) => $"JSON from {name}: {text}")) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + item);
      }
    }
  }
}
