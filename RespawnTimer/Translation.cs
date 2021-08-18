namespace RespawnTimer
{
    using Exiled.API.Interfaces;

    public class Translation : ITranslation
    {
        public string YouWillRespawnIn { get; private set; } = "<color=orange>You will respawn in: </color>";
        public string YouWillSpawnAs { get; private set; } = "You will spawn as: ";
        public string Ntf { get; private set; } = "<color=blue>Nine-Tailed Fox</color>";
        public string Ci { get; private set; } = "<color=green>Chaos Insurgency</color>";
        public string Sh { get; private set; } = "<color=red>Serpent's Hand</color>";
        public string Uiu { get; private set; } = "<color=yellow>Unusual Incidents Unit</color>";
        public string Spectators { get; private set; } = "<color=#B3B6B7>Spectators: </color>";
        public string NtfTickets { get; private set; } = "<color=blue>NTF Tickets: </color>";
        public string CiTickets { get; private set; } = "<color=green>CI Tickets: </color>";
        public string Seconds { get; private set; } = " <b>{seconds} s</b>";
        public string Minutes { get; private set; } = "<b>{minutes} min.</b>";
        public string SpectatorsNum { get; private set; } = "{spectators_num}";
        public string NtfTicketsNum { get; private set; } = "{ntf_tickets_num}";
        public string CiTicketsNum { get; private set; } = "{ci_tickets_num}";
    }
}
