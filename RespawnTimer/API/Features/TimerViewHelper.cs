namespace RespawnTimer.API.Features;

using System;
using System.Globalization;
using System.Linq;
using GameCore;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using Respawning;
using UnityEngine;
#if EXILED
using Exiled.API.Enums;
using Exiled.API.Features;
#else
    using PluginAPI.Core;
#endif

public partial class TimerView
{
    private void SetAllProperties(int? spectatorCount = null)
    {
        SetRoundTime();
        SetMinutesAndSeconds();
        SetSpawnableTeam();
        SetSpectatorCountAndSpawnChance(spectatorCount);
        SetWarheadStatus();
        SetGeneratorCount();
        SetTpsAndTickrate();
        SetHint();
    }

    private void SetRoundTime()
    {
        
        int minutes = RoundStart.RoundLength.Minutes;
        StringBuilder.Replace("{round_minutes}", $"{(Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

        int seconds = RoundStart.RoundLength.Seconds;
        StringBuilder.Replace("{round_seconds}", $"{(Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
    }

    private void SetMinutesAndSeconds()
    {
        TimeSpan time = TimeSpan.FromSeconds(RespawnManager.Singleton._timeForNextSequence - RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds);

        if (RespawnManager.Singleton._curSequence is RespawnManager.RespawnSequencePhase.PlayingEntryAnimations or RespawnManager.RespawnSequencePhase.SpawningSelectedTeam ||
            !Properties.TimerOffset)
        {
            int minutes = (int)time.TotalSeconds / 60;
            StringBuilder.Replace("{minutes}", $"{(Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

            int seconds = (int)Math.Round(time.TotalSeconds % 60);
            StringBuilder.Replace("{seconds}", $"{(Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
        }
        else
        {
            int offset = RespawnTokensManager.Counters[1].Amount >= 50 ? 18 : 14;

            int minutes = (int)(time.TotalSeconds + offset) / 60;
            StringBuilder.Replace("{minutes}", $"{(Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

            int seconds = (int)Math.Round((time.TotalSeconds + offset) % 60);
            StringBuilder.Replace("{seconds}", $"{(Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
        }
    }

    private void SetSpawnableTeam()
    {
        switch (Respawn.NextKnownTeam)
        {
            case SpawnableTeamType.None:
                return;

#if EXILED
            case SpawnableTeamType.NineTailedFox:
                StringBuilder.Replace("{team}", !API.UiuSpawnable ? Properties.Ntf : Properties.Uiu);
                break;

            case SpawnableTeamType.ChaosInsurgency:
                StringBuilder.Replace("{team}", !API.SerpentsHandSpawnable ? Properties.Ci : Properties.Sh);
                break;
#else
            case SpawnableTeamType.NineTailedFox:
                StringBuilder.Replace("{team}", Properties.Ntf);
                break;

            case SpawnableTeamType.ChaosInsurgency:
                StringBuilder.Replace("{team}", Properties.Ci);
                break;
#endif
        }
    }

    private void SetSpectatorCountAndSpawnChance(int? spectatorCount = null)
    {
#if EXILED
        StringBuilder.Replace("{spectators_num}", spectatorCount?.ToString() ?? Player.List.Count(x => x.Role.Team == Team.Dead && !x.IsOverwatchEnabled).ToString());
#else
        StringBuilder.Replace("{spectators_num}", spectatorCount?.ToString() ?? Player.GetPlayers().Count(x => x.Role == RoleTypeId.Spectator && !x.IsOverwatchEnabled).ToString());
#endif
        // Backwards compatibility
        StringBuilder.Replace("{ntf_tickets_num}", "{ntf_spawn_chance}");
        StringBuilder.Replace("{ci_tickets_num}", "{ci_spawn_chance}");
        //

        StringBuilder.Replace("{ntf_spawn_chance}", Mathf.Round(RespawnTokensManager.Counters[1].Amount).ToString());
        StringBuilder.Replace("{ci_spawn_chance}", Mathf.Round(RespawnTokensManager.Counters[0].Amount).ToString());
    }

    private void SetWarheadStatus()
    {
#if EXILED
        WarheadStatus warheadStatus = Warhead.Status;
        StringBuilder.Replace("{warhead_status}", Properties.WarheadStatus[warheadStatus]);
        StringBuilder.Replace(
            "{detonation_time}",
            warheadStatus == WarheadStatus.InProgress ? Mathf.Round(Warhead.DetonationTimer).ToString(CultureInfo.InvariantCulture) : string.Empty);
#else
        string warheadStatus = Warhead.IsDetonationInProgress ? Warhead.IsDetonated ? "Detonated" : "InProgress" : Warhead.LeverStatus ? "Armed" : "NotArmed";
        StringBuilder.Replace("{warhead_status}", Properties.WarheadStatus[warheadStatus]);
        StringBuilder.Replace("{detonation_time}", warheadStatus == "InProgress" ? Mathf.Round(Warhead.DetonationTime).ToString(CultureInfo.InvariantCulture) : string.Empty);
#endif
    }

    private void SetGeneratorCount()
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

        StringBuilder.Replace("{generator_engaged}", generatorEngaged.ToString());
        StringBuilder.Replace("{generator_count}", generatorCount.ToString());
        */
        StringBuilder.Replace("{generator_engaged}", Scp079Recontainer.AllGenerators.Count(x => x.Engaged).ToString());
        StringBuilder.Replace("{generator_count}", "3");
    }

    private void SetTpsAndTickrate()
    {
        StringBuilder.Replace("{tps}", Math.Round(1.0 / Time.smoothDeltaTime).ToString(CultureInfo.InvariantCulture));
        StringBuilder.Replace("{tickrate}", Application.targetFrameRate.ToString());
    }

    private void SetHint()
    {
        if (!Hints.Any())
            return;

        StringBuilder.Replace("{hint}", Hints[HintIndex]);
    }
}