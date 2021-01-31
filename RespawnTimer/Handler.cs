using System;
using System.Text;
using System.Linq;
using Exiled.API.Features;
using System.Collections.Generic;
using MEC;
using Respawning;


namespace RespawnTimer
{
    class Handler
    {
        private readonly RespawnTimer plugin;
        public Handler(RespawnTimer plugin) => this.plugin = plugin;

        public List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

        static StringBuilder text = new StringBuilder();


        public void OnRoundStart()
        {
            foreach (CoroutineHandle coroutine in coroutines)
            {
                Timing.KillCoroutines(coroutine);
            }
            coroutines.Clear();

            coroutines.Add(Timing.RunCoroutine(Timer()));
            Log.Debug($"RespawnTimer coroutine started successfully! The timer will be refreshed every {plugin.Config.Interval} second/s!", plugin.Config.ShowDebugMessages);
        }

        private IEnumerator<float> Timer()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(plugin.Config.Interval);

                if (!Player.Get(Team.RIP).Any()) continue;

                if (!Respawn.IsSpawning && plugin.Config.ShowTimerOnlyOnSpawn) continue;

                text.Clear();

                for (int n = plugin.Config.TextLowering; n > 0; n--) text.Append("\n");
                text.Append(plugin.Config.YouWillRespawnIn + "\n");

                if (plugin.Config.ShowMinutes) text.Append(plugin.Config.Minutes);
                if (plugin.Config.ShowSeconds) text.Append(plugin.Config.Seconds);

                if (Respawn.IsSpawning)
                {
                    if (plugin.Config.ShowMinutes) text.Replace("{minutes}", (Respawn.TimeUntilRespawn / 60).ToString());

                    if (plugin.Config.ShowSeconds)
                    {
                        if (plugin.Config.ShowMinutes) text.Replace("{seconds}", ((Respawn.TimeUntilRespawn % 60)).ToString());

                        else text.Replace("{seconds}", (Respawn.TimeUntilRespawn.ToString()));
                    }
                }
                else
                {
                    if (plugin.Config.ShowMinutes) text.Replace("{minutes}", ((Respawn.TimeUntilRespawn + 15) / 60).ToString());

                    if (plugin.Config.ShowSeconds)
                    {
                        if (plugin.Config.ShowMinutes) text.Replace("{seconds}", ((Respawn.TimeUntilRespawn + 15) % 60).ToString());

                        else text.Replace("{seconds}", (Respawn.TimeUntilRespawn + 15).ToString());
                    }
                }

                text.Append("\n");

                if (RespawnManager.Singleton.NextKnownTeam != SpawnableTeamType.None)
                {
                    text.Append(plugin.Config.YouWillSpawnAs);

                    if (RespawnManager.Singleton.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                    {
                        text.Append(plugin.Config.Ntf);

                        UIUTeam();
                    }

                    else
                    {
                        text.Append(plugin.Config.Ci);

                        SerpentsHandTeam();
                    }
                }


                if (plugin.Config.ShowTickets)
                {
                    for (int n = 14 - plugin.Config.TextLowering; n > 0; n--) text.Append("\n");
                    text.Append($"<align=right>{plugin.Config.NtfTickets} {plugin.Config.NtfTicketsNum}</align>" +
                                $"\n<align=right>{plugin.Config.CiTickets} {plugin.Config.CiTicketsNum}</align>");

                    text.Replace("{ntf_tickets_num}", Respawn.NtfTickets.ToString());
                    text.Replace("{ci_tickets_num}", Respawn.ChaosTickets.ToString());

                }

                foreach (Player ply in Player.List.Where(p => p.Team == Team.RIP))
                {
                    ply.ShowHint(text.ToString(), plugin.Config.Interval);
                }
            }
        }


        public void SerpentsHandTeam()
        {
            try
            {
                if (SerpentsHand.EventHandlers.isSpawnable) text.Replace(plugin.Config.Ci, plugin.Config.Sh);
            }
            catch (Exception) { }
        }


        public void UIUTeam()
        {
            try
            {
                if (UIURescueSquad.Handlers.EventHandlers.isSpawnable) text.Replace(plugin.Config.Ntf, plugin.Config.Uiu);
            }
            catch (Exception) { }
        }
    }
}

