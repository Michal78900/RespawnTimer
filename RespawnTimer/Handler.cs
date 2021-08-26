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
        internal void OnRoundStart()
        {
            if (timerCoroutine.IsRunning)
            {
                Timing.KillCoroutines(timerCoroutine);
            }

            timerCoroutine = Timing.RunCoroutine(Timer());

            Log.Debug($"RespawnTimer coroutine started successfully!", Config.ShowDebugMessages);
        }

        private IEnumerator<float> Timer()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);

                try
                {
                    if (!Respawn.IsSpawning && Config.ShowTimerOnlyOnSpawn)
                        continue;

                    Text = string.Empty;

                    Text += new string('\n', Config.TextLowering);

                    Text += $"{Translation.YouWillRespawnIn}\n";

                    if (Config.ShowMinutes)
                        Text += Translation.Minutes;

                    if (Config.ShowSeconds)
                        Text += Translation.Seconds;

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
                        Text += Translation.YouWillSpawnAs;

                        if (Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                        {
                            Text += Translation.Ntf;

                            if (IsUIUTeamSpawnable())
                                Text = Text.Replace(Translation.Ntf, Translation.Uiu);
                        }
                        else
                        {
                            Text += Translation.Ci;

                            if (IsSerpentsHandTeamSpawnable())
                                Text = Text.Replace(Translation.Ci, Translation.Sh);
                        }
                    }

                    Text += new string('\n', 14 - Config.TextLowering - Convert.ToInt32(Config.ShowNumberOfSpectators));

                    Spectators = Player.Get(Team.RIP).ToList();

                    if (Config.ShowNumberOfSpectators)
                    {
                        Text += $"<align=right>{Translation.Spectators} {Translation.SpectatorsNum}\n</align>";
                        Text = Text.Replace("{spectators_num}", Spectators.Count().ToString());
                    }

                    if (Config.ShowTickets)
                    {
                        Text += $"<align=right>{Translation.NtfTickets} {Translation.NtfTicketsNum}</align>\n" +
                                $"<align=right>{Translation.CiTickets} {Translation.CiTicketsNum}</align>";


                        Text = Text.Replace("{ntf_tickets_num}", Respawn.NtfTickets.ToString());
                        Text = Text.Replace("{ci_tickets_num}", Respawn.ChaosTickets.ToString());
                    }

                    foreach (Player player in Spectators.Where(x => !TimerHidden.Contains(x.UserId)))
                    {
                        player.ShowHint(Text, 1.1f);
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

        #region Variables

        public static List<string> TimerHidden = new List<string>();

        public static string Text;

        private List<Player> Spectators = new List<Player>();

        private CoroutineHandle timerCoroutine = new CoroutineHandle();

        private static readonly Translation Translation = RespawnTimer.Singleton.Translation;
        private static readonly Config Config = RespawnTimer.Singleton.Config;

        #endregion
    }
}

