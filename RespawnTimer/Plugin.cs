namespace RespawnTimer
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using API.Features;
#if EXILED
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Exiled.Loader;
#else
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;
#endif

#if EXILED
    public class RespawnTimer : Plugin<Configs.Config>
#else
    public class RespawnTimer
#endif
    {
        public static RespawnTimer Singleton;
        public static string RespawnTimerDirectoryPath { get; private set; }

#if !EXILED
        [PluginConfig]
        public Configs.Config Config;
#else
        public EventHandler EventHandler;
#endif


#if EXILED
        public override void OnEnabled()
#else
        [PluginAPI.Core.Attributes.PluginPriority(LoadPriority.Medium)]
        [PluginEntryPoint("RespawnTimer", "4.0.3", "RespawnTimer", "Michal78900")]
        private void LoadPlugin()
#endif
        {
#if !EXILED
            if (!Config.IsEnabled)
                return;
#endif

            Singleton = this;
#if EXILED
            RespawnTimerDirectoryPath = Path.Combine(Paths.Configs, "RespawnTimer");
            EventHandler = new EventHandler();
#else
            RespawnTimerDirectoryPath = PluginHandler.Get(this).PluginDirectoryPath;
            EventManager.RegisterEvents<EventHandler>(this);
#endif

            if (!Directory.Exists(RespawnTimerDirectoryPath))
            {
                // Log.Warn("RespawnTimer directory does not exist. Creating...");
                Log.Info("RespawnTimer directory does not exist. Creating...");
                Directory.CreateDirectory(RespawnTimerDirectoryPath);
            }

            string exampleTimerDirectory = Path.Combine(RespawnTimerDirectoryPath, "ExampleTimer");
            if (!Directory.Exists(exampleTimerDirectory))
                DownloadExampleTimer(exampleTimerDirectory);

#if EXILED
            Exiled.Events.Handlers.Map.Generated += EventHandler.OnGenerated;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandler.OnRoundStart;
            Exiled.Events.Handlers.Player.Dying += EventHandler.OnDying;
            Exiled.Events.Handlers.Server.ReloadedConfigs += OnReloaded;

            foreach (IPlugin<IConfig> plugin in Loader.Plugins)
            {
                switch (plugin.Name)
                {
                    case "Serpents Hand" when plugin.Config.IsEnabled:
                        API.API.SerpentsHandTeam.Init(plugin.Assembly);
                        Log.Debug("Serpents Hand plugin detected!");
                        break;

                    case "UIURescueSquad" when plugin.Config.IsEnabled:
                        API.API.UiuTeam.Init(plugin.Assembly);
                        Log.Debug("UIURescueSquad plugin detected!");
                        break;
                }
            }

            if (!Config.ReloadTimerEachRound)
                OnReloaded();

            base.OnEnabled();
#endif
        }

        private void DownloadExampleTimer(string exampleTimerDirectory)
        {
            string exampleTimerZip = exampleTimerDirectory + ".zip";
            string exampleTimerTemp = exampleTimerDirectory + "_Temp";

            using WebClient client = new();

            // Log.Warn("Downloading ExampleTimer.zip...");
            Log.Info("Downloading ExampleTimer.zip...");
#if EXILED
            string url = $"https://github.com/Michal78900/RespawnTimer/releases/download/v{Version}/ExampleTimer.zip";
#else
            string url = $"https://github.com/Michal78900/RespawnTimer/releases/download/v{PluginHandler.Get(this).PluginVersion}/ExampleTimer.zip";
#endif
            try
            {
                client.DownloadFile(url, exampleTimerZip);
            }
            catch (WebException e)
            {
                if (e.Response is HttpWebResponse response)
                    Log.Error($"Error while downloading ExampleTimer.zip: {(int)response.StatusCode} {response.StatusCode}");
                
                return;
            }

            Log.Info("ExampleTimer.zip has been downloaded!");

            // Log.Warn("Extracting...");
            Log.Info("Extracting...");
            ZipFile.ExtractToDirectory(exampleTimerZip, exampleTimerTemp);
            Directory.Move(Path.Combine(exampleTimerTemp, "ExampleTimer"), exampleTimerDirectory);

            Directory.Delete(exampleTimerTemp);
            File.Delete(exampleTimerZip);

            Log.Info("Done!");
        }

#if EXILED
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Map.Generated -= EventHandler.OnGenerated;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandler.OnRoundStart;
            Exiled.Events.Handlers.Player.Dying -= EventHandler.OnDying;
            Exiled.Events.Handlers.Server.ReloadedConfigs -= OnReloaded;

            EventHandler = null;
            Singleton = null;

            base.OnDisabled();
        }

        public override void OnReloaded()
        {
            if (Config.Timers.IsEmpty())
            {
                Log.Error("Timer list is empty!");
                return;
            }

            TimerView.CachedTimers.Clear();

            foreach (string name in Config.Timers.Values)
                TimerView.AddTimer(name);
        }

        public override string Name => "RespawnTimer";
        public override string Author => "Michal78900";
        public override Version Version => new(4, 0, 3);
        public override Version RequiredExiledVersion => new(8, 6, 0);
        public override PluginPriority Priority => PluginPriority.Last;
#endif
    }
}