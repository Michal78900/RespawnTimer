namespace RespawnTimer.Commands
{
    using CommandSystem;
    using Exiled.API.Features;
    using System;

    using static API.API;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Timer : ICommand
    {
        public string Command => "timer";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Shows / hides RespawnTimer.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string userId = Player.Get(sender).UserId;

            if (!TimerHidden.Remove(userId))
            {
                TimerHidden.Add(userId);

                response = "<color=red>Respawn Timer has been hidden!</color>";
                return true;
            }

            response = "<color=green>Respawn Timer has been shown!</color>";
            return true;
        }
    }
}
