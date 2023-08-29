using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using System;
using System.Collections.Generic;
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
            if (arguments.Count == 1 && int.TryParse(arguments.At(0), out var radius))
            {
                var predmetov = 0;
                var trupov = 0;

                foreach (Pickup item in Pickup.List)
                {
                    if (Vector3.Distance(item.Position, plaor.Position) <= radius)
                    {
                        item.Destroy();
                        ++predmetov;
                    }
                }
                foreach (Ragdoll item in Ragdoll.List)
                {
                    if (Vector3.Distance(item.Position, plaor.Position) <= radius)
                    {
                        item.Destroy();
                        ++trupov;
                    }
                }
                response = $"Было удаленно {trupov} трупов и {predmetov} предметов в радиусе {radius}.";
                return true;
            }
            response = "Не правильно введён радиус.";
            return false;
        }
    }
}
