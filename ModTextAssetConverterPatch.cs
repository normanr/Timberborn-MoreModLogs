using System.IO;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using System.Linq;

namespace Mods.SteamUpdateButtons {

  [HarmonyPatch]
  static class ModTextAssetConverterPatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.ModdingAssets.ModTextAssetConverter").Method("TryConvert");
    }

    static void Postfix(FileInfo fileInfo, ref TextAsset asset) {
      if (fileInfo.FullName.ToLower().Split(Path.DirectorySeparatorChar).Contains("localizations")) {
        asset.name += "_in_" + fileInfo.FullName;
      }
    }
  }
}
