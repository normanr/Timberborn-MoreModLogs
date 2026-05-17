using System;
using System.IO;
using UnityEngine;
using HarmonyLib;
using Timberborn.ModdingAssets;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(ModSystemFileProvider<UnityEngine.Object>))]
static class ModSystemFileProviderPatch {
  [HarmonyPatch(nameof(ModSystemFileProvider<>.GetMetadata))]
  static void Finalizer(FileInfo fileInfo, Exception __exception) {
    if (__exception == null) return;
    Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to read: {UserDataSanitizer.Sanitize(fileInfo.FullName)}.meta.json");
  }
}
