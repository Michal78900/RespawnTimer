using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace RespawnTimer
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Should a timer be only shown, when a spawnning sequence has begun? (NTF Helicopter / Chaos Car arrives)")]
        public bool ShowTimerOnlyOnSpawn { get; set; } = false;

        [Description("Should the NTF and CI respawn tickets be shown?")]
        public bool ShowTickets { get; set; } = true;

        [Description("All of the texts in this plugin. If you need to translate them, you can easily do this: (The dashed line is just a interspace. Things in { } are numeric variables, you can for example bold them. DO NOT CHANGE NAMES IN { }!")]
        public List<string> translations { get; set; } = new List<string>
        {
            "<color=orange>You will respawn in: </color>",
            "You will spawn as: ",
            "<color=blue>Nine-Tailed Fox</color>",
            "<color=green>Chaos Insurgency</color>",
            "<color=blue>NTF Tickets: </color>",
            "<color=green>CI Tickets: </color>",
            "-------------------------------------",
            "<b>{seconds} s</b>",
            "{ntf_tickets_num}",
            "{ci_tickets_num}"
        };
    }
}
