using System;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using Respawning;

using EPlayer = Exiled.API.Features.Player;


namespace RespawnTimer
{
    class Handler
    {
        public List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

        StringBuilder text = new StringBuilder();

        public void OnRoundStart()
        {
            foreach (CoroutineHandle coroutine in coroutines)
            {
                Timing.KillCoroutines(coroutine);
            }

            coroutines.Add(Timing.RunCoroutine(Timer()));
            Log.Info("RespawnTimer coroutine started successfully!");
        }

        private IEnumerator<float> Timer()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);

                if (RespawnManager.Singleton.NextKnownTeam == SpawnableTeamType.None && RespawnTimer.Instance.Config.ShowTimerOnlyOnSpawn) continue;

                text.Clear();

                int i = Mathf.RoundToInt(RespawnManager.Singleton._timeForNextSequence - (float)RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds);

                text.Append(RespawnTimer.Instance.Config.translations[0] + "\n");
                text.Append(RespawnTimer.Instance.Config.translations[7] + "\n");
                text.Replace("{seconds}", i.ToString());

                if (RespawnManager.Singleton.NextKnownTeam != SpawnableTeamType.None)
                {
                    text.Append(RespawnTimer.Instance.Config.translations[1]);

                    switch (RespawnManager.Singleton.NextKnownTeam)
                    {
                        case SpawnableTeamType.NineTailedFox: text.Append(RespawnTimer.Instance.Config.translations[2]); break;
                        case SpawnableTeamType.ChaosInsurgency: text.Append(RespawnTimer.Instance.Config.translations[3]); break;
                    }
                }

                if(RespawnTimer.Instance.Config.ShowTickets)
                {
                    text.Append($"\n\n\n\n\n\n\n\n\n\n\n\n\n\n<align=right>{RespawnTimer.Instance.Config.translations[4]} {RespawnTimer.Instance.Config.translations[8]}</align>" +
                                $"\n<align=right>{RespawnTimer.Instance.Config.translations[5]} {Respawning.RespawnTickets.Singleton.GetAvailableTickets(Respawning.SpawnableTeamType.ChaosInsurgency)}</align>");

                    text.Replace("{ntf_tickets_num}", Respawning.RespawnTickets.Singleton.GetAvailableTickets(Respawning.SpawnableTeamType.NineTailedFox).ToString());
                    text.Replace("{ci_tickets_num}", Respawning.RespawnTickets.Singleton.GetAvailableTickets(Respawning.SpawnableTeamType.ChaosInsurgency).ToString());
                }

                foreach (EPlayer ply in EPlayer.List)
                {
                    if (ply.Team == Team.RIP)
                    {
                        ply.ShowHint(text.ToString(), 1f);
                    }
                }
            }
        }
    }
}

