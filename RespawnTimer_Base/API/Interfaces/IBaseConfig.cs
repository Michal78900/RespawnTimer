namespace RespawnTimer_Base.API.Interfaces
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IBaseConfig
    {
        [Description("List of timer names that will be used:")]
        public List<string> Timers { get; set; }

        [Description("Whether the timer should be reloaded each round. Useful if you have many different timers designed.")]
        public bool ReloadTimerEachRound { get; set; }

        [Description("Whether the timer should be hidden for players in overwatch.")]
        public bool HideTimerForOverwatch { get; set; }
    }
}