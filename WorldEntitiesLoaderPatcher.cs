using System;
using System.Reflection;
using UnityEngine;
using HarmonyLib;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class WorldEntitiesLoaderPatcher {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.WorldPersistence.WorldEntitiesLoader").Method("LoadEntity");
    }

    static void Finalizer(object serializedEntity, Exception __exception) {
      if (__exception == null) return;
      var templateName = Traverse.Create(serializedEntity).Property<string>("TemplateName").Value;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + "WorldEntitiesLoader.LoadEntity(" + templateName + ") failed with an exception");
    }

  }
}
