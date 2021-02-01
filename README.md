# RespawnTimer

This plugin supports the Cyanox62/SerpentsHand plugin and Jesus-QC/UIURescueSquad. You need to download the modified version of SerpentsHand to make it work with RespawnTimer (see releases). UIURescueSquad will work fine on it's own.

# Config
| Name | Type | Description | Default |
| --- | --- | --- | --- |
| IsEnabled | bool | Is the plugin enabled | true |
| Interval | float | How often (in seconds) should timer be refreshed? | 1 |
| TextLowering | byte | Should a timer be lower or higher on the screen (values from 0 to 14) | 8 |
| ShowMinutes | bool | Should a timer show an exact number of minutes? | true |
| ShowSeconds | bool | Should a timer show an exact number of seconds? | true |
| ShowTimerOnlyOnSpawn | bool | Should a timer be only shown, when a spawnning sequence has begun? (NTF Helicopter / Chaos Car arrives) Good, if you want to keep ghosting to minimum or something | false |
| ShowTickets | bool | Should the NTF and CI respawn tickets be shown? | true

# Translations
Feel free to send me your translation so I can add it to the list

English (by me):
```yml
# Translations: (do NOT change text in { }, you can for example bold them)
  you_will_respawn_in: '<color=orange>You will respawn in: </color>'
  you_will_spawn_as: 'You will spawn as: '
  ntf: <color=blue>Nine-Tailed Fox</color>
  ci: <color=green>Chaos Insurgency</color>
  sh: <color=red>Serpent's Hand</color>
  uiu: <color=#1078e0>Unusual Incidents Unit</color>
  ntf_tickets: '<color=blue>NTF Tickets: </color>'
  ci_tickets: '<color=green>CI Tickets: </color>'
  seconds: <b>{seconds} s</b>
  ntf_tickets_num: '{ntf_tickets_num}'
  ci_tickets_num: '{ci_tickets_num}'
```

Polish (by me):
```yml
# Translations: (do NOT change text in { }, you can for example bold them)
  you_will_respawn_in: '<color=orange>Zrespawnujesz się za: </color>'
  you_will_spawn_as: 'Pojawisz się jako: '
  ntf: <color=blue>Nine-Tailed Fox</color>
  ci: <color=green>Rebelia Chaosu</color>
  sh: <color=red>Ręka Węża</color>
  uiu: <color=#1078e0>Jednostka Incydentów Nadzwyczajnych</color>
  ntf_tickets: '<color=blue>Bilety NTF: </color>'
  ci_tickets: '<color=green>Bilety CI: </color>'
  seconds: <b>{seconds} s</b>
  ntf_tickets_num: '{ntf_tickets_num}'
  ci_tickets_num: '{ci_tickets_num}'
  ```
