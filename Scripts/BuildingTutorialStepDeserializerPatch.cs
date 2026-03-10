using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Timberborn.BlockObjectTools;
using Timberborn.TemplateSystem;
using Timberborn.ToolButtonSystem;
using Timberborn.TutorialSteps;

namespace Mods.MoreModLogs {

  [HarmonyPatch()]
  static class BuildingTutorialStepDeserializerPatch {

    [HarmonyPatch("Timberborn.TutorialSteps.BuildingTutorialStepDeserializer", "GetToolButtons")]
    [HarmonyPrefix()]
    static void GetToolButtonsPrefix(ToolButtonService ____toolButtonService, IEnumerable<string> templateNames) {
      var buttons = ____toolButtonService.ToolButtons.Where(b => b.Tool is BlockObjectTool).Select(b => (BlockObjectTool)b.Tool);
      foreach (string templateName in templateNames)
      {
        var count = buttons.Count((BlockObjectTool tool) => tool.Template.GetSpec<TemplateSpec>().IsNamedExactly(templateName));
        if (count != 1) {
          TutorialStageServicePatch.AddWarning($"*** {templateName} has the wrong count: {count}");
        }
      }
    }
  }
}
