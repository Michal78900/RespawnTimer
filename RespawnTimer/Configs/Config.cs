namespace RespawnTimer
{
    using System.Collections.Generic;
    using Exiled.API.Interfaces;
    using System.ComponentModel;

    public sealed class Config : IConfig
    {
        [Description("Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether debug messages shoul be shown in a server console.")]
        public bool Debug { get; private set; } = false;

        public List<string> Timers { get; private set; } = new()
        {
            "ExampleTimer"
        };

        public bool ReloadTimerEachRound { get; private set; } = true;
    }
}