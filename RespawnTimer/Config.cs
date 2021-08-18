namespace RespawnTimer
{
    using Exiled.API.Interfaces;
    using System.ComponentModel;

    public class Config : IConfig
    {
        [Description("Is the plugin enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Should debug messages be shown in a server console.")]
        public bool ShowDebugMessages { get; private set; } = false;

        [Description("How long the hint should be presented to the player. The hint will be refreshed every seconds anyways, but make this value higher if the hint \"blinks\"")]
        public float HintDuration { get; private set; } = 1.1f;

        [Description("Should a timer be lower or higher on the screen. (values from 0 to 14, 0 - very high, 14 - very low)")]
        public byte TextLowering { get; private set; } = 8;

        [Description("Should a timer show an exact number of minutes?")]
        public bool ShowMinutes { get; private set; } = true;

        [Description("Should a timer show an exact number of seconds?")]
        public bool ShowSeconds { get; private set; } = true;

        [Description("Should a timer be only shown, when a spawnning sequence has begun? (NTF Helicopter / Chaos Car arrives)")]
        public bool ShowTimerOnlyOnSpawn { get; private set; } = false;

        [Description("Should number of spectators be shown?")]
        public bool ShowNumberOfSpectators { get; private set; } = true;

        [Description("Should the NTF and CI respawn tickets be shown?")]
        public bool ShowTickets { get; private set; } = true;
    }
}