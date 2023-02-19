namespace RespawnTimer.API.Features;

using System;
using System.Globalization;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using Respawning;
using UnityEngine;

public partial class TimerView
{
    private void SetAllProperties(int? spectatorCount = null)
    {
        SetRoundTime();
        SetMinutesAndSeconds();
        SetSpawnableTeam();
        SetSpectatorCountAndTickets(spectatorCount);
        SetWarheadStatus();
        SetGeneratorCount();
        SetTpsAndTickrate();
        SetHint();
    }

    private void SetRoundTime()
    {
        int minutes = Round.ElapsedTime.Minutes;
        StringBuilder.Replace("{round_minutes}", $"{(Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

        int seconds = Round.ElapsedTime.Seconds;
        StringBuilder.Replace("{round_seconds}", $"{(Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
    }

    private void SetMinutesAndSeconds()
    {
        TimeSpan time = Respawn.TimeUntilSpawnWave;

        if (Respawn.IsSpawning || !Properties.TimerOffset)
        {
            int minutes = (int)time.TotalSeconds / 60;
            StringBuilder.Replace("{minutes}", $"{(Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

            int seconds = (int)Math.Round(time.TotalSeconds % 60);
            StringBuilder.Replace("{seconds}", $"{(Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
        }
        else
        {
            int offset = RespawnTokensManager.GetTeamDominance(SpawnableTeamType.NineTailedFox) == 1 ? 18 : 14;

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

            case SpawnableTeamType.NineTailedFox:
                StringBuilder.Replace("{team}", !API.UiuSpawnable ? Properties.Ntf : Properties.Uiu);
                break;

            case SpawnableTeamType.ChaosInsurgency:
                StringBuilder.Replace("{team}", !API.SerpentsHandSpawnable ? Properties.Ci : Properties.Sh);
                break;
        }
    }

    private void SetSpectatorCountAndTickets(int? spectatorCount = null)
    {
        StringBuilder.Replace("{spectators_num}", spectatorCount?.ToString() ?? Player.List.Count(x => x.Role.Team == Team.Dead && !x.IsOverwatchEnabled).ToString());
        StringBuilder.Replace("{ntf_tickets_num}", Mathf.Round(Respawn.NtfTickets).ToString());
        StringBuilder.Replace("{ci_tickets_num}", Mathf.Round(Respawn.ChaosTickets).ToString());
    }

    private void SetWarheadStatus()
    {
        WarheadStatus warheadStatus = Warhead.Status;
        StringBuilder.Replace("{warhead_status}", Properties.WarheadStatus[warheadStatus]);
        StringBuilder.Replace("{detonation_time}", warheadStatus == WarheadStatus.InProgress ? Mathf.Round(Warhead.DetonationTimer).ToString(CultureInfo.InvariantCulture) : string.Empty);
    }

    private void SetGeneratorCount()
    {
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
    }

    private void SetTpsAndTickrate()
    {
        StringBuilder.Replace("{tps}", Server.Tps.ToString(CultureInfo.InvariantCulture));
        StringBuilder.Replace("{tickrate}", Application.targetFrameRate.ToString());
    }

    private void SetHint()
    {
        if (!Hints.Any())
            return;

        StringBuilder.Replace("{hint}", Hints[HintIndex]);
    }
}