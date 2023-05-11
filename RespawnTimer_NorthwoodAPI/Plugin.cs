namespace RespawnTimer_NorthwoodAPI
{
    using System.IO;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;
    using Configs;
    using PluginAPI.Core;
    using Serialization;

    public class RespawnTimer
    {
        public static RespawnTimer Singleton { get; private set; }

        public static string RespawnTimerDirectoryPath { get; private set; }

        [PluginConfig]
        public Config Config;

        [PluginPriority(LoadPriority.Medium)]
        [PluginEntryPoint("RespawnTimer", "4.0.1", "RespawnTimer", "Michal78900")]
        void LoadPlugin()
        {
            if (!Config.IsEnabled)
                return;

            Singleton = this;
            RespawnTimerDirectoryPath = PluginHandler.Get(this).PluginDirectoryPath;
            EventManager.RegisterEvents<EventHandler>(this);

            if (!Directory.Exists(RespawnTimerDirectoryPath))
            {
                Log.Warning("RespawnTimer directory does not exist. Creating...");
                Directory.CreateDirectory(RespawnTimerDirectoryPath);
            }

            string templateDirectory = Path.Combine(RespawnTimerDirectoryPath, "Template");
            if (!Directory.Exists(templateDirectory))
            {
                Directory.CreateDirectory(templateDirectory);

                File.Create(Path.Combine(templateDirectory, "TimerBeforeSpawn.txt"));
                File.Create(Path.Combine(templateDirectory, "TimerDuringSpawn.txt"));
                File.WriteAllText(Path.Combine(templateDirectory, "Properties.yml"), YamlParser.Serializer.Serialize(new Properties()));

                string hintsPath = Path.Combine(templateDirectory, "Hints.txt");
                File.WriteAllText(hintsPath, "This is an example hint. You can add as much as you want.");
            }
        }
    }
}