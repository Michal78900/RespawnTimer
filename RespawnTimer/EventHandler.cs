namespace RespawnTimer
{
    using System;
    using System.Linq;
    using Exiled.API.Features;
    using System.Collections.Generic;
    using MEC;
    using Respawning;

    using static API.API;

    public static class EventHandler
    {
        internal static void OnRoundStart()
        {
            if (timerCoroutine != null && timerCoroutine.IsRunning)
            {
                Timing.KillCoroutines(timerCoroutine);
            }

            timerCoroutine = Timing.RunCoroutine(Timer());

            Log.Debug($"RespawnTimer coroutine started successfully!", Config.ShowDebugMessages);
        }

        private static IEnumerator<float> Timer()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);

                try
                {
                    if (!Respawn.IsSpawning && Config.ShowTimerOnlyOnSpawn)
                        continue;

                    string text = string.Empty;

                    text += new string('\n', Config.TextLowering);

                    text += $"{Translation.YouWillRespawnIn}\n";

                    if (Config.ShowMinutes)
                        text += Translation.Minutes;

                    if (Config.ShowSeconds)
                        text += Translation.Seconds;

                    if (Respawn.IsSpawning)
                    {
                        if (Config.ShowMinutes)
                            text = text.Replace("{minutes}", (Respawn.TimeUntilRespawn / 60).ToString()); ;

                        if (Config.ShowSeconds)
                        {
                            if (Config.ShowMinutes)
                                text = text.Replace("{seconds}", (Respawn.TimeUntilRespawn % 60).ToString());

                            else
                                text = text.Replace("{seconds}", Respawn.TimeUntilRespawn.ToString());
                        }
                    }
                    else
                    {
                        if (Config.ShowMinutes)
                            text = text.Replace("{minutes}", ((Respawn.TimeUntilRespawn + 15) / 60).ToString());

                        if (Config.ShowSeconds)
                        {
                            if (Config.ShowMinutes)
                                text = text.Replace("{seconds}", ((Respawn.TimeUntilRespawn + 15) % 60).ToString());

                            else
                                text = text.Replace("{seconds}", (Respawn.TimeUntilRespawn + 15).ToString());
                        }
                    }

                    text += "\n";

                    if (Respawn.NextKnownTeam != SpawnableTeamType.None)
                    {
                        text += Translation.YouWillSpawnAs;

                        if (Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                        {
                            text += Translation.Ntf;

                            if (IsUIUTeamSpawnable())
                                text = text.Replace(Translation.Ntf, Translation.Uiu);
                        }
                        else
                        {
                            text += Translation.Ci;

                            if (IsSerpentsHandTeamSpawnable())
                                text = text.Replace(Translation.Ci, Translation.Sh);
                        }
                    }

                    text += new string('\n', 14 - Config.TextLowering - Convert.ToInt32(Config.ShowNumberOfSpectators));

                    List<Player> spectators = Player.Get(Team.RIP).ToList();

                    if (Config.ShowNumberOfSpectators)
                    {
                        text += $"<align=right>{Translation.Spectators} {Translation.SpectatorsNum}\n</align>";
                        text = text.Replace("{spectators_num}", spectators.Count.ToString());
                    }

                    if (Config.ShowTickets)
                    {
                        text += $"<align=right>{Translation.NtfTickets} {Translation.NtfTicketsNum}</align>\n" +
                                $"<align=right>{Translation.CiTickets} {Translation.CiTicketsNum}</align>";


                        text = text.Replace("{ntf_tickets_num}", Respawn.NtfTickets.ToString());
                        text = text.Replace("{ci_tickets_num}", Respawn.ChaosTickets.ToString());
                    }

                    foreach (Player player in spectators)
                    {
                        if (TimerHidden.Contains(player.UserId))
                            continue;

                        player.ShowHint(text, 1.1f);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private static bool IsSerpentsHandTeamSpawnable()
        {
            if (RespawnTimer.SerpentsHandAssembly == null)
                return false;

            return (bool)RespawnTimer.SerpentsHandAssembly.GetType("SerpentsHand.EventHandlers")?.GetField("IsSpawnable").GetValue(null);
        }

        private static bool IsUIUTeamSpawnable()
        {
            if (RespawnTimer.UIURescueSquadAssembly == null)
                return false;

            return (bool)RespawnTimer.UIURescueSquadAssembly.GetType("UIURescueSquad.EventHandlers")?.GetField("IsSpawnable").GetValue(null);
        }

        private static CoroutineHandle timerCoroutine;

        private static readonly Translation Translation = RespawnTimer.Singleton.Translation;
        private static readonly Config Config = RespawnTimer.Singleton.Config;
    }
}

