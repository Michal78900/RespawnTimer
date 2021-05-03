namespace RespawnTimer
{
    using System;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Exiled.Loader;

    using ServerEvent = Exiled.Events.Handlers.Server;

    public class RespawnTimer : Plugin<Config>
    {
        public static RespawnTimer Singleton;

        public override string Author => "Michal78900";
        public override string Name => "RespawnTimer";
        public override Version Version => new Version(2, 4, 1);
        public override Version RequiredExiledVersion => new Version(2, 10, 0);

        private Handler handler;

        public static bool IsSH = false;
        public static bool IsUIU = false;
        public static bool IsGS = false;

        public override void OnEnabled()
        {
            Singleton = this;

            handler = new Handler();

            ServerEvent.RoundStarted += handler.OnRoundStart;

            foreach (IPlugin<IConfig> plugin in Loader.Plugins)
            {
                if (plugin.Name == "SerpentsHand" && plugin.Config.IsEnabled)
                {
                    IsSH = true;
                    Log.Debug("SerpentsHand plugin detected!", Config.ShowDebugMessages);
                }

                if (plugin.Name == "UIURescueSquad" && plugin.Config.IsEnabled)
                {
                    IsUIU = true;
                    Log.Debug("UIURescueSquad plugin detected!", Config.ShowDebugMessages);
                }

                if (plugin.Name == "GhostSpectator" && plugin.Config.IsEnabled)
                {
                    IsGS = true;
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
