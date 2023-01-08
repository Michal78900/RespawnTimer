namespace RespawnTimer_Base
{
    using System.Collections.Generic;
    using System.IO;
    using Serialization;

    public static class API
    {
        public static void Init(BaseConfig config, string directoryPath)
        {
            Config = config;
            DirectoryPath = directoryPath;
            
            if (!Directory.Exists(DirectoryPath))
            {
                // Log.Warning("RespawnTimer directory does not exist. Creating...");
                Directory.CreateDirectory(DirectoryPath);
            }

            string templateDirectory = Path.Combine(DirectoryPath, "Template");
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
        
        public static BaseConfig Config { get; private set; }
        
        public static string DirectoryPath { get; private set; }
        
        public static List<string> TimerHidden { get; } = new();
    }
}
