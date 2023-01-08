namespace RespawnTimer_Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hints;
    using MEC;
    using PlayerRoles;
    using Random = UnityEngine.Random;
    using static API;

    public static class EventHandler
    {
        private static CoroutineHandle _timerCoroutine;

        public static void OnWaitingForPlayers()
        {
            /*
            if (RespawnTimer.Singleton.Config.ReloadTimerEachRound)
                RespawnTimer.Singleton.OnReloaded();
            */


            if (Config.Timers.IsEmpty())
            {
                ServerConsole.AddLog("Timer list is empty!", ConsoleColor.Red);
                // Log.Error("Timer list is empty!");
                return;
            }

            string chosenTimerName = Config.Timers[Random.Range(0, Config.Timers.Count)];
            TimerView.GetNew(chosenTimerName);
        }

        public static void OnRoundStart()
        {
            /*
            if (_timerCoroutine.IsRunning)
                Timing.KillCoroutines(_timerCoroutine);

            _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());

            Log.Debug($"RespawnTimer coroutine started successfully!", RespawnTimer.Singleton.Config.Debug);
            */
        }

        private static IEnumerator<float> TimerCoroutine()
        {
            do
            {
                yield return Timing.WaitForSeconds(1f);

                Spectators.Clear();
                Spectators.AddRange(ReferenceHub.AllHubs.Where(x => x.characterClassManager.InstanceMode == ClientInstanceMode.ReadyClient));
                string text = TimerView.Current.GetText(Spectators.Count);

                foreach (ReferenceHub referenceHub in Spectators)
                {
                    if (referenceHub.roleManager.CurrentRole.RoleTypeId == RoleTypeId.Overwatch && true || TimerHidden.Contains(referenceHub.characterClassManager.UserId))
                        continue;

                    /*
                    if (player.Role == RoleTypeId.Overwatch && RespawnTimer.Singleton.Config.HideTimerForOverwatch || API.API.TimerHidden.Contains(player.UserId))
                        continue;
                    */

                    ShowHint(referenceHub, text, 1.25f);
                }
            } while (!RoundSummary.singleton._roundEnded);
        }

        private static void ShowHint(ReferenceHub referenceHub, string message, float duration = 3f)
        {
            HintParameter[] parameters =
            {
                new StringHintParameter(message)
            };

            referenceHub.networkIdentity.connectionToClient.Send(new HintMessage(new TextHint(message, parameters, durationScalar: duration)));
        }

        private static readonly List<ReferenceHub> Spectators = new(25);
    }
}