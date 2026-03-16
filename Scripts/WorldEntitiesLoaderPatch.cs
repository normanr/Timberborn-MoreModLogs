using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.WorldPersistence;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(EntitiesLoader))]
[HarmonyPatch(nameof(EntitiesLoader.Load))]
[HarmonyPatch([typeof(InstantiatedSerializedEntity)])]
static class WorldEntitiesLoaderPatch {

  static void Finalizer(InstantiatedSerializedEntity serializedEntity, Exception __exception) {
    if (__exception == null) return;
    Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"WorldEntitiesLoader.LoadEntity({serializedEntity.SerializedEntity.TemplateName}) failed with an exception");
  }

}
