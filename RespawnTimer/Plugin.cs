namespace RespawnTimer
{
    using System;
    using System.IO;
    using System.Reflection;
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using Exiled.Loader;
    
    using Random = UnityEngine.Random;
    using ServerEvent = Exiled.Events.Handlers.Server;

    public class RespawnTimer : Plugin<Config>
    {
        public static RespawnTimer Singleton;

        public static Assembly SerpentsHandAssembly;
        public static Assembly UIURescueSquadAssembly;

        public static string RespawnTimerDirectoryPath { get; } = Path.Combine(Paths.Configs, "RespawnTimer");

        public override void OnEnabled()
        {
            Singleton = this;

            if (!Directory.Exists(RespawnTimerDirectoryPath))
            {
                Log.Warn("RespawnTimer directory does not exist. Creating...");
                Directory.CreateDirectory(RespawnTimerDirectoryPath);

                string template = Path.Combine(RespawnTimerDirectoryPath, "Template");
                Directory.CreateDirectory(template);

                File.Create(Path.Combine(template, "TimerBeforeSpawn.txt"));
                File.Create(Path.Combine(template, "TimerDuringSpawn.txt"));
                File.WriteAllText(Path.Combine(template, "Properties.yml"), Loader.Serializer.Serialize(new Properties()));
            }

            ServerEvent.RoundStarted += EventHandler.OnRoundStart;
            ServerEvent.ReloadedConfigs += OnReloaded;

            foreach (IPlugin<IConfig> plugin in Loader.Plugins)
            {
                switch (plugin.Name)
                {
                    case "SerpentsHand" when plugin.Config.IsEnabled:
                        SerpentsHandAssembly = plugin.Assembly;
                        Log.Debug("SerpentsHand plugin detected!", Config.Debug);
                        break;

                    case "UIURescueSquad" when plugin.Config.IsEnabled:
                        UIURescueSquadAssembly = plugin.Assembly;
                        Log.Debug("UIURescueSquad plugin detected!", Config.Debug);
                        break;
                }
            }

            OnReloaded();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            ServerEvent.RoundStarted -= EventHandler.OnRoundStart;
            ServerEvent.ReloadedConfigs -= OnReloaded;
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

            string chosenTimerName = Config.Timers[Random.Range(0, Config.Timers.Count)];

            string directoryPath = Path.Combine(RespawnTimerDirectoryPath, chosenTimerName);
            if (!Directory.Exists(directoryPath))
            {
                Log.Error($"{Path.GetFileNameWithoutExtension(directoryPath)} directory does not exist!");
                return;
            }

            string timerBeforePath = Path.Combine(directoryPath, "TimerBeforeSpawn.txt");
            if (!File.Exists(timerBeforePath))
            {
                Log.Error($"{Path.GetFileName(timerBeforePath)} file does not exist!");
                return;
            }

            string timerDuringPath = Path.Combine(directoryPath, "TimerBeforeSpawn.txt");
            if (!File.Exists(timerDuringPath))
            {
                Log.Error($"{Path.GetFileName(timerDuringPath)} file does not exist!");
                return;
            }

            string propertiesPath = Path.Combine(directoryPath, "Properties.yml");
            if (!File.Exists(propertiesPath))
            {
                Log.Error($"{Path.GetFileName(propertiesPath)} file does not exist!");
                return;
            }

            API.API.TimerView = new(
                File.ReadAllText(timerBeforePath),
                File.ReadAllText(timerDuringPath),
                Loader.Deserializer.Deserialize<Properties>(File.ReadAllText(propertiesPath)));
        }

        public override string Name => "RespawnTimer";
        public override string Author => "Michal78900";
        public override Version Version => new(4, 0, 0);
        public override Version RequiredExiledVersion => new(5, 3, 0);
    }
}