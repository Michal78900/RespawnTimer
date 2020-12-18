using System;
using Exiled.API.Enums;
using Exiled.API.Features;

using ServerEvent = Exiled.Events.Handlers.Server;

namespace RespawnTimer
{
    public class RespawnTimer : Plugin<Config>
    {
        private static readonly Lazy<RespawnTimer> LazyInstance = new Lazy<RespawnTimer>(() => new RespawnTimer());
        public static RespawnTimer Instance => LazyInstance.Value;

        public override PluginPriority Priority => PluginPriority.Medium;

        public override string Author => "Michal78900";
        public override Version Version => new Version(1, 0, 0);

        private RespawnTimer() { }

        private Handler handler;

        public override void OnEnabled()
        {
            base.OnEnabled();

            handler = new Handler();

            ServerEvent.RoundStarted += handler.OnRoundStart;
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            ServerEvent.RoundStarted -= handler.OnRoundStart;

            handler = null;
        }
    }
}
