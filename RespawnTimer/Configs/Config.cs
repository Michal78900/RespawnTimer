namespace RespawnTimer.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Interfaces;

    public sealed class Config : IConfig
    {
        [Description("Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether debug messages shoul be shown in a server console.")]
        public bool Debug { get; set; } = false;

        [Description("List of timer names that will be used:")]
        public List<string> Timers { get; private set; } = new()
        {
            "ExampleTimer"
        };

        [Description("Whether the timer should be reloaded each round. Useful if you have many different timers designed.")]
        public bool ReloadTimerEachRound { get; private set; } = true;

        [Description("Whether the timer should be hidden for players in overwatch.")]
        public bool HideTimerForOverwatch { get; private set; } = true;
    }
}