using Newtonsoft.Json.Linq;
using HarmonyLib;
using Timberborn.SerializationSystem;
using System.Collections.Generic;

namespace Mods.MoreModLogs {

  [HarmonyPatch(typeof(JsonMerger))]
  [HarmonyPatch("MergeJsons")]
  static class JsonMergerPatch {

    static readonly Dictionary<string, string> idFields = new() {
      {"BlockObjectToolGroupSpec", "Id"},
      {"BonusTypeSpec", "Id"},
      {"BotTexturesSpec", "FactionID"},  // from BobingaboutScriptPack
      {"CharacterGraphicsSetSpec", "SetId"},  // from BobingaboutScriptPack
      {"CustomBeaverByNameSpec", "Name"},  // from BobingaboutScriptPack
      {"CustomBottomBarElementSpec", "Id"},  // from ModdableToolGroups
      {"CustomCursorSpec", "Id"},
      {"FactionSpec", "Id"},
      {"GoodCollectionSpec", "CollectionId"},
      {"GoodGroupSpec", "Id"},
      {"GoodSpec", "Id"},
      {"GoodVisualizationSpec", "Id"},
      {"KeyBindingGroupSpec", "Id"},
      {"KeyBindingSpec", "Id"},
      {"MaterialCollectionSpec", "CollectionId"},
      {"NeedCollectionSpec", "CollectionId"},
      {"NeedGroupSpec", "Id"},
      {"NeedSpec", "Id"},
      {"RecipeSpec", "Id"},
      {"RemoveYieldStrategySpec", "Id"},
      {"TemplateCollectionSpec", "CollectionId"},
      {"TemplateSpec", "TemplateName"},
      {"ToolGroupSpec", "Id"},
      {"TutorialSpec", "Id"},
      {"TutorialStageSpec", "Id"},
      {"WorkerOutfitSpec", "Id"},
      {"WorkerTypeSpec", "Id"},
    };

    static void Prefix(JObject mergedJson, JObject json) {
      if (mergedJson == null) { return; }
      foreach (JProperty item in json.Children<JProperty>()) {
        if (!idFields.TryGetValue(item.Name, out var id)) {
          continue;
        }
        var path = $"{item.Name}.{id}";
        var newValue = json.SelectToken(path);
        if (newValue == null) {
          continue;
        }
        var oldValue = mergedJson.SelectToken(path);
        if (oldValue.Value<string>() == newValue.Value<string>()) {
          continue;
        }
        BlueprintDeserializerPatch.AddWarning($"{path} has conflicting values: {oldValue} != {newValue}");
      }
    }
  }
}
