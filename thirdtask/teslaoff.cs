using CommandSystem;
using Exiled.API.Features;
using System;

namespace thirdtask
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Teslaoff : CommandSystem.ICommand
    {
        public string Command => "offtesla";

        public string[] Aliases => new string[] { "offt" };

        public string Description => "Отключает близжающую теслу на 1 минуту";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var igrok = Player.Get(sender);
            if(igrok.CurrentRoom.TeslaGate != null)
            {
                igrok.CurrentRoom.TeslaGate.InactiveTime = 60;
                response = "Тесла успешна отключена";
                return true;
            }
            else
            {
                response = "В комнате нет теслы";
                return false;
            }

        }
    }
}
