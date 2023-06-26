namespace RespawnTimer
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using API.Features;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Exiled.Loader;
    using Config = Configs.Config;
    using MapEvent = Exiled.Events.Handlers.Map;
    using ServerEvent = Exiled.Events.Handlers.Server;
    using PlayerEvent = Exiled.Events.Handlers.Player;

    public class RespawnTimer : Plugin<Config>
    {
        public static RespawnTimer Singleton;
        public static string RespawnTimerDirectoryPath { get; } = Path.Combine(Paths.Configs, "RespawnTimer");

        public override void OnEnabled()
        {
            Singleton = this;

            if (!Directory.Exists(RespawnTimerDirectoryPath))
            {
                Log.Warn("RespawnTimer directory does not exist. Creating...");
                Directory.CreateDirectory(RespawnTimerDirectoryPath);
            }

            string exampleTimerDirectory = Path.Combine(RespawnTimerDirectoryPath, "ExampleTimer");
            if (!Directory.Exists(exampleTimerDirectory))
                DownloadExampleTimer(exampleTimerDirectory);

            /*
            string templateDirectory = Path.Combine(RespawnTimerDirectoryPath, "Template");
            if (!Directory.Exists(templateDirectory))
            {
                Directory.CreateDirectory(templateDirectory);

                File.Create(Path.Combine(templateDirectory, "TimerBeforeSpawn.txt"));
                File.Create(Path.Combine(templateDirectory, "TimerDuringSpawn.txt"));
                File.WriteAllText(Path.Combine(templateDirectory, "Properties.yml"), Loader.Serializer.Serialize(new Properties()));

                string hintsPath = Path.Combine(templateDirectory, "Hints.txt");
                File.WriteAllText(hintsPath, "This is an example hint. You can add as much as you want.");
            }
            */

            MapEvent.Generated += EventHandler.OnGenerated;
            ServerEvent.RoundStarted += EventHandler.OnRoundStart;
            ServerEvent.ReloadedConfigs += OnReloaded;
            PlayerEvent.Dying += EventHandler.OnDying;

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
        }

        public override void OnDisabled()
        {
            MapEvent.Generated -= EventHandler.OnGenerated;
            ServerEvent.RoundStarted -= EventHandler.OnRoundStart;
            ServerEvent.ReloadedConfigs -= OnReloaded;
            PlayerEvent.Dying -= EventHandler.OnDying;

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

        private void DownloadExampleTimer(string exampleTimerDirectory)
        {
            string exampleTimerZip = exampleTimerDirectory + ".zip";
            string exampleTimerTemp = exampleTimerDirectory + "_Temp";

            using WebClient client = new();

            Log.Warn("Downloading ExampleTimer.zip...");
            client.DownloadFile(
                $"https://github.com/Michal78900/RespawnTimer/releases/download/v{Version}/ExampleTimer.zip", exampleTimerZip);

            Log.Info("ExampleTimer.zip has been downloaded!");

            Log.Warn("Extracting...");
            ZipFile.ExtractToDirectory(exampleTimerZip, exampleTimerTemp);
            Directory.Move(Path.Combine(exampleTimerTemp, "ExampleTimer"), exampleTimerDirectory);

            Directory.Delete(exampleTimerTemp);
            File.Delete(exampleTimerZip);

            Log.Info("Done!");
        }

        public override string Name => "RespawnTimer";
        public override string Author => "Michal78900";
        public override Version Version => new(4, 0, 1);
        public override Version RequiredExiledVersion => new(7, 0, 0);
        public override PluginPriority Priority => PluginPriority.Last;
    }
}