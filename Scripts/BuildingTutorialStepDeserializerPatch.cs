using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Timberborn.BlockObjectTools;
using Timberborn.TemplateSystem;
using Timberborn.TutorialSteps;

namespace Mods.MoreModLogs;

[HarmonyPatch(typeof(BuildingTutorialStepDeserializer))]
[HarmonyPatch(nameof(BuildingTutorialStepDeserializer.GetToolButtons))]
static class BuildingTutorialStepDeserializerPatch {

  static void Prefix(BuildingTutorialStepDeserializer __instance, IEnumerable<string> templateNames) {
    var buttons = __instance._toolButtonService.ToolButtons.Where(b => b.Tool is BlockObjectTool).Select(b => (BlockObjectTool)b.Tool);
    foreach (string templateName in templateNames) {
      var count = buttons.Count(tool => tool.Template.GetSpec<TemplateSpec>().IsNamedExactly(templateName));
      if (count != 1) {
        TutorialStageServicePatch.AddWarning($"*** {templateName} has the wrong count: {count}");
      }
    }
  }
}
