namespace RespawnTimer.API.Features.ExternalTeams
{
    using System.Reflection;

    public abstract class ExternalTeamChecker
    {
        public abstract void Init(Assembly assembly);

        public bool IsSpawnable
        {
            get
            {
                if (!PluginEnabled)
                    return false;

                return (bool)FieldInfo.GetValue(Singleton);
            }
        }

        protected bool PluginEnabled { get; set; }
        protected FieldInfo FieldInfo { get; set; }
        protected object Singleton { get; set; }
    }
}