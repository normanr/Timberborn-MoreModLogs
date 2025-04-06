using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using HarmonyLib;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class SingletonSystemPatch {

    public static IEnumerable<MethodBase> TargetMethods() {
      var sls = AccessTools.TypeByName("Timberborn.SingletonSystem.SingletonLifecycleService");
      return new List<MethodBase> {
        sls.Method("LoadSingletons"),
        sls.Method("LoadNonSingletons"),
        sls.Method("PostLoadSingletons"),
        sls.Method("PostLoadNonSingletons"),
        sls.Method("UnloadSingletons"),
      };
    }

    static IEnumerable<CodeInstruction> Transpiler(MethodBase __originalMethod, IEnumerable<CodeInstruction> instructions) {
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + ModStarter.ModName + ": Transpiling " + __originalMethod.DeclaringType + "." +  __originalMethod.Name);
      foreach (var instruction in instructions) {
        // C# loader.Load()
        // IL callvirt instance void[Timberborn.SingletonSystem] Timberborn.SingletonSystem.ILoadableSingleton::Load()
        if (instruction.opcode == OpCodes.Callvirt) {
          // C# SingletonSystemPatch.Wrap(loader.Loader())
          // IL dup
          // IL ldvirtftn instance void [Timberborn.SingletonSystem]Timberborn.SingletonSystem.ILoadableSingleton::Load()
          // IL newobj instance void [mscorlib]System.Action::.ctor(object, native int)
          // IL call void Mods.MoreModLogs.SingletonSystemPatch::Wrap(class [mscorlib]System.Action)
          yield return new CodeInstruction(OpCodes.Dup);
          yield return new CodeInstruction(OpCodes.Ldvirtftn, instruction.operand);
          yield return new CodeInstruction(OpCodes.Newobj, typeof(Action).GetConstructors()[0]);
          yield return new CodeInstruction(OpCodes.Call, typeof(SingletonSystemPatch).GetMethod(nameof(Wrap), BindingFlags.Static | BindingFlags.NonPublic));
          continue;
        }
        yield return instruction;
      }
    }

    private static void Wrap(Action fn) {
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + fn.Method.DeclaringType + "." + fn.Method.Name + "()...");
      var __state = DateTime.Now;
      fn();
      var duration = DateTime.Now - __state;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + fn.Method.DeclaringType + "." + fn.Method.Name + "() executed in " + duration);
    }
  }
}
