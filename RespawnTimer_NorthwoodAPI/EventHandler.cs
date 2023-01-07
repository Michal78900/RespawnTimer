namespace RespawnTimer_NorthwoodAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using API.Features;
    using Hints;
    using MEC;
    using NorthwoodLib.Pools;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using Random = UnityEngine.Random;

    public class EventHandler
    {
        private CoroutineHandle _timerCoroutine;

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        internal void OnWaitingForPlayers()
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

            string chosenTimerName = RespawnTimer.Singleton.Config.Timers[Random.Range(0, RespawnTimer.Singleton.Config.Timers.Count)];
            TimerView.GetNew(chosenTimerName);
        }

        [PluginEvent(ServerEventType.RoundStart)]
        internal void OnRoundStart()
        {
            if (_timerCoroutine.IsRunning)
                Timing.KillCoroutines(_timerCoroutine);

            _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());

            Log.Debug($"RespawnTimer coroutine started successfully!", RespawnTimer.Singleton.Config.Debug);
        }

        private IEnumerator<float> TimerCoroutine()
        {
            do
            {
                yield return Timing.WaitForSeconds(1f);

                List<Player> spectators = ListPool<Player>.Shared.Rent(ReferenceHub.AllHubs.Select(Player.Get).Where(x => !x.IsServer && x.Role == RoleTypeId.Spectator));
                string text = TimerView.Current.GetText(spectators.Count);

                foreach (Player player in spectators)
                {
                    if (player.Role == RoleTypeId.Overwatch && RespawnTimer.Singleton.Config.HideTimerForOverwatch || API.API.TimerHidden.Contains(player.UserId))
                        continue;

                    ShowHint(player, text, 1.25f);
                }

                ListPool<Player>.Shared.Return(spectators);
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
    }
}