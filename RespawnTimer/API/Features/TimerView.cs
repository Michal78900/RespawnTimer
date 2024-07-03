﻿namespace RespawnTimer.API.Features
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Configs;
    using Respawning;
    using Serialization;
    using UnityEngine;
#if EXILED
    using Exiled.API.Features;
    using Exiled.Loader;
    using System;
#else
    using PluginAPI.Core;
#endif

    public partial class TimerView
    {
        public static readonly Dictionary<string, TimerView> CachedTimers = new();

        public int HintIndex { get; private set; }

        private int HintInterval { get; set; }

        public static void AddTimer(string name)
        {
            if (CachedTimers.ContainsKey(name))
                return;

            string directoryPath = Path.Combine(RespawnTimer.RespawnTimerDirectoryPath, name);
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
                Log.Error($"{Path.GetFileName(propertiesPath)} file does not exist! Creating...");
                File.WriteAllText(propertiesPath, YamlParser.Serializer.Serialize(new Properties()));
            }

            string hintsPath = Path.Combine(directoryPath, "Hints.txt");
            List<string> hints = new();
            if (File.Exists(hintsPath))
                hints.AddRange(File.ReadAllLines(hintsPath));

            TimerView timerView = new(
                File.ReadAllText(timerBeforePath),
                File.ReadAllText(timerDuringPath),
                YamlParser.Deserializer.Deserialize<Properties>(File.ReadAllText(propertiesPath)),
                hints);

            CachedTimers.Add(name, timerView);
        }

        public static void AddReplaceHelper(string name, Func<Player, string> action)
        {
            ReplaceHelper.Add(name, action);
        }

        public static void RemoveReplaceHelper(string name)
        {
            if (!ReplaceHelper.ContainsKey(name))
                return;

            ReplaceHelper.Remove(name);
        }

        public static bool TryGetTimerForPlayer(Player player, out TimerView timerView)
        {
            string groupName = !ServerStatic.PermissionsHandler._members.TryGetValue(player.UserId, out string str) ? null : str;

            // Check by group name
            if (groupName is not null && RespawnTimer.Singleton.Config.Timers.TryGetValue(groupName, out string timerName))
            {
                timerView = CachedTimers[timerName];
                return true;
            }

            // Check by user id
            if (RespawnTimer.Singleton.Config.Timers.TryGetValue(player.UserId, out timerName))
            {
                timerView = CachedTimers[timerName];
                return true;
            }

            // Use fallback default timer
            if (RespawnTimer.Singleton.Config.Timers.TryGetValue("default", out timerName))
            {
                timerView = CachedTimers[timerName];
                return true;
            }

            // Default fallback does not exist
            timerView = null!;
            return false;
        }

        public string GetText(Player player, int? spectatorCount = null)
        {
            StringBuilder.Clear();
            StringBuilder.Append(
                RespawnManager.Singleton._curSequence is not RespawnManager.RespawnSequencePhase.PlayingEntryAnimations or RespawnManager.RespawnSequencePhase.SpawningSelectedTeam
                    ? BeforeRespawnString
                    : DuringRespawnString);

            foreach (KeyValuePair<string, Func<Player, string>> data in ReplaceHelper)
            {
                StringBuilder.Replace("{" + data.Key + "}", data.Value(player));
            }

            SetAllProperties(spectatorCount);

            StringBuilder.Replace("{RANDOM_COLOR}", $"#{UnityEngine.Random.Range(0x0, 0xFFFFFF):X6}");
            StringBuilder.Replace('{', '[').Replace('}', ']');

            return StringBuilder.ToString();
        }

        internal void IncrementHintInterval()
        {
            HintInterval++;
            if (HintInterval != Properties.HintInterval)
                return;

            HintInterval = 0;
            IncrementHintIndex();
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

        private readonly StringBuilder StringBuilder = new(1024);

        public static Dictionary<string, Func<Player, string>> ReplaceHelper { get; } = new();
    }
}