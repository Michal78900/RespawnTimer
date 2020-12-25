using System;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Loader;
using System.Reflection;

using ServerEvent = Exiled.Events.Handlers.Server;

namespace RespawnTimer
{
    public class RespawnTimer : Plugin<Config>
    {
        private static readonly Lazy<RespawnTimer> LazyInstance = new Lazy<RespawnTimer>(() => new RespawnTimer());
        public static RespawnTimer Instance => LazyInstance.Value;

        public override PluginPriority Priority => PluginPriority.Low;

        public override string Author => "Michal78900";
        public override string Name => "RespawnTimer";
        public override Version Version => new Version(1, 1, 1);

        private RespawnTimer() { }

        private Handler handler;

        public static bool ThereIsSH;

        public override void OnEnabled()
        {
            base.OnEnabled();

            handler = new Handler();

            ServerEvent.RoundStarted += handler.OnRoundStart;

            //SH Support
            if (IsSH())
            {
                ThereIsSH = true;
                Log.Info("SerpentsHand plugin detected!");
            }
            else ThereIsSH = false;
            //
        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            ServerEvent.RoundStarted -= handler.OnRoundStart;

            handler = null;
        }

        //SH Support
        private static bool IsSH()
        {
            Assembly assembly = Loader.Plugins.FirstOrDefault(pl => pl.Name == "SerpentsHand")?.Assembly;
            if (assembly == null) return false;
            else return true;
        }
        //
    }
}
