using System;
using System.Text;
using Exiled.API.Features;
using System.Collections.Generic;
using MEC;
using Respawning;

using EPlayer = Exiled.API.Features.Player;


namespace RespawnTimer
{
    class Handler
    {
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
            Log.Info($"RespawnTimer coroutine started successfully! The timer will be refreshed every {RespawnTimer.Instance.Config.Interval} second/s!");
        }

        private IEnumerator<float> Timer()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(RespawnTimer.Instance.Config.Interval);

                if (!Respawn.IsSpawning && RespawnTimer.Instance.Config.ShowTimerOnlyOnSpawn) continue;

                text.Clear();

                for (int n = RespawnTimer.Instance.Config.TextLowering; n > 0; n--) text.Append("\n");
                text.Append(RespawnTimer.Instance.Config.YouWillRespawnIn + "\n");
                text.Append(RespawnTimer.Instance.Config.Seconds + "\n");

                if (Respawn.IsSpawning)
                {
                    text.Replace("{seconds}", (Respawn.TimeUntilRespawn).ToString());
                }
                else
                    text.Replace("{seconds}", (Respawn.TimeUntilRespawn + 15).ToString());

                if (RespawnManager.Singleton.NextKnownTeam != SpawnableTeamType.None)
                {
                    text.Append(RespawnTimer.Instance.Config.YouWillSpawnAs);


                    if (RespawnTimer.ThereIsSH && RespawnManager.Singleton.NextKnownTeam == SpawnableTeamType.ChaosInsurgency) SerpentsHandTeam();
                    else
                    if (RespawnTimer.ThereIsUIU && RespawnManager.Singleton.NextKnownTeam == SpawnableTeamType.NineTailedFox) UIUTeam();

                    else
                    {
                        switch (RespawnManager.Singleton.NextKnownTeam)
                        {
                            case SpawnableTeamType.NineTailedFox: text.Append(RespawnTimer.Instance.Config.Ntf); break;
                            case SpawnableTeamType.ChaosInsurgency: text.Append(RespawnTimer.Instance.Config.Ci); break;
                        }
                    }
                }


                if (RespawnTimer.Instance.Config.ShowTickets)
                {
                    for (int n = 14 - RespawnTimer.Instance.Config.TextLowering; n > 0; n--) text.Append("\n");
                    text.Append($"<align=right>{RespawnTimer.Instance.Config.NtfTickets} {RespawnTimer.Instance.Config.NtfTicketsNum}</align>" +
                                $"\n<align=right>{RespawnTimer.Instance.Config.CiTickets} {RespawnTimer.Instance.Config.CiTicketsNum}</align>");

                    text.Replace("{ntf_tickets_num}", Respawn.NtfTickets.ToString());
                    text.Replace("{ci_tickets_num}", Respawn.ChaosTickets.ToString());

                }

                foreach (EPlayer ply in EPlayer.List)
                {
                    if (ply.Team == Team.RIP)
                    {
                        ply.ShowHint(text.ToString(), RespawnTimer.Instance.Config.Interval);
                    }
                }
            }
        }

        //SH Support
        public static void SerpentsHandTeam()
        {
            if (SerpentsHand.EventHandlers.isSpawnable) text.Append(RespawnTimer.Instance.Config.Sh);
            else text.Append(RespawnTimer.Instance.Config.Ci);
        }

        //UIU Support
        public static void UIUTeam()
        {
            if (UIURescueSquad.Handlers.EventHandlers.isSpawnable) text.Append(RespawnTimer.Instance.Config.Uiu);
            else text.Append(RespawnTimer.Instance.Config.Ntf);
        }
    }
}

