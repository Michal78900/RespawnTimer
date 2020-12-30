using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using scp035.API;

namespace SerpentsHand
{
    partial class EventHandlers
    {
        internal static void SpawnPlayer(Player player, bool full = true)
        {
            shPlayers.Add(player.Id);
            player.SetRole(RoleType.Tutorial, true);
            player.Position = shSpawnPos;
            player.Broadcast(10, SerpentsHand.instance.Config.SpawnBroadcast);
            if (full)
            {
                player.Ammo[(int)AmmoType.Nato556] = 250;
                player.Ammo[(int)AmmoType.Nato762] = 250;
                player.Ammo[(int)AmmoType.Nato9] = 250;

                for (int i = 0; i < SerpentsHand.instance.Config.SpawnItems.Count; i++)
                {
                    player.Inventory.AddNewItem((ItemType)SerpentsHand.instance.Config.SpawnItems[i]);
                }
                player.Health = SerpentsHand.instance.Config.Health;
                // Prevent Serpents Hand from taking up Chaos spawn tickets
                //Respawning.RespawnTickets.Singleton.GrantTickets(Respawning.SpawnableTeamType.ChaosInsurgency, 1);
            }

            //Timing.CallDelayed(0.3f, () => player.Position = shSpawnPos);
        }

        internal static void CreateSquad(int size)
        {
            List<Player> spec = new List<Player>();
            List<Player> pList = Player.List.ToList();

            foreach (Player player in pList)
            {
                if (player.Team == Team.RIP)
                {
                    spec.Add(player);
                }
            }

            int spawnCount = 1;
            while (spec.Count > 0 && spawnCount <= size)
            {
                int index = rand.Next(0, spec.Count);
                if (spec[index] != null)
                {
                    SpawnPlayer(spec[index]);
                    spec.RemoveAt(index);
                    spawnCount++;
                }
            }
        }

        internal static void SpawnSquad(List<Player> players)
        {
            foreach (Player player in players)
            {
                SpawnPlayer(player);
            }

            Cassie.Message(SerpentsHand.instance.Config.EntryAnnouncement, true, true);
        }

        internal static void GrantFF()
		{
            foreach (int id in shPlayers)
            {
                Player p = Player.Get(id);
                if (p != null) p.IsFriendlyFireEnabled = true;
            }

            foreach (int id in SerpentsHand.instance.EventHandlers.shPocketPlayers)
            {
                Player p = Player.Get(id);
                if (p != null) p.IsFriendlyFireEnabled = true;
            }

            shPlayers.Clear();
            SerpentsHand.instance.EventHandlers.shPocketPlayers.Clear();
        }

        private Player TryGet035()
        {
            return Scp035Data.GetScp035();
        }

        private int CountRoles(Team team)
        {
            Player scp035 = null;

            if (SerpentsHand.isScp035)
            {
                scp035 = TryGet035();
            }

            int count = 0;
            foreach (Player pl in Player.List)
            {
                if (pl.Team == team)
                {
                    if (scp035 != null && pl.Id == scp035.Id) continue;
                    count++;
                }
            }
            return count;
        }

        private void TeleportTo106(Player player)
        {
            Player scp106 = Player.List.Where(x => x.Role == RoleType.Scp106).FirstOrDefault();
            if (scp106 != null)
            {
                player.Position = scp106.Position;
            }
            else
            {
                player.Position = Map.GetRandomSpawnPoint(RoleType.Scp096);
            }
        }

        private Team GetTeam(RoleType roleType)
        {
            switch (roleType)
            {
                case RoleType.ChaosInsurgency:
                    return Team.CHI;
                case RoleType.Scientist:
                    return Team.RSC;
                case RoleType.ClassD:
                    return Team.CDP;
                case RoleType.Scp049:
                case RoleType.Scp93953:
                case RoleType.Scp93989:
                case RoleType.Scp0492:
                case RoleType.Scp079:
                case RoleType.Scp096:
                case RoleType.Scp106:
                case RoleType.Scp173:
                    return Team.SCP;
                case RoleType.Spectator:
                    return Team.RIP;
                case RoleType.FacilityGuard:
                case RoleType.NtfCadet:
                case RoleType.NtfLieutenant:
                case RoleType.NtfCommander:
                case RoleType.NtfScientist:
                    return Team.MTF;
                case RoleType.Tutorial:
                    return Team.TUT;
                default:
                    return Team.RIP;
            }
        }
    }
}