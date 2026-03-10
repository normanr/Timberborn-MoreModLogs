using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using HarmonyLib;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class ComponentCachePatch {

    public static IEnumerable<MethodBase> TargetMethods() {
      var cc = AccessTools.TypeByName("Timberborn.BaseComponentSystem.ComponentCache");
      return [
        cc.Method("Initialize"),
        cc.Method("SetActive"),
      ];
    }

    static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions) {
      List<CodeInstruction> newInstructions = [];
      foreach (var instruction in instructions) {
        // C# awakableComponent.Awake()
        // IL callvirt instance void Timberborn.BaseComponentSystem.IAwakableComponent::Awake()
        // or
        // C# baseComponent.Initialize(this)
        // IL ldarg.0
        // IL callvirt instance void Timberborn.BaseComponentSystem.BaseComponent::Initialize(class Timberborn.BaseComponentSystem.ComponentCache)
        if (instruction.opcode != OpCodes.Callvirt) {
          newInstructions.Add(instruction);
          continue;
        }
        var mi = (MethodInfo)instruction.operand;
        if (mi.ReturnType != typeof(void) || mi.GetParameters().Length > 1 || mi.Name == "Dispose") {
          newInstructions.Add(instruction);
          continue;
        }
        if (mi.GetParameters().Length == 1) {
          if (mi.GetParameters()[0].ParameterType.Name != "ComponentCache") {
            newInstructions.Add(instruction);
            continue;
          }
        }
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + ModStarter.ModName + $": Patching {original.DeclaringType.FullName.Split('.').Last()}.{original.Name} call to {mi.DeclaringType.Name}.{mi.Name}");
        if (mi.GetParameters().Length == 0) {
          // C# ComponentCachePatch.ErrorReporter(awakableComponent.Awake)
          // IL dup
          // IL ldvirtftn instance void Timberborn.BaseComponentSystem.IAwakableComponent::Awake()
          // IL newobj instance void [mscorlib]System.Action::.ctor(object, native int)
          // IL call void Mods.MoreModLogs.ComponentCachePatch::ErrorReporter(class [mscorlib]System.Action)
          newInstructions.Add(new CodeInstruction(OpCodes.Dup));
          newInstructions.Add(new CodeInstruction(OpCodes.Ldvirtftn, instruction.operand));
          newInstructions.Add(new CodeInstruction(OpCodes.Newobj, typeof(Action).GetConstructors()[0]));
          newInstructions.Add(new CodeInstruction(OpCodes.Call, typeof(ComponentCachePatch).GetMethod(nameof(ErrorReporter), BindingFlags.Static | BindingFlags.NonPublic)));
        } else {
          // C# ComponentCachePatch.ErrorReporter1(baseComponent.Initialize, this)
          // IL dup
          // IL ldvirtftn instance void Timberborn.BaseComponentSystem.IAwakableComponent::Awake()
          // IL newobj instance void [mscorlib]System.Action::.ctor(object, native int)
          // IL ldarg.0
          // IL call void Mods.MoreModLogs.ComponentCachePatch::ErrorReporter1(class [mscorlib]System.Action`object, object)
          if (newInstructions[^1].opcode != OpCodes.Ldarg_0) {
            throw new InvalidProgramException("Expected ldarg.0 before callvirt");
          }
          newInstructions.RemoveAt(newInstructions.Count - 1);
          newInstructions.Add(new CodeInstruction(OpCodes.Dup));
          newInstructions.Add(new CodeInstruction(OpCodes.Ldvirtftn, instruction.operand));
          newInstructions.Add(new CodeInstruction(OpCodes.Newobj, typeof(Action<object>).GetConstructors()[0]));
          newInstructions.Add(new CodeInstruction(OpCodes.Ldarg_0));
          newInstructions.Add(new CodeInstruction(OpCodes.Call, typeof(ComponentCachePatch).GetMethod(nameof(ErrorReporter1), BindingFlags.Static | BindingFlags.NonPublic)));
        }
      }
      foreach (var instruction in newInstructions) {
        yield return instruction;          
      }
    }

    private static void ErrorReporter(Action fn) {
      var start = DateTime.Now;
      try {
        fn();
      }
      catch {
        var duration = DateTime.Now - start;
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + fn.Target.GetType() + "." + fn.Method.Name + "() failed after " + duration);
        throw;
      }
      {
        var duration = DateTime.Now - start;
        if (duration.TotalMilliseconds > 100) {
          Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + fn.Target.GetType() + "." + fn.Method.Name + "() executed in " + duration);
        }
      }
    }

    private static void ErrorReporter1(Action<object> fn, object obj) {
      var start = DateTime.Now;
      try {
        fn(obj);
      }
      catch {
        var duration = DateTime.Now - start;
        Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + fn.Target.GetType() + "." + fn.Method.Name + $"({obj}) failed after " + duration);
        throw;
      }
      {
        var duration = DateTime.Now - start;
        if (duration.TotalMilliseconds > 100) {
          Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + fn.Target.GetType() + "." + fn.Method.Name + $"({obj}) executed in " + duration);
        }
      }
    }
  }
}
