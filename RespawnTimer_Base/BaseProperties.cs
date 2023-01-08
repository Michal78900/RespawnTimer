namespace RespawnTimer_Base
{
    using System.ComponentModel;

    public abstract class BaseProperties
    {
        [Description("Whether the leading zeros should be added in minutes and seconds if number is less than 10.")]
        public bool LeadingZeros { get; private set; } = true;
        
        [Description("Whether the timer should add time offset depending on MTF/CI spawn.")]
        public bool TimerOffset { get; private set; } = true;

        [Description("How often custom hints should be changed (in seconds).")]
        public int HintInterval { get; private set; } = 10;
        
        [Description("The Nine-Tailed Fox display name.")]
        public string Ntf { get; private set; } = "<color=blue>Nine-Tailed Fox</color>";
        
        [Description("The Chaos Insurgency display name.")]
        public string Ci { get; private set; } = "<color=green>Chaos Insurgency</color>";
    }
}