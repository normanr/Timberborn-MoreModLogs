using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using HarmonyLib;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  public static class PathMeshDrawerPatch {

    private const byte Nothing = 0;
    private const byte Path = 1;
    private const byte AlternativePath = 2;
    private const byte Building = 3;
    private const byte AlternativeBuilding = 4;

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.BuildingsNavigation.PathMeshDrawer").Method("Add");
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + ModStarter.ModName + $": Patching PathMeshDrawer.Add");
      var keys = Traverse.CreateWithType("Timberborn.BuildingsNavigation.PathMeshConnectionKeys");
      if ((Nothing != keys.Field("Nothing").GetValue<byte>()) ||
          (Path != keys.Field("Path").GetValue<byte>()) ||
          (AlternativePath != keys.Field("AlternativePath").GetValue<byte>()) ||
          (Building != keys.Field("Building").GetValue<byte>()) ||
          (AlternativeBuilding != keys.Field("AlternativeBuilding").GetValue<byte>())) {
        throw new InvalidProgramException("Unexpected values for PathMeshConnectionKeys");
      }
      List<CodeInstruction> newInstructions = [];
      List<CodeInstruction> neighboredValueLoads = [];
      foreach (var instruction in instructions) {
        if (instruction.opcode != OpCodes.Call && instruction.opcode != OpCodes.Callvirt) {
          newInstructions.Add(instruction);
          continue;
        }
        var mi = (MethodInfo)instruction.operand;
        if (mi.Name == "TryGetMatch") {
          // C# _meshes.TryGetMatch(down, left, up, right, out var value)
          // IL ldloc.1
          // IL ldloc.2
          // IL ldloc.3
          // IL ldloc.s 4
          // IL ldloca.s 5
          // IL callvirt instance bool class [Timberborn.Coordinates]Timberborn.Coordinates.NeighboredValues4`1<class [Timberborn.PrefabOptimization]Timberborn.PrefabOptimization.IntermediateMesh>::TryGetMatch(uint8, uint8, uint8, uint8, valuetype [Timberborn.Coordinates]Timberborn.Coordinates.OrientedValue`1<!0>&)
          foreach (var ldloc in newInstructions.GetRange(newInstructions.Count - 5, 4)) {
            if (!ldloc.IsLdloc()) {
              throw new InvalidProgramException("Expected 4 ldloc before TryGetMatch");
            }
            neighboredValueLoads.Add(ldloc);
          }
          newInstructions.Add(instruction);
          continue;
        }
        if (mi.Name != "LogWarning") {
          newInstructions.Add(instruction);
          continue;
        }
        // C# Debug.LogWarning(message)
        // IL call void [UnityEngine.CoreModule]UnityEngine.Debug::LogWarning(object)
        // C# PathMeshDrawerPatch.LogWarning(message, down, left, up, right)
        // IL call void Mods.MoreModLogs.PathMeshDrawerPatch::LogWarning(object, uint8, uint8, uint8, uint8)
        newInstructions.AddRange(neighboredValueLoads);
        newInstructions.Add(new CodeInstruction(OpCodes.Call, typeof(PathMeshDrawerPatch).GetMethod(nameof(LogWarning), BindingFlags.Static | BindingFlags.NonPublic)));
      }
      foreach (var instruction in newInstructions) {
        yield return instruction;
      }
    }

    private static string ConvertKeyToChar(byte key) {
      return key switch {
        Nothing => "0",
        Path => "P",
        AlternativePath => "p",
        Building => "B",
        AlternativeBuilding => "b",
        _ => "?{key}",
      };
    }

    private static void LogWarning(object message, byte down, byte left, byte up, byte right) {
      var variant = ConvertKeyToChar(down) + ConvertKeyToChar(left) + ConvertKeyToChar(up) + ConvertKeyToChar(right);
      Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + ModStarter.ModName + $": {variant}: {message}");
    }
  }
}
