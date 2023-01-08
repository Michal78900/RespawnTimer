namespace RespawnTimer_Base.API.Features
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Configs;
    using Extensions;
    using PluginAPI.Core;
    using Respawning;
    using Serialization;
    using static API;

    public class TimerView
    {
        public static TimerView Current { get; private set; }

        public int HintIndex { get; private set; }

        private int HintInterval { get; set; }

        public static void GetNew(string name)
        {
            string directoryPath = Path.Combine(DirectoryPath, name);
            if (!Directory.Exists(directoryPath))
            {
                Log.Error($"{name} directory does not exist!");
                return;
            }

            string timerBeforePath = Path.Combine(directoryPath, "TimerBeforeSpawn.txt");
            if (!File.Exists(timerBeforePath))
            {
                Log.Error($"{Path.GetFileName(timerBeforePath)} file does not exist!");
                return;
            }

            string timerDuringPath = Path.Combine(directoryPath, "TimerDuringSpawn.txt");
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

            string hintsPath = Path.Combine(directoryPath, "Hints.txt");
            List<string> hints = new();
            if (File.Exists(hintsPath))
                hints.AddRange(File.ReadAllLines(hintsPath));

            Current = new(
                File.ReadAllText(timerBeforePath),
                File.ReadAllText(timerDuringPath),
                YamlParser.Deserializer.Deserialize<Properties>(File.ReadAllText(propertiesPath)),
                hints);

            // Log.Debug($"{name} has been successfully loaded!", RespawnTimer.Singleton.Config.Debug);
        }

        public string GetText(int? spectatorCount = null)
        {
            StringBuilder.Clear();
            StringBuilder.Append(
                RespawnManager.Singleton._curSequence is not RespawnManager.RespawnSequencePhase.PlayingEntryAnimations or RespawnManager.RespawnSequencePhase.SpawningSelectedTeam
                    ? BeforeRespawnString
                    : DuringRespawnString);

            // StringBuilder.Append(BeforeRespawnString.Replace('{', '[').Replace('}', ']'));
            StringBuilder.SetAllProperties(spectatorCount);
            // StringBuilder.Replace('{', '[').Replace('}', ']');

            HintInterval++;
            if (HintInterval == Properties.HintInterval)
            {
                HintInterval = 0;
                IncrementHintIndex();
            }

            return StringBuilder.ToString();
        }

        private void IncrementHintIndex()
        {
            HintIndex++;
            if (Hints.Count == HintIndex)
                HintIndex = 0;
        }

        private TimerView(string beforeRespawnString, string duringRespawnString, Properties properties, List<string> hints)
        {
            BeforeRespawnString = beforeRespawnString;
            DuringRespawnString = duringRespawnString;
            Properties = properties;
            Hints = hints;
        }

        public string BeforeRespawnString { get; }

        public string DuringRespawnString { get; }

        public Properties Properties { get; }

        public List<string> Hints { get; }

        private static readonly StringBuilder StringBuilder = new(1024);
    }
}