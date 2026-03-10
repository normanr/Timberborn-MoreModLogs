using System;
using System.IO;
using UnityEngine;
using HarmonyLib;
using Timberborn.GameFactionSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(FactionNeedService))]
  [HarmonyPatch(nameof(FactionNeedService.GetBeaverOrBotNeedById ))]
  [HarmonyPatch([typeof(string)] )]
  static class FactionNeedServicePatch {

    static void Prefix(out DateTime __state) {
      __state = DateTime.Now;
    }

    static void Finalizer(string id, DateTime __state, Exception __exception) {
      if (__exception == null) return;
      var duration = DateTime.Now - __state;
      Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"FactionNeedService.GetBeaverOrBotNeedById({id}) failed after {duration}");
    }
  }
}
