namespace RespawnTimer.API.Features.ExternalTeams
{
    using System.Reflection;

    public class UiuTeam : ExternalTeamChecker
    {
        public override void Init(Assembly assembly)
        {
            PluginEnabled = true;

            Singleton = null;
            FieldInfo = assembly.GetType("UIURescueSquad.EventHandlers").GetField("IsSpawnable");
        }
    }
}