# RespawnTimer

# Config
| Name | Type | Description | Default |
| --- | --- | --- | --- |
| IsEnabled | bool | Is the plugin enabled | true |
| ShowTimerOnlyOnSpawn | bool | Should a timer be only shown, when a spawnning sequence has begun? (NTF Helicopter / Chaos Car arrives) Good, if you want to keep ghosting to minimum or something | false |
| ShowTickets | bool | Should the NTF and CI respawn tickets be shown? | true
| translations | List<string> | All of the texts in this plugin. If you need to translate them, you can easily do this: (The dashed line is just a interspace. Things in { } are numeric variables, you can for example bold them. DO NOT CHANGE NAMES IN { }! | English

#Translations
Feel free to send me your translation and I can add it to the list

#Polish (by me):
translations:
  - '<color=orange>Zrespawnujesz się za: </color>'
  - 'Pojawisz się jako: '
  - <color=blue>Nine-Tailed Fox</color>
  - <color=green>Rebelia Chaosu</color>
  - '<color=blue>Bilety NTF: </color>'
  - '<color=green>Bilety Rebelli: </color>'
  - '-------------------------------------'
  - <b>{seconds} s</b>
  - '{ntf_tickets_num}'
  - '{ci_tickets_num}'
