namespace RespawnTimer_NorthwoodAPI.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using RespawnTimer_Base;
    using RespawnTimer_Base.API.Interfaces;

    public sealed class Config : IBaseConfig
    {
        [Description("Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether debug messages shoul be shown in a server console.")]
        public bool Debug { get; set; } = false;
        
        public List<string> Timers { get; set; } = new()
        {
            "ExampleTimer"
        };
        
        public bool ReloadTimerEachRound { get; set; } = true;
        
        public bool HideTimerForOverwatch { get; set; } = true;
    }
}