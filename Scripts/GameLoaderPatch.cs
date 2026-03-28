using UnityEngine;
using HarmonyLib;
using Timberborn.GameSaveRuntimeSystem;
using Timberborn.WorldSerialization;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(GameLoader))]
[HarmonyPatch(nameof(GameLoader.Load))]
static class GameLoaderPatch {

  static void Postfix(SerializedWorld __result) {
    var factionId = __result.GetSingleton("FactionService").Get<string>("Id");
    var mapName = __result.GetSingleton("MapNameService").Get<string>("Name");
    Debug.Log($"FactionId: {factionId}, MapName: {mapName}");
  }
}
