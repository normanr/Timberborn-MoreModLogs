using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using HarmonyLib;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class SingletonSystemPatch {

    public static IEnumerable<MethodBase> TargetMethods() {
      var sls = AccessTools.TypeByName("Timberborn.SingletonSystem.SingletonLifecycleService");
      var tms = AccessTools.TypeByName("Timberborn.TickSystem.TickableSingletonService+MeteredSingleton");
      return [
        sls.Method("LoadSingletons"),
        sls.Method("LoadNonSingletons"),
        sls.Method("PostLoadSingletons"),
        sls.Method("PostLoadNonSingletons"),
        sls.Method("UnloadSingletons"),
        tms.Method("Tick")
      ];
    }

    static IEnumerable<CodeInstruction> Transpiler(MethodBase __originalMethod, IEnumerable<CodeInstruction> instructions) {
      foreach (var instruction in instructions) {
        // C# loader.Load()
        // IL callvirt instance void[Timberborn.SingletonSystem] Timberborn.SingletonSystem.ILoadableSingleton::Load()
        if (instruction.opcode != OpCodes.Callvirt) {
          yield return instruction;
          continue;
        }
        var mi = (MethodInfo)instruction.operand;
        if (mi.ReturnType != typeof(void) || mi.GetParameters().Length > 0 || mi.Name == "Resume" || mi.Name == "Pause") {
          yield return instruction;
          continue;
        }
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + ModStarter.ModName + $": Patching {__originalMethod.DeclaringType.FullName.Split('.').Last()}.{__originalMethod.Name} call to {mi.DeclaringType.Name}.{mi.Name}");
        // C# SingletonSystemPatch.ErrorReporter(loader.Load)
        // IL dup
        // IL ldvirtftn instance void [Timberborn.SingletonSystem]Timberborn.SingletonSystem.ILoadableSingleton::Load()
        // IL newobj instance void [mscorlib]System.Action::.ctor(object, native int)
        // IL call void Mods.MoreModLogs.SingletonSystemPatch::ErrorReporter(class [mscorlib]System.Action)
        yield return new CodeInstruction(OpCodes.Dup);
        yield return new CodeInstruction(OpCodes.Ldvirtftn, instruction.operand);
        yield return new CodeInstruction(OpCodes.Newobj, typeof(Action).GetConstructors()[0]);
        yield return new CodeInstruction(OpCodes.Call, typeof(SingletonSystemPatch).GetMethod(nameof(ErrorReporter), BindingFlags.Static | BindingFlags.NonPublic));
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
  }
}
