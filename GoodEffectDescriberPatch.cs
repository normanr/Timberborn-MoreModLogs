using System;
using System.Text;
using UnityEngine;
using HarmonyLib;
using Timberborn.Effects;
using Timberborn.Goods;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(GoodEffectDescriber))]
  [HarmonyPatch(nameof(GoodEffectDescriber.DescribeEffects))]
  [HarmonyPatch([typeof(GoodSpec), typeof(StringBuilder)] )]
  static class GoodEffectDescriberPatch {

    static void Finalizer(Exception __exception, GoodSpec goodSpec) {
      if (__exception == null) return;
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"GoodEffectDescriber.DescribeEffects({goodSpec.Id}) failed with an exception");
      if (goodSpec.ConsumptionEffects == null) {
        Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + "  " + goodSpec.Id + " is missing ConsumptionEffects");
      }
    }

  }
}
