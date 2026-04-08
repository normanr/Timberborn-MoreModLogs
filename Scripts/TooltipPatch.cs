using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;
using HarmonyLib;
using Timberborn.TooltipSystem;

namespace Mods.MoreModLogs;

[HarmonyPatch]
static class TooltipPatch {

  public static IEnumerable<MethodBase> TargetMethods() {
    var tt = typeof(Tooltip).GetNestedTypes(BindingFlags.NonPublic).SingleOrDefault();
    var me = tt?.GetMethods(BindingFlags.NonPublic|BindingFlags.Instance).SingleOrDefault(
      m => {
        if (!m.Name.Contains("RegisterTooltip")) return false;
        var p = m.GetParameters();
        return p.Length > 0 && p[0].ParameterType == typeof(MouseEnterEvent);
      }
    );
    if (me != null) {
      yield return me;
    } else {
      Debug.LogWarning(DateTime.Now.ToString("HH:mm:ss ") + $"Failed to find Tooltip.RegisterTooltip MouseEnterEvent");
    }
  }

  static IEnumerable<CodeInstruction> Transpiler(MethodBase __originalMethod, IEnumerable<CodeInstruction> instructions) {
    foreach (var instruction in instructions) {
      // C# loader.Load()
      // IL callvirt instance !0 class [netstandard]System.Func`1<valuetype Timberborn.TooltipSystem.TooltipContent>::Invoke()
      if (instruction.opcode != OpCodes.Callvirt) {
        yield return instruction;
        continue;
      }
      var mi = (MethodInfo)instruction.operand;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"{ModStarter.ModName}: Patching {__originalMethod.DeclaringType.FullName.Split('.').Last()}.{__originalMethod.Name} call to {mi.DeclaringType.Name}.{mi.Name}");
      // C# SingletonSystemPatch.ErrorReporter(loader.Load)
      // IL call void Mods.MoreModLogs.TooltipPatch::ErrorReporter(class [mscorlib]System.Func`1<valuetype Timberborn.TooltipSystem.TooltipContent>)
      yield return new CodeInstruction(OpCodes.Call, typeof(TooltipPatch).GetMethod(nameof(ErrorReporter), BindingFlags.Static | BindingFlags.NonPublic));
    }
  }

  private static TooltipContent ErrorReporter(Func<TooltipContent> fn) {
    var start = DateTime.Now;
    try {
      return fn();
    }
    catch (Exception ex) {
      var duration = DateTime.Now - start;
      Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + $"{fn.Target.GetType()}.{fn.Method.Name}() failed after {duration}");
      throw new TargetInvocationException($"{fn.Target.GetType()}.{fn.Method.Name}() failed", ex);
    }
  }
}
