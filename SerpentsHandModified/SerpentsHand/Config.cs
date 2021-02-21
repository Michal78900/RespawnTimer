using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace SerpentsHand
{
    public class Config : IConfig
    {
		[Description("If Serpents Hand is enabled.")]
		public bool IsEnabled { get; set; } = true;

		[Description("The items Serpents Hand spawn with.")]
		public List<int> SpawnItems { get; set; } = new List<int>() { 21, 26, 12, 14, 10 };

		[Description("The change for Serpents Hand to spawn instead of Chaos.")]
		public int SpawnChance { get; set; } = 50;
		[Description("The amount of health Serpents Hand has.")]
		public int Health { get; set; } = 120;
		[Description("The maximum size of a Serpents Hand squad.")]
		public int MaxSquad { get; set; } = 8;
		[Description("How many respawn waves must occur before considering Serpents Hand to spawn.")]
		public int RespawnDelay { get; set; } = 1;
		[Description("The maxium number of times Serpents can spawn per game.")]
		public int MaxSpawns { get; set; } = 1;

		[Description("The message announced by CASSIE when Serpents hand spawn.")]
		public string EntryAnnouncement { get; set; } = "SERPENTS HAND HASENTERED";
		[Description("The message announced by CASSIE when Chaos spawn.")]
		public string CiEntryAnnouncement { get; set; } = "";
		[Description("The broadcast sent to Serpents Hand when they spawn.")]
		public string SpawnBroadcast { get; set; } = "<size=60>You are <color=#03F555><b>Serpents Hand</b></color></size>\n<i>Help the <color=\"red\">SCPs</color> by killing all other classes!</i>";
		[Description("Determines role name seen in game:")]
		public string RoleName { get; set; } = "Serpent's Hand";

		[Description("Determines if friendly fire between Serpents Hand and SCPs is enabled.")]
		public bool FriendlyFire { get; set; } = false;
		[Description("Determines if Serpents Hand should teleport to SCP-106 after exiting his pocket dimension.")]
		public bool TeleportTo106 { get; set; } = true;
		[Description("Determines if Serpents Hand should be able to hurt SCPs after the round ends.")]
		public bool EndRoundFriendlyFire { get; set; } = false;
		[Description("[IMPORTANT] Set this config to false if Chaos and SCPs CANNOT win together on your server.")]
		public bool ScpsWinWithChaos { get; set; } = true;
    }
}
