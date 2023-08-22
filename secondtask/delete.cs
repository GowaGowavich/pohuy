using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;

namespace secondtask
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Delete : CommandSystem.ICommand
    {
        public string Command => "delete";

        public string[] Aliases => new string[] { "del" };

        public string Description => "Удали все вокруг!";

        public List<string> Reports => new List<string> { };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var plaor = Player.Get(sender);
            if (arguments.Count == 1)
            {
                var predmetov = 0;
                var trupov = 0;
                int radiuse = int.Parse(string.Join(" ", arguments));
                foreach (Pickup item in Pickup.List)
                {
                    if (Vector3.Distance(item.Position, plaor.Position) <= radiuse)
                    {
                        item.Destroy();
                        predmetov = predmetov + 1;
                    }
                }
                foreach (Ragdoll item in Ragdoll.List)
                {
                    if (Vector3.Distance(item.Position, plaor.Position) <= radiuse)
                    {
                        item.Destroy();
                        trupov = trupov + 1;
                    }
                }
                plaor.RemoteAdminMessage($"Было удаленно {trupov} трупов и {predmetov} предметов в радиусе {radiuse}.");
                response = "";
                return true;
            }
            plaor.RemoteAdminMessage("Не правильно введён радиус.");
            response = "";
            return false;
        }
    }
}
