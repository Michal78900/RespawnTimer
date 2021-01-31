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
        public static RespawnTimer Singleton;

        public override PluginPriority Priority => PluginPriority.Low;

        public override string Author => "Michal78900";
        public override string Name => "RespawnTimer";
        public override Version Version => new Version(2, 0, 0);
        public override Version RequiredExiledVersion => new Version(2, 1, 29);

        private Handler handler;

        public static Assembly assemblySH;
        public static Assembly assemblyUIU;

        public override void OnEnabled()
        {
            base.OnEnabled();

            Singleton = this;

            handler = new Handler(this);

            ServerEvent.RoundStarted += handler.OnRoundStart;


            Log.Debug("Checking for SerpentsHand...", Config.ShowDebugMessages);
            try
            {
                assemblySH = Loader.Plugins.FirstOrDefault(pl => pl.Name == "SerpentsHand")?.Assembly;
                Log.Debug("SerpentsHand plugin detected!", Config.ShowDebugMessages);

            }
            catch(Exception)
            {
                Log.Debug("SerpentsHand plugin is not installed", Config.ShowDebugMessages);
            }


            Log.Debug("Checking for UIURescueSquad...", Config.ShowDebugMessages);
            try
            {
                assemblyUIU = Loader.Plugins.FirstOrDefault(pl => pl.Name == "UIU Rescue Squad")?.Assembly;
                Log.Debug("UIURescueSquad plugin detected!", Config.ShowDebugMessages);

            }
            catch (Exception)
            {
                Log.Debug("UIURescueSquad plugin is not installed", Config.ShowDebugMessages);
            }


        }

        public override void OnDisabled()
        {
            base.OnDisabled();

            ServerEvent.RoundStarted -= handler.OnRoundStart;

            handler = null;
        }
    }
}
