# UIURescueSquad
 A new exiled plugin that add a new GOI to the game [FBI/UIU]

# Installation
Download the .dll file of the latest release and place it inside the Exiled Plugins folder

# Configs
```yaml
UIURescueSquad:
  # Is the plugin enabled?
  is_enabled: true

  # How many times mtfs must have respawn to spawn UIU
  respawns: 1
  # Probability of a UIU Squad replacing a MTF spawn
  probability: 50

  # Use hints instead of broadcasts?
  use_hints: false
  # Entrance announcement message (null to disable it)
  announcement_text: <b>The <color=#FFFA4B>UIU Rescue Squad</color> has arrived to the facility</b>
  # Entrance announcement message time
  announcement_time: 10
  # UIU Player broadcast (null to disable it)
  u_i_u_broadcast: <i>You are an</i><color=yellow><b> UIU trooper</b></color>, <i>help </i><color=#0377fc><b>MTFs</b></color><i> to finish its job</i>
  # UIU Player broadcast (null to disable it)
  u_i_u_broadcast_time: 10

  # UIU Soldier life (NTF CADET)
  u_i_u_soldier_life: 160
  # The items UIUs soldiers spawn with.
  u_i_u_soldier_inventory:
  - KeycardNTFLieutenant
  - GunProject90
  - GunUSP
  - Disarmer
  - Medkit
  - Adrenaline
  - Radio
  - GrenadeFrag
  # UIU Soldier Rank (THE BADGE ON THE LIST)
  u_i_u_soldier_rank: UIU Soldier

  # UIU Agent life (NTF LIEUTENANT)
  u_i_u_agent_life: 175
  # The items UIUs agents spawn with.
  u_i_u_agent_inventory:
  - KeycardNTFLieutenant
  - GunProject90
  - GunUSP
  - Disarmer
  - Medkit
  - Adrenaline
  - Radio
  - GrenadeFrag
  # UIU Agent Rank (THE BADGE ON THE LIST)
  u_i_u_agent_rank: UIU Agent

  # UIU Leader life (NTF COMMANDER)
  u_i_u_leader_life: 215
  # The items UIU leader spawn with.
  u_i_u_leader_inventory:
  - KeycardNTFLieutenant
  - GunProject90
  - GunUSP
  - Disarmer
  - Medkit
  - Adrenaline
  - Radio
  - GrenadeFrag
  # UIU Leader Rank (THE BADGE ON THE LIST)
  u_i_u_leader_rank: UIU Leader
```
