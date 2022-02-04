namespace RespawnTimer
{
    using System;
    using System.Reflection;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Exiled.Loader;

    using ServerEvent = Exiled.Events.Handlers.Server;

    public class RespawnTimer : Plugin<Config, Translation>
    {
        public static RespawnTimer Singleton;

        public static Assembly SerpentsHandAssembly;
        public static Assembly UIURescueSquadAssembly;

        public override void OnEnabled()
        {
            Singleton = this;

            ServerEvent.RoundStarted += EventHandler.OnRoundStart;

            foreach (IPlugin<IConfig> plugin in Loader.Plugins)
            {
                if (plugin.Name == "SerpentsHand" && plugin.Config.IsEnabled)
                {
                    SerpentsHandAssembly = plugin.Assembly;
                    Log.Debug("SerpentsHand plugin detected!", Config.ShowDebugMessages);
                }

                if (plugin.Name == "UIURescueSquad" && plugin.Config.IsEnabled)
                {
                    UIURescueSquadAssembly = plugin.Assembly;
                    Log.Debug("UIURescueSquad plugin detected!", Config.ShowDebugMessages);
                }
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            ServerEvent.RoundStarted -= EventHandler.OnRoundStart;
            Singleton = null;

            base.OnDisabled();
        }

        public override string Name => "RespawnTimer";
        public override string Author => "Michal78900";
        public override Version Version => new Version(3, 1, 0);
        public override Version RequiredExiledVersion => new Version(3, 0, 5);
    }
}
