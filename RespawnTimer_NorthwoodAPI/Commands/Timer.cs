namespace RespawnTimer_NorthwoodAPI.Commands
{
    using System;
    using CommandSystem;
    using API;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Timer : ICommand
    {
        public string Command => "timer";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Shows / hides RespawnTimer.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string userId = ((CommandSender)sender).SenderId;

            if (!API.TimerHidden.Remove(userId))
            {
                API.TimerHidden.Add(userId);

                response = "<color=red>Respawn Timer has been hidden!</color>";
                return true;
            }

            response = "<color=green>Respawn Timer has been shown!</color>";
            return true;
        }
    }
}
