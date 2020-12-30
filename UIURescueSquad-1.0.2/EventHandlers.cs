using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace UIURescueSquad.Handlers
{
    public class EventHandlers
    {
        public static bool isSpawnable;

        public static List<int> uiuPlayers = new List<int>();

        private int respawns = 0;
        private int randnums;

        private static System.Random rand = new System.Random();

        private static Vector3 SpawnPos = new Vector3(170, 985, 29);
        //NOTE: Make spawnpos configurable
        private string rank;

        public void OnWaitingForPlayers()
        {
            uiuPlayers.Clear();
            respawns = 0;
        }

        public void IsSpawnable()
        {
            randnums = rand.Next(1, 101);
            if (randnums <= UIURescueSquad.Instance.Config.probability && respawns >= UIURescueSquad.Instance.Config.respawns) isSpawnable = true;
            else isSpawnable = false;
        }

        public void OnTeamRespawn(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox)
            {
                //randnums = rand.Next(1, 101);
                //Log.Info(randnums);
                //if (randnums <= UIURescueSquad.Instance.Config.probability & respawns >= UIURescueSquad.Instance.Config.respawns)
                if(isSpawnable)
                {
                    if (UIURescueSquad.Instance.Config.AnnouncementText != null)
                    {
                        if (UIURescueSquad.Instance.Config.AnnouncementText != null && UIURescueSquad.Instance.Config.AnnouncementText != null)
                        {
                            Map.ClearBroadcasts();
                            Map.Broadcast(UIURescueSquad.Instance.Config.AnnouncementTime, UIURescueSquad.Instance.Config.AnnouncementText);
                        }
                    }
                    //Cassie.Message("Attention, the U I U HasEntered please help the MtfUnit that are AwaitingRecontainment .g7 ScpSubjects", true, true);
                    //NOTE: disable MTF entrance message to allow cassie messages
                    foreach (Player player in ev.Players)
                    {
                        uiuPlayers.Add(player.Id);
                        if (UIURescueSquad.Instance.Config.UIUBroadcast != null && UIURescueSquad.Instance.Config.UIUBroadcastTime.ToString() != null)
                        {
                            if (UIURescueSquad.Instance.Config.UseHintsHere)
                            {
                                player.ClearBroadcasts();
                                player.ShowHint(UIURescueSquad.Instance.Config.UIUBroadcast, UIURescueSquad.Instance.Config.UIUBroadcastTime);
                            }
                            else
                            {
                                player.ClearBroadcasts();
                                player.Broadcast(UIURescueSquad.Instance.Config.UIUBroadcastTime, UIURescueSquad.Instance.Config.UIUBroadcast);
                            }
                        }
                        Timing.CallDelayed(0.01f, () => {
                            switch (player.Role)
                            {
                                case RoleType.NtfCadet:
                                    player.Health = UIURescueSquad.Instance.Config.UIUSoldierLife;
                                    Timing.CallDelayed(0.4f, () => { player.Position = SpawnPos; });
                                    player.ResetInventory(UIURescueSquad.Instance.Config.UIUSoldierInventory);
                                    //NOTE: Add possibilities for the inventory system
                                    player.BadgeHidden = false;
                                    player.RankName = UIURescueSquad.Instance.Config.UIUSoldierRank;
                                    player.RankColor = "yellow";
                                    break;
                                case RoleType.NtfLieutenant:
                                    player.Health = UIURescueSquad.Instance.Config.UIUAgentLife;
                                    Timing.CallDelayed(0.4f, () => { player.Position = SpawnPos; });
                                    player.ResetInventory(UIURescueSquad.Instance.Config.UIUAgentInventory);
                                    player.BadgeHidden = false;
                                    player.RankName = UIURescueSquad.Instance.Config.UIUAgentRank;
                                    player.RankColor = "yellow";
                                    break;
                                case RoleType.NtfCommander:
                                    player.Health = UIURescueSquad.Instance.Config.UIULeaderLife;
                                    Timing.CallDelayed(0.4f, () => { player.Position = SpawnPos; });
                                    player.ResetInventory(UIURescueSquad.Instance.Config.UIULeaderInventory);
                                    player.BadgeHidden = false;
                                    player.RankName = UIURescueSquad.Instance.Config.UIULeaderRank;
                                    player.RankColor = "yellow";
                                    break;
                            }
                        });
                    }
                }
            respawns++;
            }
        }

        public void OnAnnouncingMTF(AnnouncingNtfEntranceEventArgs ev)
        {
            if (isSpawnable && respawns >= UIURescueSquad.Instance.Config.respawns + 1)
            {
                ev.IsAllowed = false;
                if (UIURescueSquad.Instance.Config.DisableNTFAnnounce)
                {
                    Cassie.Message(UIURescueSquad.Instance.Config.AnnouncementCassie);
                }
            }
        }

        public void OnDying(DiedEventArgs ev)
        {
            if (uiuPlayers.Contains(ev.Target.Id))
            {
                //NOTE: Fix this shit fix
                //Block users from using hidetag/showtag beeing UIU
                uiuPlayers.Remove(ev.Target.Id);
                ev.Target.ReferenceHub.serverRoles.NetworkMyText = rank;
                ev.Target.ReferenceHub.serverRoles.NetworkMyColor = "default";
                ev.Target.BadgeHidden = false;
            }
        }
        public void OnChanging(ChangingRoleEventArgs ev)
        {
            if (uiuPlayers.Contains(ev.Player.Id))
            {
                //NOTE: Fix this shit fix
                uiuPlayers.Remove(ev.Player.Id);
                ev.Player.ReferenceHub.serverRoles.NetworkMyText = rank;
                ev.Player.ReferenceHub.serverRoles.NetworkMyColor = "default";
                ev.Player.BadgeHidden = false;
            }
        }
    }
}
