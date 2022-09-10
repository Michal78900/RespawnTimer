namespace RespawnTimer.API.Features
{
    public class TimerView
    {
        public TimerView(string beforeRespawnString, string duringRespawnString, Properties properties)
        {
            BeforeRespawnString = beforeRespawnString;
            DuringRespawnString = duringRespawnString;
            Properties = properties;
        }
        
        public string BeforeRespawnString { get; }
        
        public string DuringRespawnString { get; }
        
        public Properties Properties { get; }
    }
}