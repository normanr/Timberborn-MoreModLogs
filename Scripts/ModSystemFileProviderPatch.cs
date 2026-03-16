using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Timberborn.ModdingAssets;

namespace Mods.MoreModLogs;

[HarmonyPatch]
static class ModSystemFileProviderPatch {

  public static MethodBase TargetMethod() {
    return SymbolExtensions.GetMethodInfo(() => default(ModSystemFileProvider<UnityEngine.Object>).GetMetadata);
  }

  static void Finalizer(FileInfo fileInfo, Exception __exception) {
    if (__exception == null) return;
    Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to read: {UserDataSanitizer.Sanitize(fileInfo.FullName)}.meta.json");
  }
}
