using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.GameFactionSystem;
using System.Linq;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(FactionNeedService))]
  static class FactionNeedServicePatch {

    [HarmonyPatch(nameof(FactionNeedService.Load))]
    [HarmonyPostfix()]
    static void LoadPostfix(FactionNeedService __instance) {
      foreach (var group in __instance.Needs.GroupBy(n => n.Id).Where(l => l.Count() > 1)) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"*** {group.Key} need is defined multiple times:");
        foreach (var need in group) {
          try {
            var bundle = Singletons.BlueprintSourceService?.Get(need.Unmodified.Blueprint);
            Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"- {need.Blueprint.Name} in {bundle.Path} ({string.Join(", ", bundle.Sources)})");
          }
          catch {
            Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"- {need.Blueprint.Name} in unknown location");
          }
        }
      }
    }

    [HarmonyPatch(nameof(FactionNeedService.GetBeaverOrBotNeedById))]
    [HarmonyPrefix()]
    static void GetBeaverOrBotNeedByIdPrefix(out DateTime __state) {
      __state = DateTime.Now;
    }

    [HarmonyPatch(nameof(FactionNeedService.GetBeaverOrBotNeedById))]
    [HarmonyFinalizer()]
    static void GetBeaverOrBotNeedByIdFinalizer(string id, DateTime __state, Exception __exception) {
      if (__exception == null) return;
      var duration = DateTime.Now - __state;
      Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"FactionNeedService.GetBeaverOrBotNeedById({id}) failed after {duration}");
    }
  }
}
