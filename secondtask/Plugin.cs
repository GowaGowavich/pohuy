using Exiled.API.Features;
using Pochemu;
using UnityEngine;

namespace secondtask
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "gowa";
        public override string Name => "pochemu";
        public override string Prefix => "pizda";
        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Server.LocalReporting += Server_LocalReporting;
            Exiled.Events.Handlers.Player.TriggeringTesla += Player_TriggeringTesla;
            Exiled.Events.Handlers.Map.ExplodingGrenade += Map_ExplodingGrenade;
            Exiled.Events.Handlers.Server.EndingRound += Server_EndingRound;
        }

        private void Server_EndingRound(Exiled.Events.EventArgs.Server.EndingRoundEventArgs ev)
        {
            Reportslist.Reports.Clear();
        }

        private void Map_ExplodingGrenade(Exiled.Events.EventArgs.Map.ExplodingGrenadeEventArgs ev)
        {
            if (ev.Projectile.Info.ItemId == ItemType.GrenadeHE)
            {
                foreach (Lift lift in Lift.List)
                {
                    if (lift.IsInElevator(ev.Projectile.Position))
                    if (Random.Range(1, 101) <= 50)
                    {
                        lift.TryStart(lift.CurrentLevel == 1 ? 0 : 1);
                    }
                }
            }
        }

        private void Player_TriggeringTesla(Exiled.Events.EventArgs.Player.TriggeringTeslaEventArgs ev)
        {
            if (ev.Player.CurrentItem != null && Config.Items.Contains(ev.Player.CurrentItem.Type))
            {
                ev.IsTriggerable = false;
            }
        }

        public void Server_LocalReporting(Exiled.Events.EventArgs.Player.LocalReportingEventArgs ev)
        {
            var igrok = ev.Player.Nickname;
            var pidoras = ev.Target.Nickname;
            Reportslist.Reports.Add($"Жалоба от {igrok} на {pidoras}. Причина:{ev.Reason}");
            foreach (Player admins in Player.List)
            {
                if (admins.RemoteAdminAccess)
                {
                    admins.Broadcast(10, $"Игрок {igrok} подал жалобу на игрока {pidoras}. \nПричина:{ev.Reason}");
                }
            }
        }
    }
}
