using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using HarmonyLib;
using Timberborn.BlueprintSystem;
using Timberborn.SerializationSystem;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class SpecServicePatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.BlueprintSystem.SpecService").Method("Load");
    }

    public static void Prefix(BlueprintSourceService ____blueprintSourceService) {
      Singletons.BlueprintSourceService = ____blueprintSourceService;
    }

    public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions) {
      List<CodeInstruction> newInstructions = [];
      List<CodeInstruction> neighboredValueLoads = [];
      foreach (var instruction in instructions) {
        // C# serializedObject = _serializedObjectReaderWriter.ReadJsons(blueprintBundle.Jsons);
        // IL callvirt instance valuetype [System.Collections.Immutable]System.Collections.Immutable.ImmutableArray`1<string> Timberborn.BlueprintSystem.BlueprintFileBundle::get_Jsons()
        // IL box valuetype [System.Collections.Immutable]System.Collections.Immutable.ImmutableArray`1<string>
        // IL callvirt instance class [Timberborn.SerializationSystem]Timberborn.SerializationSystem.SerializedObject [Timberborn.SerializationSystem]Timberborn.SerializationSystem.SerializedObjectReaderWriter::ReadJsons(class [netstandard]System.Collections.Generic.IEnumerable`1<string>)
        if (instruction.opcode != OpCodes.Callvirt) {
          newInstructions.Add(instruction);
          continue;
        }
        var mi = (MethodInfo)instruction.operand;
        if (mi.DeclaringType != typeof(SerializedObjectReaderWriter) || mi.Name != "ReadJsons") {
          newInstructions.Add(instruction);
          continue;
        }
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + ModStarter.ModName + $": Patching {original.DeclaringType.FullName.Split('.').Last()}.{original.Name} call to {mi.DeclaringType.Name}.{mi.Name}");
        // C# SpecServicePatch.ErrorReporter(baseComponent.Initialize, this)
        // IL call class [Timberborn.SerializationSystem]Timberborn.SerializationSystem.SerializedObject Mods.MoreModLogs.SpecServicePatch::ErrorReporter(class [Timberborn.SerializationSystem]Timberborn.SerializationSystem.SerializedObjectReaderWriter, class [Timberborn.BlueprintSystem]Timberborn.BlueprintSystem.BlueprintFileBundle)
        if (newInstructions[^1].opcode != OpCodes.Box) {
          throw new InvalidProgramException("Expected box before callvirt");
        }
        newInstructions.RemoveAt(newInstructions.Count - 1);
        if (newInstructions[^1].opcode != OpCodes.Callvirt) {
          throw new InvalidProgramException("Expected callvirt before box");
        }
        var mi2 = (MethodInfo)newInstructions[^1].operand;
        if (mi2.DeclaringType != typeof(BlueprintFileBundle) || mi2.Name != "get_Jsons") {
          throw new InvalidProgramException("Expected callvirt BlueprintFileBundle::get_Jsons");
        }
        newInstructions.RemoveAt(newInstructions.Count - 1);
        newInstructions.Add(new CodeInstruction(OpCodes.Call, typeof(SpecServicePatch).GetMethod(nameof(ErrorReporter), BindingFlags.Static | BindingFlags.NonPublic)));
      }
      foreach (var instruction in newInstructions) {
        yield return instruction;
      }
    }

    private static SerializedObject ErrorReporter(SerializedObjectReaderWriter serializedObjectReaderWriter, BlueprintFileBundle blueprintFileBundle) {
      try {
        return serializedObjectReaderWriter.ReadJsons(blueprintFileBundle.Jsons);
      }
      catch {
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"SpecService.Load of {blueprintFileBundle.Path} failed");
        throw;
      }
    }
  }
}
