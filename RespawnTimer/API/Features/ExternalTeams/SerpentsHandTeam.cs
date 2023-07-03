namespace RespawnTimer.API.Features.ExternalTeams
{
    using System;
    using System.Reflection;
    using Exiled.API.Features;

    public class SerpentsHandTeam : ExternalTeamChecker
    {
        public override void Init(Assembly assembly)
        {
            PluginEnabled = true;

            Singleton = null;
            FieldInfo = assembly.GetType("SerpentsHand.Plugin").GetField("IsSpawnable");
        }
    }
}