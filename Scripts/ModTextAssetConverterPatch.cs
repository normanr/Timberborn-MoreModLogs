using System.IO;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Timberborn.ModdingAssets;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(ModTextAssetConverter))]
[HarmonyPatch(nameof(ModTextAssetConverter.TryConvert))]
static class ModTextAssetConverterPatch {

  static void Postfix(OrderedFile orderedFile, ref TextAsset asset) {
    if (orderedFile.File.FullName.ToLower().Split(Path.DirectorySeparatorChar).Contains("localizations")) {
      asset.name += "_in_" + UserDataSanitizer.Sanitize(orderedFile.File.FullName);
    }
  }
}
