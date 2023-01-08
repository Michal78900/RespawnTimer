namespace RespawnTimer_Base.API
{
    using System.Collections.Generic;
    using Configs;

    public static class API
    {
        public static Config Config { get; internal set; }
        
        public static string DirectoryPath { get; internal set; }
        
        public static List<string> TimerHidden { get; } = new();
    }
}
