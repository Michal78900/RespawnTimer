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

        internal static void OnWaitingForPlayers()
        {
            if (RespawnTimer.Singleton.Config.ReloadTimerEachRound)
                RespawnTimer.Singleton.OnReloaded();
        }

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
    }
}