namespace RespawnTimer_Base
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public abstract class BaseConfig
    {
        [Description("List of timer names that will be used:")]
        public List<string> Timers { get; set; } = new()
        {
            "ExampleTimer"
        };

        [Description("Whether the timer should be reloaded each round. Useful if you have many different timers designed.")]
        public bool ReloadTimerEachRound { get; set; } = true;

        [Description("Whether the timer should be hidden for players in overwatch.")]
        public bool HideTimerForOverwatch { get; set; } = true;
    }
}