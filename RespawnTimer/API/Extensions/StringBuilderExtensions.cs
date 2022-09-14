namespace RespawnTimer.API.Extensions
{
    using System;
    using System.Globalization;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using System.Linq;
    using System.Text;
    using Respawning;
    using UnityEngine;
    
    using static API;

    public static class StringBuilderExtensions
    {
        public static StringBuilder SetAllProperties(this StringBuilder builder, int? spectatorCount = null) => builder
            .SetRoundTime()   
            .SetMinutesAndSeconds()
            .SetSpawnableTeam()
            .SetSpectatorCountAndTickets(spectatorCount)
            .SetWarheadStatus()
            .SetGeneratorCount()
            .SetTpsAndTickrate();

        private static StringBuilder SetRoundTime(this StringBuilder builder)
        {
            int minutes = Round.ElapsedTime.Minutes;
            builder.Replace("{round_minutes}", $"{(TimerView.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

            int seconds = Round.ElapsedTime.Seconds;
            builder.Replace("{round_seconds}", $"{(TimerView.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
            
            return builder;
        }

        private static StringBuilder SetMinutesAndSeconds(this StringBuilder builder)
        {
            TimeSpan time = Respawn.TimeUntilSpawnWave;
            
            if (Respawn.IsSpawning || !TimerView.Properties.TimerOffset)
            {
                int minutes = (int)time.TotalSeconds / 60;
                builder.Replace("{minutes}", $"{(TimerView.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

                int seconds = (int)Math.Round(time.TotalSeconds % 60);
                builder.Replace("{seconds}", $"{(TimerView.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
            }
            else
            {
                int minutes = (int)(time.TotalSeconds + 15) / 60;
                builder.Replace("{minutes}", $"{(TimerView.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

                int seconds = (int)Math.Round((time.TotalSeconds + 15) % 60);
                builder.Replace("{seconds}", $"{(TimerView.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
            }

            return builder;
        }

        private static StringBuilder SetSpawnableTeam(this StringBuilder builder)
        {
            switch (Respawn.NextKnownTeam)
            {
                case SpawnableTeamType.None:
                    return builder;

                case SpawnableTeamType.NineTailedFox:
                    builder.Replace("{team}", !UiuSpawnable ? TimerView.Properties.Ntf : TimerView.Properties.Uiu);
                    break;

                case SpawnableTeamType.ChaosInsurgency:
                    builder.Replace("{team}", !SerpentsHandSpawnable ? TimerView.Properties.Ci : TimerView.Properties.Sh);
                    break;
            }

            return builder;
        }

        private static StringBuilder SetSpectatorCountAndTickets(this StringBuilder builder, int? spectatorCount = null)
        {
            builder.Replace("{spectators_num}", spectatorCount?.ToString() ?? Player.List.Count(x => x.Role.Team == Team.RIP && !x.IsOverwatchEnabled).ToString());
            builder.Replace("{ntf_tickets_num}", Respawn.NtfTickets.ToString());
            builder.Replace("{ci_tickets_num}", Respawn.ChaosTickets.ToString());

            return builder;
        }

        private static StringBuilder SetWarheadStatus(this StringBuilder builder)
        {
            WarheadStatus warheadStatus = Warhead.Status;
            builder.Replace("{warhead_status}", TimerView.Properties.WarheadStatus[warheadStatus]);
            builder.Replace("{detonation_time}", warheadStatus == WarheadStatus.InProgress ? Mathf.Round(Warhead.DetonationTimer).ToString(CultureInfo.InvariantCulture) : string.Empty);

            return builder;
        }

        private static StringBuilder SetGeneratorCount(this StringBuilder builder)
        {
            int generatorEngaged = 0;
            int generatorCount = 0;

            foreach (Generator generator in Generator.List)
            {
                if (generator.State == GeneratorState.Engaged)
                    generatorEngaged++;

                generatorCount++;
            }

            builder.Replace("{generator_engaged}", generatorEngaged.ToString());
            builder.Replace("{generator_count}", generatorCount.ToString());

            return builder;
        }

        private static StringBuilder SetTpsAndTickrate(this StringBuilder builder)
        {
            builder.Replace("{tps}", Server.Tps.ToString(CultureInfo.InvariantCulture));
            builder.Replace("{tickrate}", ServerStatic.ServerTickrate.ToString());

            return builder;
        }
    }
}