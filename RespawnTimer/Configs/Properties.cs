namespace RespawnTimer.Configs
{
    using System.Collections.Generic;
    using Exiled.API.Enums;

    public sealed class Properties
    {
        public bool LeadingZeros { get; private set; } = true;
        
        public bool TimerOffset { get; private set; } = true;

        public int HintInterval { get; private set; } = 10;
        
        public string Ntf { get; private set; } = "<color=blue>Nine-Tailed Fox</color>";
        
        public string Ci { get; private set; } = "<color=green>Chaos Insurgency</color>";
        
        public string Sh { get; private set; } = "<color=red>Serpent's Hand</color>";
        
        public string Uiu { get; private set; } = "<color=yellow>Unusual Incidents Unit</color>";

        public Dictionary<WarheadStatus, string> WarheadStatus { get; private set; } = new()
        {
            { Exiled.API.Enums.WarheadStatus.NotArmed, "<color=green>Unarmed</color>" },
            { Exiled.API.Enums.WarheadStatus.Armed, "<color=orange>Armed</color>" },
            { Exiled.API.Enums.WarheadStatus.InProgress, "<color=red>In Progress - </color> {detonation_time} s" },
            { Exiled.API.Enums.WarheadStatus.Detonated, "<color=#640000>Detonated</color>" },
        };
    }
}
