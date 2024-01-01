#if EXILED
namespace RespawnTimer.API.Features.ExternalTeams
{
    using System;
    using System.Reflection;

    public class SerpentsHandTeam : ExternalTeamChecker
    {
        public override void Init(Assembly assembly)
        {
            PluginEnabled = true;
            
            Type mainClass = assembly.GetType("SerpentsHand.Plugin");
            Instance = mainClass.GetField("Instance").GetValue(null);
            FieldInfo = mainClass.GetField("IsSpawnable");
        }
    }
}
#endif