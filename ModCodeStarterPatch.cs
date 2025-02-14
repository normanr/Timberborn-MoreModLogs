﻿using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.Modding;
using Timberborn.ModManagerScene;

namespace Mods.SteamUpdateButtons {

  [HarmonyPatch(typeof(ModCodeStarter))]
  [HarmonyPatch("StartMod")]
  static class ModCodeStarterPatch {
    static void Prefix(Mod mod, out DateTime __state) {
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + mod.DisplayName + ": Starting from " + (mod?.ModDirectory.Path ?? "an unknown location"));
      __state = DateTime.Now;
    }

    static void Postfix(Mod mod, DateTime __state) {
      var duration = DateTime.Now - __state;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + mod.DisplayName + ": Started in " + duration);
    }
  }
}
