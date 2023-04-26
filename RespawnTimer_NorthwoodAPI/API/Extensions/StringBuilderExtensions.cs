namespace RespawnTimer_NorthwoodAPI.API.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using PluginAPI.Core;
    using GameCore;
    using PlayerRoles;
    using PlayerRoles.PlayableScps.Scp079;
    using Respawning;
    
    using static Features.TimerView;

    public static class StringBuilderExtensions
    {
        public static StringBuilder SetAllProperties(this StringBuilder builder, int? spectatorCount = null) => builder
            .SetRoundTime()
            .SetMinutesAndSeconds()
            .SetSpawnableTeam()
            .SetSpectatorCountAndTickets(spectatorCount)
            .SetWarheadStatus()
            .SetGeneratorCount()
            .SetTpsAndTickrate()
            .SetHint();

        private static StringBuilder SetRoundTime(this StringBuilder builder)
        {
            int minutes = RoundStart.RoundLength.Minutes;
            builder.Replace("{round_minutes}", $"{(Current.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

            int seconds = RoundStart.RoundLength.Seconds;
            builder.Replace("{round_seconds}", $"{(Current.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
            
            return builder;
        }

        private static StringBuilder SetMinutesAndSeconds(this StringBuilder builder)
        {
            TimeSpan time = TimeSpan.FromSeconds(RespawnManager.Singleton._timeForNextSequence - RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds);
            
            if (RespawnManager.Singleton._curSequence is RespawnManager.RespawnSequencePhase.PlayingEntryAnimations or RespawnManager.RespawnSequencePhase.SpawningSelectedTeam || !Current.Properties.TimerOffset)
            {
                int minutes = (int)time.TotalSeconds / 60;
                builder.Replace("{minutes}", $"{(Current.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

                int seconds = (int)Math.Round(time.TotalSeconds % 60);
                builder.Replace("{seconds}", $"{(Current.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
            }
            else
            {
                int offset = RespawnTokensManager.Counters[1].Amount >= 50 ? 18 : 14;
                
                int minutes = (int)(time.TotalSeconds + offset) / 60;
                builder.Replace("{minutes}", $"{(Current.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

                int seconds = (int)Math.Round((time.TotalSeconds + offset) % 60);
                builder.Replace("{seconds}", $"{(Current.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
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
                    builder.Replace("{team}", Current.Properties.Ntf);
                    break;

                case SpawnableTeamType.ChaosInsurgency:
                    builder.Replace("{team}", Current.Properties.Ci);
                    break;
            }

            return builder;
        }

        private static StringBuilder SetSpectatorCountAndTickets(this StringBuilder builder, int? spectatorCount = null)
        {
            builder.Replace("{spectators_num}", spectatorCount?.ToString() ?? Player.GetPlayers().Count(x => x.Role == RoleTypeId.Spectator && !x.IsOverwatchEnabled).ToString());
            builder.Replace("{ntf_tickets_num}", Mathf.Round(RespawnTokensManager.Counters[1].Amount).ToString());
            builder.Replace("{ci_tickets_num}", Mathf.Round(RespawnTokensManager.Counters[0].Amount).ToString());

            return builder;
        }

        private static StringBuilder SetWarheadStatus(this StringBuilder builder)
        {
            builder.Replace("{warhead_status}", Warhead.IsDetonated ? "Detonated" : Warhead.IsDetonationInProgress ? "Detonating" : "Not detonated");

            return builder;
        }

        private static StringBuilder SetGeneratorCount(this StringBuilder builder)
        {
            /*
            int generatorEngaged = 0;
            int generatorCount = 0;

            foreach (Generator generator in Generator.List)
            {
                if (generator.State.HasFlag(GeneratorState.Engaged))
                    generatorEngaged++;

                generatorCount++;
            }
            */

            builder.Replace("{generator_engaged}", Scp079Recontainer.AllGenerators.Count(x => x.Engaged).ToString());
            builder.Replace("{generator_count}", "3");
            return builder;
        }

        private static StringBuilder SetTpsAndTickrate(this StringBuilder builder)
        {
            builder.Replace("{tps}", Math.Round(1.0 / Time.smoothDeltaTime).ToString(CultureInfo.InvariantCulture));
            builder.Replace("{tickrate}", Application.targetFrameRate.ToString());

            return builder;
        }

        private static StringBuilder SetHint(this StringBuilder builder)
        {
            if (!Current.Hints.Any())
                return builder;

            builder.Replace("{hint}", Current.Hints[Current.HintIndex]);

            return builder;
        }
    }
}
