using Newtonsoft.Json.Linq;
using HarmonyLib;
using Timberborn.SerializationSystem;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(JsonMerger))]
[HarmonyPatch(nameof(JsonMerger.MergeJsons))]
static class JsonMergerPatch {

  static void Prefix(JObject mergedJson, JObject json) {
    if (mergedJson == null) { return; }
    foreach (JProperty item in json.Children<JProperty>()) {
      if (!Constants.IdFields.TryGetValue(item.Name, out var id)) {
        continue;
      }
      var path = $"{item.Name}.{id}";
      var newValue = json.SelectToken(path);
      if (newValue == null) {
        continue;
      }
      var oldValue = mergedJson.SelectToken(path);
      if (oldValue == null || (oldValue.Value<string>() == newValue.Value<string>())) {
        continue;
      }
      BlueprintDeserializerPatch.AddWarning($"{path} has conflicting values: {oldValue} != {newValue}");
    }
  }
}
