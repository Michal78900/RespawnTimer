namespace RespawnTimer_NorthwoodAPI
{
    using MEC;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;

    public class EventHandler
    {
        private CoroutineHandle _timerCoroutine;

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        internal void OnWaitingForPlayers() => RespawnTimer_Base.EventHandler.OnWaitingForPlayers();

        [PluginEvent(ServerEventType.RoundStart)]
        internal void OnRoundStart() => RespawnTimer_Base.EventHandler.OnRoundStart();
    }
}