namespace RespawnTimer_NorthwoodAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API.Features;
    using Configs;
    using Hints;
    using MEC;
    using PlayerRoles;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using Random = UnityEngine.Random;

    public class EventHandler
    {
        private CoroutineHandle _timerCoroutine;
        private CoroutineHandle _hintsCoroutine;

        [PluginEvent(ServerEventType.MapGenerated)]
        internal void OnGenerated()
        {
            /*
            if (RespawnTimer.Singleton.Config.ReloadTimerEachRound)
                RespawnTimer.Singleton.OnReloaded();
                */

            if (RespawnTimer.Singleton.Config.Timers.IsEmpty())
            {
                Log.Error("Timer list is empty!");
                return;
            }

            TimerView.CachedTimers.Clear();

            foreach (string name in RespawnTimer.Singleton.Config.Timers.Values)
                TimerView.AddTimer(name);

            if (_timerCoroutine.IsRunning)
                Timing.KillCoroutines(_timerCoroutine);
            
            if (_hintsCoroutine.IsRunning)
                Timing.KillCoroutines(_hintsCoroutine);
        }

        [PluginEvent(ServerEventType.RoundStart)]
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

            Log.Debug($"RespawnTimer coroutine started successfully!", RespawnTimer.Singleton.Config.Debug);
        }


        [PluginEvent(ServerEventType.PlayerDeath)]
        internal void OnDying(Player victim, Player _, DamageHandlerBase __)
        {
            if (RespawnTimer.Singleton.Config.TimerDelay < 0)
                return;

            if (PlayerDeathDictionary.ContainsKey(victim))
            {
                Timing.KillCoroutines(PlayerDeathDictionary[victim]);
                PlayerDeathDictionary.Remove(victim);
            }

            PlayerDeathDictionary.Add(victim, Timing.CallDelayed(RespawnTimer.Singleton.Config.TimerDelay, () => PlayerDeathDictionary.Remove(victim)));
        }

        private IEnumerator<float> TimerCoroutine()
        {
            do
            {
                yield return Timing.WaitForSeconds(1f);
                int specNum = Player.GetPlayers().Count(x => !x.IsAlive);

                foreach (Player player in Player.GetPlayers())
                {
                    if (player.IsAlive)
                        continue;

                    if (player.IsOverwatchEnabled && RespawnTimer.Singleton.Config.HideTimerForOverwatch)
                        continue;

                    if (API.API.TimerHidden.Contains(player.UserId))
                        continue;

                    if (PlayerDeathDictionary.ContainsKey(player))
                        continue;

                    if (!TimerView.TryGetTimerForPlayer(player, out TimerView timerView))
                        continue;

                    string text = timerView.GetText(specNum);

                    if (PlayerDeathDictionary.ContainsKey(player))
                        continue;

                    ShowHint(player, text, 1.25f);
                }
            } while (!RoundSummary.singleton._roundEnded);
        }
        
        private static IEnumerator<float> HintsCoroutine()
        {
            do
            {
                yield return Timing.WaitForSeconds(1f);

                foreach (TimerView timerView in TimerView.CachedTimers.Values)
                    timerView.IncrementHintInterval();
                
            } while (!RoundSummary.singleton._roundEnded);
        }

        public void ShowHint(Player player, string message, float duration = 3f)
        {
            HintParameter[] parameters =
            {
                new StringHintParameter(message)
            };

            player.ReferenceHub.networkIdentity.connectionToClient.Send(new HintMessage(new TextHint(message, parameters, durationScalar: duration)));
        }
        
        private static readonly Dictionary<Player, CoroutineHandle> PlayerDeathDictionary = new(25);
    }
}