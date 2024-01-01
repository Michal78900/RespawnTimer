namespace RespawnTimer.API
{
    using System.Collections.Generic;
#if EXILED
    using Features.ExternalTeams;
#endif

    public static class API
    {
        public static List<string> TimerHidden { get; } = new();

#if EXILED
        public static bool SerpentsHandSpawnable => SerpentsHandTeam.IsSpawnable;

        public static bool UiuSpawnable => UiuTeam.IsSpawnable;

        internal static readonly ExternalTeamChecker SerpentsHandTeam = new SerpentsHandTeam();
        internal static readonly ExternalTeamChecker UiuTeam = new UiuTeam();
#endif
    }
}