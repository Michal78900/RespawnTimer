namespace RespawnTimer
{
    using Exiled.API.Interfaces;
    using System.ComponentModel;

    public class Config : IConfig
    {
        [Description("Is the plugin enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Should debug messages be shown in a server console?")]
        public bool ShowDebugMessages { get; set; } = false;

        [Description("How often (in seconds) should timer be refreshed?")]
        public float Interval { get; set; } = 1f;

        [Description("Should a timer be lower or higher on the screen? (values from 0 to 14, 0 - very high, 14 - very low)")]
        public byte TextLowering { get; set; } = 8;

        [Description("Should a timer show an exact number of minutes?")]
        public bool ShowMinutes { get; set; } = true;

        [Description("Should a timer show an exact number of seconds?")]
        public bool ShowSeconds { get; set; } = true;

        [Description("Should a timer be only shown, when a spawnning sequence has begun? (NTF Helicopter / Chaos Car arrives)")]
        public bool ShowTimerOnlyOnSpawn { get; set; } = false;

        [Description("Should number of spectators be shown?")]
        public bool ShowNumberOfSpectators { get; set; } = true;

        [Description("Should the NTF and CI respawn tickets be shown?")]
        public bool ShowTickets { get; set; } = true;

        [Description("Translations: (do NOT change text in { }, you can for example bold them)")]
        public Translations Translations { get; set; } = new Translations();
    }
}