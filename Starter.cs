using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.ModManagerScene;

namespace Mods.SteamUpdateButtons {
  internal class ModStarter : IModStarter {

    public void StartMod() {
      StartMod(null);
    }

    public void StartMod(IModEnvironment modEnvironment) {
      Debug.Log(DateTime.Now.ToString("HH:mm:ss.fff") + " Starting More Mod Logs from " + (modEnvironment?.ModPath ?? "an unknown location"));
      var start = DateTime.Now;
      var harmony = new Harmony("More Mod Logs");
      harmony.PatchAll();
      var duration = DateTime.Now - start;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss.fff") + " Started More Mod Logs in " + duration);
    }
  }
}
