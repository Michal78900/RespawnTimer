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
        public static List<string> TimerHidden = new List<string>();

        public static string Text;

        private List<Player> Spectators = new List<Player>();

        private CoroutineHandle timerCoroutine = new CoroutineHandle();

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

                    Text = string.Empty;

                    Text += new string('\n', Config.TextLowering);

                    Text += $"{Config.Translations.YouWillRespawnIn}\n";

                    if (Config.ShowMinutes)
                        Text += Config.Translations.Minutes;

                    if (Config.ShowSeconds)
                        Text += Config.Translations.Seconds;

                    if (Respawn.IsSpawning)
                    {
                        if (Config.ShowMinutes)
                            Text = Text.Replace("{minutes}", (Respawn.TimeUntilRespawn / 60).ToString()); ;

                        if (Config.ShowSeconds)
                        {
                            if (Config.ShowMinutes)
                                Text = Text.Replace("{seconds}", (Respawn.TimeUntilRespawn % 60).ToString());

                            else
                                Text = Text.Replace("{seconds}", Respawn.TimeUntilRespawn.ToString());
                        }
                    }
                    else
                    {
                        if (Config.ShowMinutes)
                            Text = Text.Replace("{minutes}", ((Respawn.TimeUntilRespawn + 15) / 60).ToString());

                        if (Config.ShowSeconds)
                        {
                            if (Config.ShowMinutes)
                                Text = Text.Replace("{seconds}", ((Respawn.TimeUntilRespawn + 15) % 60).ToString());

                            else 
                                Text = Text.Replace("{seconds}", (Respawn.TimeUntilRespawn + 15).ToString());
                        }
                    }

                    Text += "\n";

                    if (Respawn.NextKnownTeam != SpawnableTeamType.None)
                    {
                        Text += Config.Translations.YouWillSpawnAs;

                        if (Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                        {
                            Text += Config.Translations.Ntf;

                            if (IsUIUTeamSpawnable())
                                Text = Text.Replace(Config.Translations.Ntf, Config.Translations.Uiu);
                        }
                        else
                        {
                            Text += Config.Translations.Ci;

                            if (IsSerpentsHandTeamSpawnable())
                                Text = Text.Replace(Config.Translations.Ci, Config.Translations.Sh);
                        }
                    }

                    Text += new string('\n', 14 - Config.TextLowering - Convert.ToInt32(Config.ShowNumberOfSpectators));

                    Spectators = Player.Get(Team.RIP).ToList();

                    if (Config.ShowNumberOfSpectators)
                    {
                        Text += $"<align=right>{Config.Translations.Spectators} {Config.Translations.SpectatorsNum}\n</align>";
                        Text = Text.Replace("{spectators_num}", Spectators.Count().ToString());
                    }

                    if (Config.ShowTickets)
                    {
                        Text += $"<align=right>{Config.Translations.NtfTickets} {Config.Translations.NtfTicketsNum}</align>" +
                                    $"\n<align=right>{Config.Translations.CiTickets} {Config.Translations.CiTicketsNum}</align>";


                        Text = Text.Replace("{ntf_tickets_num}", Respawn.NtfTickets.ToString());
                        Text = Text.Replace("{ci_tickets_num}", Respawn.ChaosTickets.ToString());
                    }

                    foreach (Player ply in Spectators.Where(x => !TimerHidden.Contains(x.UserId)))
                    {
                        ply.ShowHint(Text, 0.01f + Config.Interval);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private bool IsSerpentsHandTeamSpawnable()
        {
            if (RespawnTimer.SerpentsHandAssembly == null)
                return false;

            return (bool)RespawnTimer.SerpentsHandAssembly.GetType("SerpentsHand.EventHandlers")?.GetField("IsSpawnable").GetValue(null);
        }

        private bool IsUIUTeamSpawnable()
        {
            if (RespawnTimer.UIURescueSquadAssembly == null)
                return false;

            return (bool)RespawnTimer.UIURescueSquadAssembly.GetType("UIURescueSquad.EventHandlers")?.GetField("IsSpawnable").GetValue(null);
        }

        private readonly Config Config = RespawnTimer.Singleton.Config;
    }
}

