using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Linq;
using Timberborn.BlueprintSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class SpecServicePatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.BlueprintSystem.BlueprintDeserializer").Method("DeserializeUnsafe");
    }

    static void Finalizer(BlueprintFileBundle blueprintFileBundle, Exception __exception) {
      if (__exception == null) return;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + "Failed to deserialize blueprint for " + blueprintFileBundle.Path);
      foreach (var item in blueprintFileBundle.Sources.Zip(blueprintFileBundle.Jsons, (name, text) => name + ":\n" + text)) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + item);
      }
    }
  }
}
