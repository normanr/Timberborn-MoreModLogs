using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using HarmonyLib;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class ModTextAssetConverterPatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.ModdingAssets.ModTextAssetConverter").Method("TryConvert");
    }

    static void Postfix(FileInfo fileInfo, ref TextAsset asset) {
      if (fileInfo.FullName.ToLower().Split(Path.DirectorySeparatorChar).Contains("localizations")) {
        asset.name += "_in_" + UserDataSanitizer.Sanitize(fileInfo.FullName);
      }
    }
  }
}
