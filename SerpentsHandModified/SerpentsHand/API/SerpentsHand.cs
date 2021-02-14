using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace SerpentsHand.API
{
	public static class SerpentsHand
	{
		public static void SpawnPlayer(Player player, bool full = true)
		{
			EventHandlers.SpawnPlayer(player, full);
		}

		public static void SpawnSquad(List<Player> playerList)
		{
			EventHandlers.SpawnSquad(playerList);
		}

		public static void SpawnSquad(int size)
		{
			EventHandlers.CreateSquad(size);
		}

		public static List<Player> GetSHPlayers()
		{
			return EventHandlers.shPlayers.Select(x => Player.Get(x)).ToList();
		}
	}
}
