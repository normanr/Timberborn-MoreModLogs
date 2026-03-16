using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.GameFactionSystem;
using Timberborn.NeedSpecs;
using System.Runtime.CompilerServices;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(NeedModificationService))]
[HarmonyPatch(nameof(NeedModificationService.ModifyIfEligible))]
static class NeedModificationServicePatch {

  internal static ConditionalWeakTable<NeedSpec, WeakReference<NeedSpec>> _needSources = [];

  static void Postfix(NeedSpec needSpec, NeedSpec __result) {
    _needSources.Add(__result, new(needSpec));
  }

  extension(NeedSpec needSpec) {
    public NeedSpec Unmodified {
      get {
        if (_needSources.TryGetValue(needSpec, out var wr) && wr.TryGetTarget(out var result)) {
          return result;
        }
        Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"Missing unmodified need for {needSpec.Id}");
        return needSpec;
      }
    }
  }

}
