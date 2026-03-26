using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Timberborn.ModManagerScene;

namespace Mods.MoreModLogs;

internal class ModStarter : IModStarter {

  internal const string ModName = "More Mod Logs";

  public void StartMod(IModEnvironment modEnvironment) {
    var start = DateTime.Now;
    var modPath = UserDataSanitizer.Sanitize(modEnvironment?.ModPath ?? "an unknown location");
    try {
      var harmony = new Harmony(ModName);
      harmony.PatchAll();
    }
    catch (Exception ex) {
      var duration = DateTime.Now - start;
      Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"{ModName}: Failed to start from {modPath} after {duration}");
      throw new TargetInvocationException($"{ModName}: Failed to start from {modPath}", ex);
    }
    {
      var duration = DateTime.Now - start;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"{ModName}: Started from {modPath} in {duration}");
    }
  }
}
