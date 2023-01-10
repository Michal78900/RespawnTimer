namespace RespawnTimer
{
    using System;
    using Exiled.API.Features;
    using System.Collections.Generic;
    using MEC;
    using API.Features;
    using NorthwoodLib.Pools;
    using PlayerRoles;

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

            try
            {
                _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }

            Log.Debug($"RespawnTimer coroutine started successfully!");
        }

        private static IEnumerator<float> TimerCoroutine()
        {
            yield return Timing.WaitForSeconds(1f);

            Log.Debug("Start");

            while (true)
            {
                yield return Timing.WaitForSeconds(1f);

                Log.Debug("Tick");

                List<Player> spectators = ListPool<Player>.Shared.Rent(Player.Get(x => x.Role.Team == Team.Dead));
                string text = TimerView.Current.GetText(spectators.Count);

                foreach (Player player in spectators)
                {
                    if ((player.IsOverwatchEnabled && RespawnTimer.Singleton.Config.HideTimerForOverwatch) || API.API.TimerHidden.Contains(player.UserId))
                        continue;

                    player.ShowHint(text, 1.25f);
                }

                ListPool<Player>.Shared.Return(spectators);

                 if (Round.IsEnded)
                    break;
            }
        }
    }
}