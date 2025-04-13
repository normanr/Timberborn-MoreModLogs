using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.ModManagerScene;

namespace Mods.MoreModLogs {
  internal class ModStarter : IModStarter {

    const string ModName = "More Mod Logs";

    public void StartMod() {
      StartMod(null);
    }

    public void StartMod(IModEnvironment modEnvironment) {
      var start = DateTime.Now;
      var modPath = UserDataSanitizer.Sanitize(modEnvironment?.ModPath ?? "an unknown location");
      try {
        var harmony = new Harmony(ModName);
        harmony.PatchAll();
      }
      catch {
        var duration = DateTime.Now - start;
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + ModName + ": Failed to start from " + modPath + " after " + duration);
        throw;
      }
      {
        var duration = DateTime.Now - start;
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + ModName + ": Started from " + modPath + " in " + duration);
      }
    }
  }
}
