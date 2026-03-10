using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Timberborn.FactionSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class FactionSpecValidationServicePatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.FactionValidators.FactionSpecValidationService").Method("Load");
    }

    static void Postfix(FactionSpecService ____factionSpecService, IEnumerable ____factionSpecValidators) {
      var isValid = AccessTools.TypeByName("Timberborn.FactionValidators.IFactionSpecValidator").Method("IsValid");
      foreach (var factionSpec in ____factionSpecService.Factions) {
        foreach (object factionSpecValidator in ____factionSpecValidators) {
          object[] arguments = [factionSpec, ""];
          if (!(bool)isValid.Invoke(factionSpecValidator, arguments)) {
            Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"{factionSpecValidator.GetType().Name} failed for Faction {factionSpec.Id}: {arguments[1]}");
          }
        }
      }
    }
  }
}
