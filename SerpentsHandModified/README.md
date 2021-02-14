# SerpentsHand

A plugin that adds a new class to your server named "Serpent's Hand". This class works with the SCPs to eliminate all other beings. They have a chance to spawn instead of a squad of Chaos Insurgency.

# Installation

**[EXILED](https://github.com/galaxy119/EXILED) must be installed for this to work.**

**If you have [AdminTools](https://github.com/galaxy119/AdminTools/tree/master/AdminTools) installed, make sure you set `admin_god_tuts: false` in your EXILED config.**

Place the "SerpentsHand.dll" file in your sm_plugins folder.

# Features
* Uses the tutorial model for this class
* Class has a configrable percent chance to spawn instead of chaos
* A custom spawn location
* Commands to spawn individual members and a squad manually
* Announcements for a squad of Serpent's Hand spawning, as well as one for chaos spawning to let the players know which one spawned
* Custom API for other plugins to interact with, see [this page for API usage.](https://github.com/Cyanox62/SerpentsHand/wiki/API)

# Configs
| Config        | Value Type | Default Value | Description |
| :-------------: | :---------: | :------: | :--------- |
| sh_spawn_chance | 1-100 | 50 | The percent chance for a squad of Serpent's Hand to spawn instead of chaos. |
| sh_entry_announcement | String | SERPENTS HAND HASENTERED | The announcement to be played when Serpent's Hand are spawned, sentences must be written exactly to work with CASSIE's available phrases (Ex. serpents hand . number two hundred and thirty two), all phrases can be found [here](https://pastebin.com/rpMuRYNn). |
| sh_ci_entry_announcement | String | | The annoumcement to be played when Chaos spawn instead of Serpent's Hand, same rules as the other announcement config apply. |
| sh_spawn_items | List | 21, 26, 12, 14, 10 | The item IDs that Serpent's Hand members should spawn with. A full list of item IDs can be found [here](https://github.com/Cyanox62/SerpentsHand/wiki/Item-IDs). |
| sh_friendly_fire | Boolean | False | Should SCPs and Serpent's Hand be able to hurt eachother. This includes 106's pocket dimension, with this disabled, Serpent's Hand members will never die no matter which exit they take in the Pocket Dimension. |
| sh_teleport_to_106 | Boolean | True | When a Serpent's hand member escapes the Pocket Dimension, should they teleport to 106 instead of spawning at his chamber. |
| sh_scps_win_with_chaos | Boolean | True | Set this to false if Chaos do not win with SCPs on your server. |
| sh_health | Integer | 120 | How much health Serpent's Hand members will have. |
| sh_max_squad | Integer | 8 | The maximum number of Serpent's Hand members allowed to spawn. |
| sh_team_respawn_delay | Integer | 1 | The amount of team respawns that must occur before Serpent's Hand can spawn. |

# Commands
|     Command    | Value Type | Description |
| :-------------: | :---------: | :--------- |
| SPAWNSH | Player Name / SteamID64 | Spawns the specified player as Serpent's Hand. |
| SPAWNSHSQUAD | Squad Size | Spawns a Squad of Serpent's Hand, if no size is specified it will default to 5. This will trigger the squad spawn announcement. |
