using System;
using UnityEngine;
using HarmonyLib;
using Timberborn.PrefabSystem;
using Timberborn.Workshops;
using Timberborn.WorkshopsUI;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(ManufactoryRecipeSliderToggleFactory))]
  [HarmonyPatch(nameof(ManufactoryRecipeSliderToggleFactory.Create))]
  static class ManufactoryRecipeSliderToggleFactoryPatch {

    static void Finalizer(Exception __exception, Manufactory manufactory) {
      if (__exception == null) return;
      var name = manufactory?.GetComponentFast<PrefabSpec>()?.PrefabName ?? "unknown";
      Debug.Log(DateTime.Now.ToString("HH:mm:ss ") + $"ManufactoryRecipeSliderToggleFactory.Create({name}) failed with an exception");
      foreach (var recipe in manufactory.ProductionRecipes) {
        if (recipe.UIIcon == null) {
          Debug.LogError(DateTime.Now.ToString("HH:mm:ss ") + "  " + recipe.Id + " has null UIIcon");
        }
      }
    }

  }
}
