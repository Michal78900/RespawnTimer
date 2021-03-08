using System;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Loader;
using System.Reflection;

using ServerEvent = Exiled.Events.Handlers.Server;
using Exiled.API.Interfaces;

namespace RespawnTimer
{
    public class RespawnTimer : Plugin<Config>
    {
        public static RespawnTimer Singleton;

        public override string Author => "Michal78900";
        public override string Name => "RespawnTimer";
        public override Version Version => new Version(2, 2, 0);
        public override Version RequiredExiledVersion => new Version(2, 1, 30);

        private Handler handler;

        public static bool assemblySH = false;
        public static bool assemblyUIU = false;
        public static bool assemblyGS = false;

        public override void OnEnabled()
        {
            Singleton = this;

            handler = new Handler(this);

            ServerEvent.RoundStarted += handler.OnRoundStart;


            foreach (IPlugin<IConfig> plugin in Loader.Plugins)
            {
                if (plugin.Name == "SerpentsHand" && plugin.Config.IsEnabled)
                {
                    assemblySH = true;
                    Log.Debug("SerpentsHand plugin detected!", Config.ShowDebugMessages);
                }

                if (plugin.Name == "UIU Rescue Squad" && plugin.Config.IsEnabled)
                {
                    assemblyUIU = true;
                    Log.Debug("UIU Rescue Sqad plugin detected!", Config.ShowDebugMessages);
                }

                if (plugin.Name == "GhostSpectator" && plugin.Config.IsEnabled)
                {
                    assemblyGS = true;
                    Log.Debug("GhostSpectator plugin detected!", Config.ShowDebugMessages);
                }
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            ServerEvent.RoundStarted -= handler.OnRoundStart;

            handler = null;
            Singleton = null;

            base.OnDisabled();
        }
    }
}
