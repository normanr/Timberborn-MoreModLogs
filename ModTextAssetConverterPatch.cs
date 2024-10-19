using System.IO;
using System.Reflection;
using UnityEngine;
using HarmonyLib;

namespace Mods.SteamUpdateButtons {

  [HarmonyPatch]
  static class ModTextAssetConverterPatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.ModdingAssets.ModTextAssetConverter").Method("TryConvert");
    }

    static void Postfix(FileInfo fileInfo, ref TextAsset asset) {
      asset.name += "_in_" + fileInfo.FullName;
    }
  }
}
