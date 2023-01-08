namespace RespawnTimer_NorthwoodAPI
{
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;
    using PluginAPI.Core;
    using RespawnTimer_Base;

    public class RespawnTimer
    {
        // public static RespawnTimer Singleton { get; private set; }

        // public static string RespawnTimerDirectoryPath { get; private set; }

        [PluginConfig]
        public Config Config;

        [PluginPriority(LoadPriority.Medium)]
        [PluginEntryPoint("RespawnTimer", "1.0.0", "RespawnTimer", "Michal78900")]
        void LoadPlugin()
        {
            // Singleton = this;
            EventManager.RegisterEvents<EventHandler>(this);
            API.Init(null, PluginHandler.Get(this).PluginDirectoryPath);
        }
    }
}