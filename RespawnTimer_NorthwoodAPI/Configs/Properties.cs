namespace RespawnTimer_NorthwoodAPI.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public sealed class Properties
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
        
        /*
        public string Sh { get; private set; } = "<color=red>Serpent's Hand</color>";
        
        public string Uiu { get; private set; } = "<color=yellow>Unusual Incidents Unit</color>";
        */
        
        public Dictionary<string, string> WarheadStatus { get; private set; } = new()
        {
            { "NotArmed", "<color=green>Unarmed</color>" },
            { "Armed", "<color=orange>Armed</color>" },
            { "InProgress", "<color=red>In Progress - </color> {detonation_time} s" },
            { "Detonated", "<color=#640000>Detonated</color>" },
        };
    }
}
