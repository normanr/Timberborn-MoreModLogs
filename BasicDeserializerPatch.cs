using System;
using System.Linq;
using HarmonyLib;
using Timberborn.BlueprintSystem;
using Timberborn.SerializationSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(BasicDeserializer))]
  [HarmonyPatch(nameof(BasicDeserializer.Deserialize))]
  static class BasicDeserializerPatch {

    static void Postfix(SerializedObject serializedObject, Type type) {
      foreach (string property in serializedObject.Properties().Except(type.GetSerializedProperties().Select(p => p.Name))) {
        BlueprintDeserializerPatch.AddWarning($"- {type.Name} contains unused field {property}");
      }
    }

  }
}
