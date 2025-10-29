﻿using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using Timberborn.ModdingAssets;

namespace Mods.MoreModLogs {

  [HarmonyPatch]
  static class ModTextAssetConverterPatch {

    public static MethodBase TargetMethod() {
      return AccessTools.TypeByName("Timberborn.ModdingAssets.ModTextAssetConverter").Method("TryConvert");
    }

    static void Postfix(OrderedFile orderedFile, string path, ref TextAsset asset) {
      var file = orderedFile.File;
      if (file.FullName.ToLower().Split(Path.DirectorySeparatorChar).Contains("localizations")) {
        asset.name += "_in_" + UserDataSanitizer.Sanitize(file.FullName);
      }
    }
  }
}
