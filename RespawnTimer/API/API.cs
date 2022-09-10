namespace RespawnTimer.API
{
    using System;
    using System.Collections.Generic;
    using Features;

    public static class API
    {
        public static TimerView TimerView { get; internal set; }
        
        public static List<string> TimerHidden { get; } = new();

        public static bool SerpentsHandSpawnable
        {
            get
            {
                if (RespawnTimer.SerpentsHandAssembly == null)
                    return false;

                Type mainClass = RespawnTimer.SerpentsHandAssembly.GetType("SerpentsHand");
                object singleton = mainClass?.GetField("Singleton").GetValue(null);

                return (bool)mainClass?.GetField("IsSpawnable").GetValue(singleton)!;
            }
        }

        public static bool UiuSpawnable
        {
            get
            {
                if (RespawnTimer.UIURescueSquadAssembly == null)
                    return false;

                return (bool)RespawnTimer.UIURescueSquadAssembly.GetType("UIURescueSquad.EventHandlers")?.GetField("IsSpawnable").GetValue(null)!;
            }
        }
    }
}
