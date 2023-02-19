/*
namespace RespawnTimer.API.Extensions
{
    using System;
    using System.Globalization;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using System.Linq;
    using System.Text;
    using Features;
    using PlayerRoles;
    using Respawning;
    using UnityEngine;
    
    using static API;
    using static Features.TimerView;

    public static class StringBuilderExtensions
    {
        public static StringBuilder SetAllProperties(this StringBuilder builder, TimerView timerView, int? spectatorCount = null) => builder
            .SetRoundTime(timerView)
            .SetMinutesAndSeconds(timerView)
            .SetSpawnableTeam(timerView)
            .SetSpectatorCountAndTickets(spectatorCount)
            .SetWarheadStatus(timerView)
            .SetGeneratorCount()
            .SetTpsAndTickrate()
            .SetHint(timerView);

        private static StringBuilder SetRoundTime(this StringBuilder builder, TimerView timerView)
        {
            int minutes = Round.ElapsedTime.Minutes;
            builder.Replace("{round_minutes}", $"{(timerView.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

            int seconds = Round.ElapsedTime.Seconds;
            builder.Replace("{round_seconds}", $"{(timerView.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
            
            return builder;
        }

        private static StringBuilder SetMinutesAndSeconds(this StringBuilder builder, TimerView timerView)
        {
            TimeSpan time = Respawn.TimeUntilSpawnWave;
            
            if (Respawn.IsSpawning || !timerView.Properties.TimerOffset)
            {
                int minutes = (int)time.TotalSeconds / 60;
                builder.Replace("{minutes}", $"{(timerView.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

                int seconds = (int)Math.Round(time.TotalSeconds % 60);
                builder.Replace("{seconds}", $"{(timerView.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
            }
            else
            {
                int offset = RespawnTokensManager.GetTeamDominance(SpawnableTeamType.NineTailedFox) == 1 ? 18 : 14;
                
                int minutes = (int)(time.TotalSeconds + offset) / 60;
                builder.Replace("{minutes}", $"{(timerView.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

                int seconds = (int)Math.Round((time.TotalSeconds + offset) % 60);
                builder.Replace("{seconds}", $"{(timerView.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
            }

            return builder;
        }

        private static StringBuilder SetSpawnableTeam(this StringBuilder builder, TimerView timerView)
        {
            switch (Respawn.NextKnownTeam)
            {
                case SpawnableTeamType.None:
                    return builder;

                case SpawnableTeamType.NineTailedFox:
                    builder.Replace("{team}", !UiuSpawnable ? timerView.Properties.Ntf : timerView.Properties.Uiu);
                    break;

                case SpawnableTeamType.ChaosInsurgency:
                    builder.Replace("{team}", !SerpentsHandSpawnable ? timerView.Properties.Ci : timerView.Properties.Sh);
                    break;
            }

            return builder;
        }

        private static StringBuilder SetSpectatorCountAndTickets(this StringBuilder builder, int? spectatorCount = null)
        {
            builder.Replace("{spectators_num}", spectatorCount?.ToString() ?? Player.List.Count(x => x.Role.Team == Team.Dead && !x.IsOverwatchEnabled).ToString());
            builder.Replace("{ntf_tickets_num}", Mathf.Round(Respawn.NtfTickets).ToString());
            builder.Replace("{ci_tickets_num}", Mathf.Round(Respawn.ChaosTickets).ToString());

            return builder;
        }

        private static StringBuilder SetWarheadStatus(this StringBuilder builder, TimerView timerView)
        {
            WarheadStatus warheadStatus = Warhead.Status;
            builder.Replace("{warhead_status}", timerView.Properties.WarheadStatus[warheadStatus]);
            builder.Replace("{detonation_time}", warheadStatus == WarheadStatus.InProgress ? Mathf.Round(Warhead.DetonationTimer).ToString(CultureInfo.InvariantCulture) : string.Empty);

            return builder;
        }

        private static StringBuilder SetGeneratorCount(this StringBuilder builder)
        {
            int generatorEngaged = 0;
            int generatorCount = 0;

            foreach (Generator generator in Generator.List)
            {
                if (generator.State.HasFlag(GeneratorState.Engaged))
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
            builder.Replace("{tickrate}", Application.targetFrameRate.ToString());

            return builder;
        }

        private static StringBuilder SetHint(this StringBuilder builder, TimerView timerView)
        {
            if (!timerView.Hints.Any())
                return builder;

            builder.Replace("{hint}", timerView.Hints[timerView.HintIndex]);

            return builder;
        }
    }
}
*/