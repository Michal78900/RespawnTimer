namespace RespawnTimer.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;

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
        
        [Description("The Serpent's Hand display name.")]
        public string Sh { get; private set; } = "<color=red>Serpent's Hand</color>";
        
        [Description("The Unusual Incidents Unit  display name.")]
        public string Uiu { get; private set; } = "<color=yellow>Unusual Incidents Unit</color>";

        [Description("The display names for warhead statuses:")]
        public Dictionary<WarheadStatus, string> WarheadStatus { get; private set; } = new()
        {
            { Exiled.API.Enums.WarheadStatus.NotArmed, "<color=green>Unarmed</color>" },
            { Exiled.API.Enums.WarheadStatus.Armed, "<color=orange>Armed</color>" },
            { Exiled.API.Enums.WarheadStatus.InProgress, "<color=red>In Progress - </color> {detonation_time} s" },
            { Exiled.API.Enums.WarheadStatus.Detonated, "<color=#640000>Detonated</color>" },
        };
    }
}
