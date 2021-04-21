namespace RespawnTimer
{
    using System;
    using System.Linq;
    using Exiled.API.Features;
    using System.Collections.Generic;
    using MEC;
    using Respawning;

    public class Handler
    {
        private readonly Config Config = RespawnTimer.Singleton.Config;

        private CoroutineHandle timerCoroutine = new CoroutineHandle();

        private string text;

        private List<Player> Spectators = new List<Player>();

        internal void OnRoundStart()
        {
            if (timerCoroutine.IsRunning)
            {
                Timing.KillCoroutines(timerCoroutine);
            }

            timerCoroutine = Timing.RunCoroutine(Timer());

            Log.Debug($"RespawnTimer coroutine started successfully! The timer will be refreshed every {Config.Interval} second/s!", Config.ShowDebugMessages);
        }

        private IEnumerator<float> Timer()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(Config.Interval);

                try
                {
                    if (!Respawn.IsSpawning && Config.ShowTimerOnlyOnSpawn)
                        continue;

                    text = string.Empty;

                    text += new string('\n', Config.TextLowering);

                    text += $"{Config.Translations.YouWillRespawnIn}\n";

                    if (Config.ShowMinutes)
                        text += Config.Translations.Minutes;

                    if (Config.ShowSeconds)
                        text += Config.Translations.Seconds;

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
                        text += Config.Translations.YouWillSpawnAs;

                        if (Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                        {
                            text += Config.Translations.Ntf;

                            if (RespawnTimer.IsyUIU)
                                UIUTeam();
                        }
                        else
                        {
                            text += Config.Translations.Ci;

                            if (RespawnTimer.IsSH)
                                SerpentsHandTeam();
                        }
                    }

                    text += new string('\n', 14 - Config.TextLowering - Convert.ToInt32(Config.ShowNumberOfSpectators));

                    Spectators = Player.Get(Team.RIP).ToList();

                    if (RespawnTimer.IsGS)
                        GhostSpectatorPlayers();

                    if (Config.ShowNumberOfSpectators)
                    {
                        text += $"<align=right>{Config.Translations.Spectators} {Config.Translations.SpectatorsNum}\n</align>";
                        text = text.Replace("{spectators_num}", Spectators.Count().ToString());
                    }

                    if (Config.ShowTickets)
                    {
                        text += $"<align=right>{Config.Translations.NtfTickets} {Config.Translations.NtfTicketsNum}</align>" +
                                    $"\n<align=right>{Config.Translations.CiTickets} {Config.Translations.CiTicketsNum}</align>";


                        text = text.Replace("{ntf_tickets_num}", Respawn.NtfTickets.ToString());
                        text = text.Replace("{ci_tickets_num}", Respawn.ChaosTickets.ToString());
                    }

                    foreach (Player ply in Spectators)
                    {
                        ply.ShowHint(text, 0.01f + Config.Interval);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void SerpentsHandTeam()
        {
            try
            {
                if (SerpentsHand.EventHandlers.IsSpawnable)
                    text = text.Replace(Config.Translations.Ci, Config.Translations.Sh);
            }
            catch (Exception) { }
        }

        private void UIUTeam()
        {
            try
            {
                if (UIURescueSquad.EventHandlers.IsSpawnable)
                    text = text.Replace(Config.Translations.Ntf, Config.Translations.Uiu);
            }
            catch (Exception) { }
        }

        private void GhostSpectatorPlayers()
        {
            try
            {
                Spectators.AddRange(GhostSpectator.GhostSpectator.Ghosts);
            }
            catch (Exception) { }
        }
    }
}

