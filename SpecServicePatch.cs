using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Linq;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class SpecServicePatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.BlueprintSystem.SpecService").Method("DeserializeBlueprint");
    }

    static void Finalizer(IGrouping<string, TextAsset> jsons, Exception __exception) {
      if (__exception == null) return;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + "Failed to deserialize blueprint for " + jsons.Key);
      foreach (var item in jsons) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + item.name + ":\n" + item.text);
      }
    }
  }
}
