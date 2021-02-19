using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RespawnTimer
{
    public class Translations
    {
        [Description("Translations: (do NOT change text in { }, you can for example bold them)")]
        public string YouWillRespawnIn { get; set; } = "<color=orange>You will respawn in: </color>";
        public string YouWillSpawnAs { get; set; } = "You will spawn as: ";
        public string Ntf { get; set; } = "<color=blue>Nine-Tailed Fox</color>";
        public string Ci { get; set; } = "<color=green>Chaos Insurgency</color>";
        public string Sh { get; set; } = "<color=red>Serpent's Hand</color>";
        public string Uiu { get; set; } = "<color=#1078e0>Unusual Incidents Unit</color>";
        public string Spectators { get; set; } = "<color=#B3B6B7>Spectators: </color>";
        public string NtfTickets { get; set; } = "<color=blue>NTF Tickets: </color>";
        public string CiTickets { get; set; } = "<color=green>CI Tickets: </color>";
        public string Seconds { get; set; } = " <b>{seconds} s</b>";
        public string Minutes { get; set; } = "<b>{minutes} min.</b>";
        public string SpectatorsNum { get; set; } = "{spectators_num}";
        public string NtfTicketsNum { get; set; } = "{ntf_tickets_num}";
        public string CiTicketsNum { get; set; } = "{ci_tickets_num}";
    }
}
