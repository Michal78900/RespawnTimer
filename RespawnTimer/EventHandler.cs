namespace RespawnTimer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MEC;
    using API.Features;
#if EXILED
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Player;
#else
    using Utils.NonAllocLINQ;
    using Hints;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
#endif

    public class EventHandler
    {
        private CoroutineHandle _timerCoroutine;
        private CoroutineHandle _hintsCoroutine;

#if NWAPI
        [PluginEvent(ServerEventType.MapGenerated)]
#endif
        internal void OnGenerated()
        {
#if EXILED
            if (RespawnTimer.Singleton.Config.ReloadTimerEachRound)
                RespawnTimer.Singleton.OnReloaded();
#else
            if (RespawnTimer.Singleton.Config.Timers.IsEmpty())
            {
                Log.Error("Timer list is empty!");
                return;
            }

            TimerView.CachedTimers.Clear();

            foreach (string name in RespawnTimer.Singleton.Config.Timers.Values)
                TimerView.AddTimer(name);
#endif

            if (_timerCoroutine.IsRunning)
                Timing.KillCoroutines(_timerCoroutine);

            if (_hintsCoroutine.IsRunning)
                Timing.KillCoroutines(_hintsCoroutine);
        }

#if NWAPI
        [PluginEvent(ServerEventType.RoundStart)]
#endif
        internal void OnRoundStart()
        {
            try
            {
                _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());
                _hintsCoroutine = Timing.RunCoroutine(HintsCoroutine());
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

#if EXILED
            Log.Debug("RespawnTimer coroutine started successfully!");
#else
            Log.Debug("RespawnTimer coroutine started successfully!", RespawnTimer.Singleton.Config.Debug);
#endif
        }

#if NWAPI
        [PluginEvent(ServerEventType.PlayerDeath)]
        internal void OnDying(Player victim, Player _, DamageHandlerBase __)
#else
        internal void OnDying(DyingEventArgs ev)
#endif
        {
            if (RespawnTimer.Singleton.Config.TimerDelay < 0)
                return;

#if EXILED
            if (PlayerDeathDictionary.ContainsKey(ev.Player))
            {
                Timing.KillCoroutines(PlayerDeathDictionary[ev.Player]);
                PlayerDeathDictionary.Remove(ev.Player);
            }

            PlayerDeathDictionary.Add(ev.Player, Timing.CallDelayed(RespawnTimer.Singleton.Config.TimerDelay, () => PlayerDeathDictionary.Remove(ev.Player)));
#else
            if (PlayerDeathDictionary.ContainsKey(victim))
            {
                Timing.KillCoroutines(PlayerDeathDictionary[victim]);
                PlayerDeathDictionary.Remove(victim);
            }

            PlayerDeathDictionary.Add(victim, Timing.CallDelayed(RespawnTimer.Singleton.Config.TimerDelay, () => PlayerDeathDictionary.Remove(victim)));
#endif
        }

        private IEnumerator<float> TimerCoroutine()
        {
            yield return Timing.WaitForSeconds(1f);

            while (true)
            {
                yield return Timing.WaitForSeconds(1f);
#if EXILED
                int specNum = Player.List.Count(x => !x.IsAlive || x.SessionVariables.ContainsKey("IsGhost"));
                foreach (Player player in Player.List)
#else
                int specNum = Player.GetPlayers().Count(x => !x.IsAlive);
                foreach (Player player in Player.GetPlayers())
#endif
                {
                    try
                    {
#if EXILED
                        if (player.IsAlive && !player.SessionVariables.ContainsKey("IsGhost"))
                            continue;
#else
                        if (player.IsAlive)
                            continue;
#endif

                        if (player.IsOverwatchEnabled && RespawnTimer.Singleton.Config.HideTimerForOverwatch)
                            continue;

                        if (API.API.TimerHidden.Contains(player.UserId))
                            continue;

                        if (PlayerDeathDictionary.ContainsKey(player))
                            continue;

                        if (!TimerView.TryGetTimerForPlayer(player, out TimerView timerView))
                            continue;

                        string text = timerView.GetText(specNum);

#if EXILED
                        player.ShowHint(text, 1.25f);
#else
                        ShowHint(player, text, 1.25f);
#endif
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                    }
                }

                if (RoundSummary.singleton._roundEnded)
                    break;
            }
        }

        private IEnumerator<float> HintsCoroutine()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1f);

                foreach (TimerView timerView in TimerView.CachedTimers.Values)
                    timerView.IncrementHintInterval();

                if (RoundSummary.singleton._roundEnded)
                    break;
            }
        }

#if NWAPI
        private void ShowHint(Player player, string message, float duration = 3f)
        {
            HintParameter[] parameters =
            {
                new StringHintParameter(message)
            };

            player.ReferenceHub.networkIdentity.connectionToClient.Send(new HintMessage(new TextHint(message, parameters, durationScalar: duration)));
        }
#endif

        private readonly Dictionary<Player, CoroutineHandle> PlayerDeathDictionary = new(25);
    }
}