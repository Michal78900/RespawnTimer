# RespawnTimer

This plugin supports the [SerpentsHand](https://github.com/Exiled-Team/SerpentsHand), [UIURescueSquad](https://github.com/Jesus-QC/UIURescueSquad) and [GhostSpectator](https://github.com/Thundermaker300/GhostSpectator) plugins.

# Default config
```yml
respawn_timer:
  is_enabled: true
  # Should debug messages be shown in a server console?
  show_debug_messages: false
  # How often (in seconds) should timer be refreshed?
  interval: 1
  # Should a timer be lower or higher on the screen? (values from 0 to 14, 0 - very high, 14 - very low)
  text_lowering: 8
  # Should a timer show an exact number of minutes?
  show_minutes: true
  # Should a timer show an exact number of seconds?
  show_seconds: true
  # Should a timer be only shown, when a spawnning sequence has begun? (NTF Helicopter / Chaos Car arrives)
  show_timer_only_on_spawn: false
  # Should number of spectators be shown?
  show_number_of_spectators: true
  # Should the NTF and CI respawn tickets be shown?
  show_tickets: true
  # Translations: (do NOT change text in { }, you can for example bold them)
  you_will_respawn_in: '<color=orange>You will respawn in: </color>'
  you_will_spawn_as: 'You will spawn as: '
  ntf: <color=blue>Nine-Tailed Fox</color>
  ci: <color=green>Chaos Insurgency</color>
  sh: <color=red>Serpent's Hand</color>
  uiu: <color=#1078e0>Unusual Incidents Unit</color>
  spectators: '<color=#B3B6B7>Spectators: </color>'
  ntf_tickets: '<color=blue>NTF Tickets: </color>'
  ci_tickets: '<color=green>CI Tickets: </color>'
  seconds: ' <b>{seconds} s</b>'
  minutes: <b>{minutes} min.</b>
  spectators_num: '{spectators_num}'
  ntf_tickets_num: '{ntf_tickets_num}'
  ci_tickets_num: '{ci_tickets_num}'
  ```
