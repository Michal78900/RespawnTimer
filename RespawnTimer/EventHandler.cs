namespace RespawnTimer
{
    using API.Extensions;
    using Exiled.API.Features;
    using System.Collections.Generic;
    using MEC;
    using System.Text;
    using NorthwoodLib.Pools;

    using static API.API;

    public static class EventHandler
    {
        private static CoroutineHandle _timerCoroutine;
        
        internal static void OnRoundStart()
        {
            if (_timerCoroutine.IsRunning)
                Timing.KillCoroutines(_timerCoroutine);
            
            _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());

            Log.Debug($"RespawnTimer coroutine started successfully!", RespawnTimer.Singleton.Config.Debug);
        }

        private static IEnumerator<float> TimerCoroutine()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);
                
                StringBuilder builder = StringBuilderPool.Shared.Rent(!Respawn.IsSpawning ? TimerView.BeforeRespawnString : TimerView.DuringRespawnString);
                List<Player> spectators = ListPool<Player>.Shared.Rent(Player.Get(x => x.Role.Team == Team.RIP && !x.IsOverwatchEnabled));

                foreach (Player player in spectators)
                {
                    if (TimerHidden.Contains(player.UserId))
                        continue;

                    player.ShowHint(StringBuilderPool.Shared.ToStringReturn(builder.SetAllProperties(spectators.Count)), 1.25f);
                }
                
                ListPool<Player>.Shared.Return(spectators);
            }
        }

        /*
        private static IEnumerator<float> Timer()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(1f);

                try
                {
                    if (!Respawn.IsSpawning && Config.ShowTimerOnlyOnSpawn)
                        continue;

                    StringBuilder builder = StringBuilderPool.Shared.Rent();

                    builder.AppendFormat("{0}\n", Translation.YouWillRespawnIn);

                    if (Config.ShowMinutes)
                        builder.Append(Translation.Minutes);

                    if (Config.ShowSeconds)
                        builder.Append(Translation.Seconds);

                    builder.AppendLine();

                    if (Respawn.NextKnownTeam != SpawnableTeamType.None)
                    {
                        builder.Append(Translation.YouWillSpawnAs);

                        if (Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                        {
                            builder.Append(Translation.Ntf);

                            if (UiuSpawnable)
                                builder.Replace(Translation.Ntf, Translation.Uiu);
                        }
                        else
                        {
                            builder.Append(Translation.Ci);

                            if (SerpentsHandSpawnable)
                                builder.Replace(Translation.Ci, Translation.Sh);
                        }
                    }

                    // builder.Append('\n', 14 - Config.TextLowering - Convert.ToInt32(Config.ShowNumberOfSpectators));
                    builder.Append("<size=75%>");

                    if (Config.ShowWarheadStatus)
                    {
                        builder.AppendFormat("<align=left>{0} ", Translation.Warhead);
                        builder.Append("{warhead_status}</align>\n");
                    }

                    List<Player> spectators = ListPool<Player>.Shared.Rent(Player.Get(x => x.Role.Team == Team.RIP && !x.IsOverwatchEnabled));

                    if (Config.ShowNumberOfSpectators)
                    {
                        builder.AppendFormat("<align=right>{0} {1}\n</align>", Translation.Spectators, Translation.SpectatorsNum);
                    }

                    if (Config.ShowTickets)
                    {
                        builder.AppendFormat("<align=right>{0} {1}</align>\n", Translation.NtfTickets, Translation.NtfTicketsNum);
                        builder.AppendFormat("<align=right>{0} {1}</align>", Translation.CiTickets, Translation.CiTicketsNum);
                    }

                    builder.Append("</size>");
                    builder.Replace(@"\n", "\n");

                    string message = StringBuilderPool.Shared.ToStringReturn(builder.SetAllProperties(spectators.Count));

                    foreach (Player player in spectators)
                    {
                        if (TimerHidden.Contains(player.UserId))
                            continue;

                        player.ShowHint(message, 1.1f);
                    }

                    ListPool<Player>.Shared.Return(spectators);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        */

        // private static readonly Translation Translation = RespawnTimer.Singleton.Translation;
        // private static readonly Config Config = RespawnTimer.Singleton.Config;
    }
}

