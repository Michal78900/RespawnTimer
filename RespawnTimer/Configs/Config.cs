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

        /*
        // [Description("Whether a timer be lower or higher on the screen. (values from 0 to 14, 0 - very high, 14 - very low)")]
        // public byte TextLowering { get; private set; } = 8;
        [Description("Whether a timer show an exact number of minutes")]

        public bool ShowMinutes { get; private set; } = true;

        [Description("Whether a timer show an exact number of seconds")]
        public bool ShowSeconds { get; private set; } = true;

        [Description("Whether a timer be only shown, when a spawnning sequence has begun (NTF Helicopter / Chaos Car arrives)")]
        public bool ShowTimerOnlyOnSpawn { get; private set; } = false;

        [Description("Whether number of spectators be shown")]
        public bool ShowNumberOfSpectators { get; private set; } = true;

        [Description("Whether the NTF and CI respawn tickets be shown")]
        public bool ShowTickets { get; private set; } = true;

        [Description("Whether the Warhead status should be shown")]
        public bool ShowWarheadStatus { get; private set; } = true;
        */
    }
}