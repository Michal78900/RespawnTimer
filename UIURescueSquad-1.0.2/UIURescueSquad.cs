using System;
using Exiled.API.Features;
using UIURescueSquad.Handlers;
using HarmonyLib;

namespace UIURescueSquad
{
    public class UIURescueSquad : Plugin<Config>
    {
        private static readonly Lazy<UIURescueSquad> LazyInstance = new Lazy<UIURescueSquad>(() => new UIURescueSquad());
        public static UIURescueSquad Instance => LazyInstance.Value;

        private Harmony hInstance;

        public override string Name { get; } = "UIU Rescue Squad";
        public override string Author { get; } = "JesusQC";
        public override string Prefix { get; } = "UIURescueSquad";

        private UIURescueSquad() { }

        public EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            base.OnEnabled();

            hInstance = new Harmony("jesus.uiurescuesquad");
            hInstance.PatchAll();

            EventHandlers = new EventHandlers();

            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += EventHandlers.OnAnnouncingMTF;
            Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.OnTeamRespawn;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnChanging;
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnDying;
        }
        public override void OnDisabled()
        {
            base.OnDisabled();

            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= EventHandlers.OnAnnouncingMTF;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandlers.OnTeamRespawn;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnChanging;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnDying;

            hInstance.UnpatchAll();
            EventHandlers = null;
        }
    }
}
