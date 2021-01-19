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
        public override Version Version => new Version(1, 3, 0);

        private RespawnTimer() { }

        private Handler handler;

        public static bool ThereIsSH;
        public static bool ThereIsUIU;

        public override void OnEnabled()
        {
            base.OnEnabled();

            handler = new Handler();

            ServerEvent.RoundStarted += handler.OnRoundStart;

            //SH Support
            if (IsSH())
            {
                ThereIsSH = true;
                Log.Debug("SerpentsHand plugin detected!");
            }
            else ThereIsSH = false;
            //

            //UIU Support
            if ((IsUIU()))
            {
                ThereIsUIU = true;
                Log.Debug("UIU Rescue Squad plugin detected!");
            }
            else ThereIsUIU = false;
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

        //UIU Support
        private static bool IsUIU()
        {
            Assembly assembly = Loader.Plugins.FirstOrDefault(pl => pl.Name == "UIU Rescue Squad")?.Assembly;
            if (assembly == null) return false;
            else return true;
        }
        //
    }
}
