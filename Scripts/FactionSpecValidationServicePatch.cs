using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.FactionValidators;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(FactionSpecValidationService))]
[HarmonyPatch(nameof(FactionSpecValidationService.Load))]
static class FactionSpecValidationServicePatch {

  static void Postfix(FactionSpecValidationService __instance) {
    foreach (var factionSpec in __instance._factionSpecService.Factions) {
      foreach (var factionSpecValidator in __instance._factionSpecValidators) {
        if (!factionSpecValidator.IsValid(factionSpec, out var errorMessage)) {
          Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"{factionSpecValidator.GetType().Name} failed for Faction {factionSpec.Id}: {errorMessage}");
        }
      }
    }
  }
}
