using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Timberborn.BlueprintSystem;
using Timberborn.SerializationSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(BasicDeserializer))]
  static class BasicDeserializerPatch {

    static readonly List<string> _context = [];

    [HarmonyPatch(nameof(BasicDeserializer.Deserialize))]
    [HarmonyPrefix]
    static void DeserializePrefix(Type type) {
      if (_context.Count == 0) {
        _context.Add(type.Name);
      }
    }

    [HarmonyPatch(nameof(BasicDeserializer.Deserialize))]
    [HarmonyPostfix]
    static void DeserializePostfix(SerializedObject serializedObject, Type type) {
      foreach (string property in serializedObject.Properties().Except(type.GetSerializedProperties().Select(p => p.Name))) {
        BlueprintDeserializerPatch.AddWarning($"{type.Name} at {string.Join(".", _context)} contains unused field {property}");
      }
    }

    [HarmonyPatch(nameof(BasicDeserializer.Deserialize))]
    [HarmonyFinalizer]
    static void DeserializeFinalizer(Type type) {
      if (_context.Count == 1) {
        _context.RemoveAt(0);
      }
    }

    [HarmonyPatch("GetValue")]
    [HarmonyPrefix]
    static void GetValuePrefix(PropertyInfo serializedProperty) {
      _context.Add(serializedProperty.Name);
    }

    [HarmonyPatch("GetValue")]
    [HarmonyFinalizer]
    static void GetValueFinalizer(PropertyInfo serializedProperty) {
      var top = _context[^1];
      _context.RemoveAt(_context.Count - 1);
      if (top != serializedProperty.Name) {
        Debug.LogError($"Invalid top of context: {top} at {string.Join(".", _context)}");
      }
    }
  }
}
