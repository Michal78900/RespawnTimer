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
            
            Type mainClass = assembly.GetType("SerpentsHand.SerpentsHand");
            Singleton = mainClass.GetField("Singleton").GetValue(null);
            FieldInfo = mainClass.GetField("IsSpawnable");
        }
    }
}