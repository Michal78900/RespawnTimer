# RespawnTimer
A SCP: Secret Laboratory plugin that shows when the next respawn wave will happen.

# Features
- Fully customizable timer that may show additional info, like round time, server TPS, amount of spectators, enabled generators etc.
- Option for adding multiple custom text (hints) to the interface, where you can put advertisements and/or gameplay hints for players.
- Option for hiding the timer interface (**.timer** command in client console)
- Option for hiding the timer interface if admin is using Overwatch mode (enabled by default)
- Support for [Serpent's Hand](https://github.com/Exiled-Team/SerpentsHand) and [UIURescueSquad](https://github.com/Marco15453/UIURescueSquad) custom teams.

# Configuration
When you first install the plugin, the `RespawnTimer` folder will be created. In it you will find a folder named `Template`. The `Template` is a template timer interface that isn't used by the plugin itself, but allows you to see how the structure of a config looks like.
You can copy that folder and rename it to something else and then desing your own look of the interface.<br>
An example timer interface called `ExamapleTimer` can be found in releases. It will contain an example timer interface that has all of the possible properties the plugin can show. However, it is recommended to design your own interface, so your server has its own unique look.
