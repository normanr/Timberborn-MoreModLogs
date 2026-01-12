using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Timberborn.WorldPersistence;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class WorldEntitiesLoaderPatcher {

    public static MethodBase TargetMethod() {
      var el = AccessTools.TypeByName("Timberborn.WorldPersistence.EntitiesLoader")  // >= 1.0.5.0
        ?? AccessTools.TypeByName("Timberborn.WorldPersistence.WorldEntitiesLoader");  // < 1.0.5.0
      return el.Method("LoadEntity")  // < 1.0.6.0
        ?? el.Method("Load", [typeof(InstantiatedSerializedEntity)]);  // >= 1.0.7.0
    }

    static void Finalizer(object serializedEntity, Exception __exception) {
      if (__exception == null) return;
      var templateName = Traverse.Create(serializedEntity).Property<string>("TemplateName").Value;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + "WorldEntitiesLoader.LoadEntity(" + templateName + ") failed with an exception");
    }

  }
}
