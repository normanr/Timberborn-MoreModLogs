using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.ModManagerScene;

namespace Mods.MoreModLogs {
  internal class ModStarter : IModStarter {

    internal const string ModName = "More Mod Logs";

    public void StartMod() {
      StartMod(null);
    }

    public void StartMod(IModEnvironment modEnvironment) {
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + ModName + ": Starting from " + (modEnvironment?.ModPath ?? "an unknown location"));
      var start = DateTime.Now;
      var harmony = new Harmony(ModName);
      harmony.PatchAll();
      var duration = DateTime.Now - start;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + ModName + ": Started in " + duration);
    }
  }
}
