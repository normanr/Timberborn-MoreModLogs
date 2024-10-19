Adds log entries for when a mod is about to be started and after it has finished starting (with where is was loaded from and how long it took to load), eg:

13:10:49.654 Starting More Mod Logs from C:\Users\steamuser\Documents\Timberborn\Mods\betterlogging
13:10:49.897 Started More Mod Logs in 00:00:00.2235031
13:10:49.898 Starting Beaverpower from C:\Program Files (x86)\Steam\steamapps\workshop\content\1062090\3282307053
13:10:49.898 Started Beaverpower in 00:00:00.0001183
13:10:49.898 Starting Mod Settings from C:\Users\user\Documents\Timberborn\Mods\mod-settings_4145545
13:10:49.900 Started Mod Settings in 00:00:00.0017191
13:10:49.900 Starting Goods Statistics from C:\Program Files (x86)\Steam\steamapps\workshop\content\1062090\3321521358
13:10:49.902 Started Goods Statistics in 00:00:00.0015691
13:10:49.902 Starting TimberApi from C:\Users\user\Documents\Timberborn\Mods\timberapi_2463537
13:10:50.078 Started TimberApi in 00:00:00.1765906
13:10:50.078 Starting Steam Update Buttons from C:\Users\user\Documents\Timberborn\Mods\steamupdatebuttons
13:10:50.079 Started Steam Update Buttons in 00:00:00.0002587

Appends the full path to localization files loaded from mods (to make it easier to find which localization file contains errors), eg:

Localization file contains errors:
Unnecessary space at the end of column found in enUS_at_C:\Program Files (x86)\Steam\steamapps\workshop\content\1062090\3285720709\Localizations\enUS.csv (line 107)

