Adds log entries showing how long a mod took to start (or failed to start) and where it was loaded from, eg:

13:10:49 More Mod Logs: Started from ...\Timberborn\Mods\moremodlogs\version-0.7 in 00:00:00.2235031
13:10:49 Beaverpower: Started from ...\1062090\3282307053 in 00:00:00.0001183
13:10:49 Mod Settings: Started from ...\Timberborn\Mods\mod-settings_4145545\version-0.7 in 00:00:00.0017191
13:10:49 Goods Statistics: Started from ...\1062090\3321521358\version-0.7 in 00:00:00.0015691
13:10:50 TimberApi UIBuilder: Started from ...\Timberborn\Mods\timberapi-uibuilder_4757575_1.0.0.1\version-0.7 in 00:00:00.0047461
13:10:50 Steam Update Buttons: Started from ...\Timberborn\Mods\steamupdatebuttons\version-0.7 in 00:00:00.0002587

Adds log entries showing how long an asset bundles took to load (or failed to load):

13:10:51 AssetBundle.LoadFromFile(modmanagerui.bundle) executed in 00:00:00.0016135
13:10:51 AssetBundle.LoadFromFile(modsettings_win) executed in 00:00:00.0014985
13:10:51 AssetBundle.LoadFromFile(minimap_win) executed in 00:00:00.0009108

Appends the full path to localization files loaded from mods (to make it easier to find which localization file contains errors), eg:

Localization file contains errors:
Unnecessary space at the end of column found in enUS_at_...\1062090\3285720709\Localizations\enUS.csv (line 107)

Adds log entries for singletons that took longer than 0.1s to load/unload (or failed to load/unload):

13:10:51 Timberborn.LanguageUI.ChangeLanguageBox.Load() executed in 00:00:00.1436906
13:10:51 Timberborn.MainMenuScene.MainMenuInitializer.Load() executed in 00:00:00.1567935

Adds a validation check for non-empty WorkshopRandomNeedApplier effect list

15:11:46 LumberMill.Folktails WorkshopRandomNeedApplier has no Effects!?!

Adds a log entry when failing to load an entity from a saved game

15:12:31 WorldEntitiesLoader.LoadEntity(CordialFertilizerDump.FolkTails) failed with an exception

