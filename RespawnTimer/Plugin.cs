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
        public override Version Version => new Version(2, 4, 0);
        public override Version RequiredExiledVersion => new Version(2, 8, 0);

        private Handler handler;

        public static bool IsSH = false;
        public static bool IsyUIU = false;
        public static bool IsGS = false;

        public override void OnEnabled()
        {
            Singleton = this;

            handler = new Handler();

            ServerEvent.RoundStarted += handler.OnRoundStart;

            foreach (IPlugin<IConfig> plugin in Loader.Plugins)
            {
                if (Name == "SerpentsHand" && Config.IsEnabled)
                {
                    IsSH = true;
                    Log.Debug("SerpentsHand plugin detected!", Config.ShowDebugMessages);
                }

                if (Name == "UIU Rescue Squad" && Config.IsEnabled)
                {
                    IsyUIU = true;
                    Log.Debug("UIU Rescue Sqad plugin detected!", Config.ShowDebugMessages);
                }

                if (Name == "GhostSpectator" && Config.IsEnabled)
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
