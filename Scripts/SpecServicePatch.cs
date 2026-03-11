using System;
using System.Collections.Generic;
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
      var codeMatcher = new CodeMatcher(instructions);

      // C# _serializedObjectReaderWriter.ReadJsons(blueprintBundle.Jsons);
      // IL callvirt instance valuetype [System.Collections.Immutable]System.Collections.Immutable.ImmutableArray`1<string> Timberborn.BlueprintSystem.BlueprintFileBundle::get_Jsons()
      // IL box valuetype [System.Collections.Immutable]System.Collections.Immutable.ImmutableArray`1<string>
      // IL callvirt instance class [Timberborn.SerializationSystem]Timberborn.SerializationSystem.SerializedObject [Timberborn.SerializationSystem]Timberborn.SerializationSystem.SerializedObjectReaderWriter::ReadJsons(class [netstandard]System.Collections.Generic.IEnumerable`1<string>)
      CodeMatch[] oldCode = [
        CodeMatch.Calls(typeof(BlueprintFileBundle).PropertyGetter("Jsons") ?? throw new Exception($"BlueprintFileBundle.Jsons not found!")),
        // Needs Harmony to support it, ref: https://github.com/pardeike/Harmony/issues/756
        // CodeMatch.Calls(() => default(BlueprintFileBundle).Jsons),
        CodeMatch.WithOpcodes([OpCodes.Box]),
        CodeMatch.Calls(() => default(SerializedObjectReaderWriter).ReadJsons(default)),
      ];
      // C# SpecServicePatch.ErrorReporter(_serializedObjectReaderWriter, blueprintBundle)
      // IL call class [Timberborn.SerializationSystem]Timberborn.SerializationSystem.SerializedObject Mods.MoreModLogs.SpecServicePatch::ErrorReporter(class [Timberborn.SerializationSystem]Timberborn.SerializationSystem.SerializedObjectReaderWriter, class [Timberborn.BlueprintSystem]Timberborn.BlueprintSystem.BlueprintFileBundle)
      CodeInstruction[] newCode = [
        CodeInstruction.Call(() => ErrorReporter(default, default))
      ];
      codeMatcher
        .MatchStartForward(oldCode)
        .ThrowIfInvalid("Could not find call to SpecService.Load call to SerializedObjectReaderWriter.ReadJsons")
        .RemoveInstructions(oldCode.Length)
        .InsertAndAdvance(newCode);

      return codeMatcher.Instructions();
    }

    private static SerializedObject ErrorReporter(SerializedObjectReaderWriter serializedObjectReaderWriter, BlueprintFileBundle blueprintFileBundle) {
      try {
        return serializedObjectReaderWriter.ReadJsons(blueprintFileBundle.Jsons);
      }
      catch {
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"SpecService.Load of {blueprintFileBundle.Path} ({string.Join(", ", blueprintFileBundle.Sources)}) failed");
        throw;
      }
    }
  }
}
