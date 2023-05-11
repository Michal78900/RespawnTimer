namespace RespawnTimer_NorthwoodAPI.API.Features;

using System;
using System.Globalization;
using System.Linq;
using GameCore;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PluginAPI.Core;
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

            case SpawnableTeamType.NineTailedFox:
                StringBuilder.Replace("{team}", Properties.Ntf);
                break;

            case SpawnableTeamType.ChaosInsurgency:
                StringBuilder.Replace("{team}", Properties.Ci);
                break;
        }
    }

    private void SetSpectatorCountAndTickets(int? spectatorCount = null)
    {
        StringBuilder.Replace("{spectators_num}", spectatorCount?.ToString() ?? Player.GetPlayers().Count(x => x.Role == RoleTypeId.Spectator && !x.IsOverwatchEnabled).ToString());
        StringBuilder.Replace("{ntf_tickets_num}", Mathf.Round(RespawnTokensManager.Counters[1].Amount).ToString());
        StringBuilder.Replace("{ci_tickets_num}", Mathf.Round(RespawnTokensManager.Counters[0].Amount).ToString());
    }

    private void SetWarheadStatus()
    {
        string warheadStatus = Warhead.IsDetonationInProgress ? Warhead.IsDetonated ? "Detonated" : "InProgress" : Warhead.LeverStatus ? "Armed" : "NotArmed";
        StringBuilder.Replace("{warhead_status}", Properties.WarheadStatus[warheadStatus]);
        StringBuilder.Replace("{detonation_time}", warheadStatus == "InProgress" ? Mathf.Round(Warhead.DetonationTime).ToString(CultureInfo.InvariantCulture) : string.Empty);
    }

    private void SetGeneratorCount()
    {
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