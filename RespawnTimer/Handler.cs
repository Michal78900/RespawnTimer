using System;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using Respawning;
using SerpentsHand;

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

                if (RespawnManager.Singleton.NextKnownTeam == SpawnableTeamType.None && RespawnTimer.Instance.Config.ShowTimerOnlyOnSpawn) continue;

                text.Clear();

                int i = Mathf.RoundToInt(RespawnManager.Singleton._timeForNextSequence - (float)RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds);

                for (int n = RespawnTimer.Instance.Config.TextLowering; n > 0; n--) text.Append("\n");
                text.Append(RespawnTimer.Instance.Config.YouWillRespawnIn + "\n");
                text.Append(RespawnTimer.Instance.Config.Seconds + "\n");
                text.Replace("{seconds}", i.ToString());


                if (RespawnManager.Singleton.NextKnownTeam != SpawnableTeamType.None)
                {
                    text.Append(RespawnTimer.Instance.Config.YouWillSpawnAs);

                    //SH Support
                    if (RespawnTimer.ThereIsSH)
                    {
                        SerpentsHandTeam();
                    }
                    //
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

                    text.Replace("{ntf_tickets_num}", Respawning.RespawnTickets.Singleton.GetAvailableTickets(Respawning.SpawnableTeamType.NineTailedFox).ToString());
                    text.Replace("{ci_tickets_num}", Respawning.RespawnTickets.Singleton.GetAvailableTickets(Respawning.SpawnableTeamType.ChaosInsurgency).ToString());
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
            switch (RespawnManager.Singleton.NextKnownTeam)
            {
                case SpawnableTeamType.NineTailedFox: text.Append(RespawnTimer.Instance.Config.Ntf); break;
                case SpawnableTeamType.ChaosInsurgency:
                    {
                        if (SerpentsHand.EventHandlers.isSpawnable) text.Append(RespawnTimer.Instance.Config.Sh);
                        else text.Append(RespawnTimer.Instance.Config.Ci);
                        break;
                    }
            }
        }
    }
}

