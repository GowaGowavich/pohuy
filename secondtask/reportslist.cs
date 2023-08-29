using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;

namespace Pochemu
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Reportslist : CommandSystem.ICommand
    {
        public string Command => "reportslist";

        public string[] Aliases => new string[] { "replist" };

        public string Description => "Тут список всех плохих людей!";
        public static List<string> Reports { get; } = new List<string>() {};

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var admin = Player.Get(sender);
            foreach (var reportik in Reports)
            {
                admin.RemoteAdminMessage(reportik);
            }
            response = "";
            return true;
        }
    }
}
