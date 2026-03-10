using System;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using Timberborn.SerializationSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(SerializedObjectReaderWriter))]
  static class SerializedObjectReaderWriterPatch {

    [HarmonyPatch(nameof(SerializedObjectReaderWriter.ReadJson))]
    [HarmonyPatch([typeof(string)])]
    [HarmonyFinalizer]
    static void ReadJsonFinalizer(string text, Exception __exception) {
      if (__exception == null) return;
      Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to read json: {text}");
    }

    [HarmonyPatch(nameof(SerializedObjectReaderWriter.ReadJsons))]
    [HarmonyFinalizer]
    static void ReadJsonsFinalizer(IEnumerable<string> texts, Exception __exception) {
      if (__exception == null) return;
      Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to read jsons:");
      foreach (var text in texts) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + text);
      }
    }
  }
}
