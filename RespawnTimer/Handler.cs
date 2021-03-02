using System;
using System.Text;
using System.Linq;
using Exiled.API.Features;
using System.Collections.Generic;
using MEC;
using Respawning;
using UnityEngine.Playables;

namespace RespawnTimer
{
    class Handler
    {
        private readonly RespawnTimer plugin;
        public Handler(RespawnTimer plugin) => this.plugin = plugin;

        CoroutineHandle timerCoroutine = new CoroutineHandle();

        static string text;


        public void OnRoundStart()
        {
            if(timerCoroutine.IsRunning)
            {
                Timing.KillCoroutines(timerCoroutine);
            }

            timerCoroutine = Timing.RunCoroutine(Timer());

            Log.Debug($"RespawnTimer coroutine started successfully! The timer will be refreshed every {plugin.Config.Interval} second/s!", plugin.Config.ShowDebugMessages);
        }

        private IEnumerator<float> Timer()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(plugin.Config.Interval);

                try
                {
                    if (Player.Get(Team.RIP).Count() == 0 || (!Respawn.IsSpawning && plugin.Config.ShowTimerOnlyOnSpawn)) continue;

                    text = string.Empty;

                    text += new string('\n', plugin.Config.TextLowering);

                    text += $"{plugin.Config.translations.YouWillRespawnIn}\n";


                    if (plugin.Config.ShowMinutes) text += plugin.Config.translations.Minutes;
                    if (plugin.Config.ShowSeconds) text += plugin.Config.translations.Seconds;

                    if (Respawn.IsSpawning)
                    {
                        if (plugin.Config.ShowMinutes) text = text.Replace("{minutes}", (Respawn.TimeUntilRespawn / 60).ToString());;

                        if (plugin.Config.ShowSeconds)
                        {
                            if (plugin.Config.ShowMinutes) text = text.Replace("{seconds}", (Respawn.TimeUntilRespawn % 60).ToString());

                            else text = text.Replace("{seconds}", Respawn.TimeUntilRespawn.ToString());
                        }
                    }
                    else
                    {
                        if (plugin.Config.ShowMinutes) text = text.Replace("{minutes}", ((Respawn.TimeUntilRespawn + 15) / 60).ToString());

                        if (plugin.Config.ShowSeconds)
                        {
                            if (plugin.Config.ShowMinutes) text = text.Replace("{seconds}", ((Respawn.TimeUntilRespawn + 15) % 60).ToString());

                            else text = text.Replace("{seconds}", Respawn.TimeUntilRespawn.ToString());
                        }
                    }
                    
                    text += "\n";

                    if (RespawnManager.Singleton.NextKnownTeam != SpawnableTeamType.None)
                    {
                        text += plugin.Config.translations.YouWillSpawnAs;

                        if (RespawnManager.Singleton.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                        {
                            text += plugin.Config.translations.Ntf;

                            if (RespawnTimer.assemblyUIU)
                                UIUTeam();
                        }
                        else
                        {
                            text += plugin.Config.translations.Ci;
                        }

                        if (RespawnTimer.assemblySH)
                            SerpentsHandTeam();
                    }


                    text += new string('\n', 14 - plugin.Config.TextLowering - Convert.ToInt32(plugin.Config.ShowNumberOfSpectators));

     
                    var Spectators = Player.Get(Team.RIP);
                    if (plugin.Config.ShowNumberOfSpectators)
                    {
                        text += $"<align=right>{plugin.Config.translations.Spectators} {plugin.Config.translations.SpectatorsNum}\n</align>";
                        text = text.Replace("{spectators_num}", Spectators.Count().ToString());
                    }

                    if (plugin.Config.ShowTickets)
                    {
                        text += $"<align=right>{plugin.Config.translations.NtfTickets} {plugin.Config.translations.NtfTicketsNum}</align>" +
                                    $"\n<align=right>{plugin.Config.translations.CiTickets} {plugin.Config.translations.CiTicketsNum}</align>";


                        text = text.Replace("{ntf_tickets_num}", Respawn.NtfTickets.ToString());
                        text = text.Replace("{ci_tickets_num}", Respawn.ChaosTickets.ToString());
                    }   
                    

                    foreach (Player ply in Spectators)
                    {
                        ply.ShowHint(text, 0.01f + plugin.Config.Interval); //Adeed this extra 0.01 seconds to fix the flickering hints if your ping is higher than 0 ms
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }



        public void SerpentsHandTeam()
        {
            try
            {
                if (SerpentsHand.EventHandlers.isSpawnable)
                {
                    if (RespawnManager.Singleton.NextKnownTeam == SpawnableTeamType.ChaosInsurgency) text = text.Replace(plugin.Config.translations.Ci, plugin.Config.translations.Sh);
                    else text = text.Replace(plugin.Config.translations.Ntf, plugin.Config.translations.Sh);
                }
            }
            catch (Exception) { }
        }


        public void UIUTeam()
        {
            try
            {
                if (UIURescueSquad.Handlers.EventHandlers.isSpawnable) text = text.Replace(plugin.Config.translations.Ntf, plugin.Config.translations.Uiu);
            }
            catch (Exception) { }
        }
    }
}

