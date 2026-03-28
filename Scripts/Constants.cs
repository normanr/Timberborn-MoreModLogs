using System.Collections.Generic;

namespace Mods.MoreModLogs;

internal class Constants {
  internal static readonly Dictionary<string, string> IdFields = new() {
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

}
