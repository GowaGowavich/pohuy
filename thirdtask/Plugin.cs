using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Hazards;
using Hazards;
using MEC;
using PlayerRoles;
using Respawning;
using System.Collections.Generic;
using UnityEngine;

namespace thirdtask
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "gowa";
        
        public override string Name => "pochemu";
        
        public override string Prefix => "pizda";

        public static string Status = "Не готов";

        public static string Team = "<color=blue>НТФ";

        public static List<Player> Spectators { get; } = new List<Player>() { };
        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Scp914.UpgradingPlayer += Scp914_UpgradingPlayer;
            Exiled.Events.Handlers.Player.Died += Player_Died;
            Exiled.Events.Handlers.Player.StayingOnEnvironmentalHazard += Player_StayingOnEnvironmentalHazard;
            Exiled.Events.Handlers.Warhead.ChangingLeverStatus += Warhead_ChangingLeverStatus;
            Exiled.Events.Handlers.Player.ChangingRole += Player_ChangingRole;
        }
        public IEnumerator<float> MyCoroutine()
        {
            for (; ; )
            {
                var vremya = $"{Round.ElapsedTime.Hours}:{Round.ElapsedTime.Minutes}:{Round.ElapsedTime.Seconds}";
                var status = Status;
                var timespawn = $"{Respawn.TimeUntilSpawnWave.Minutes}:{Respawn.TimeUntilSpawnWave.Seconds} до спавна {Team}";

                foreach (Player igrok in Spectators)
                {
                    igrok.ShowHint($"<align=left>Время раунда: {vremya}\nСтатус рычага: {status}\n{timespawn}</align>", 1);
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
        private void Player_ChangingRole(Exiled.Events.EventArgs.Player.ChangingRoleEventArgs ev)
        {
            switch (RespawnTokensManager.DominatingTeam)
            {
                case SpawnableTeamType.NineTailedFox:
                    Team = "<color=blue>НТФ";
                    break;
                case SpawnableTeamType.ChaosInsurgency:
                    Team = "<color=green>ПХ";
                    break;
            }
            if (Round.IsStarted)
            {
                if (ev.NewRole == RoleTypeId.Spectator || ev.NewRole == RoleTypeId.Overwatch)
                {
                    Spectators.Add(ev.Player);
                }
                else
                {
                    Spectators.Remove(ev.Player);
                }

                Timing.RunCoroutine(MyCoroutine());
            }
        }

        private void Warhead_ChangingLeverStatus(Exiled.Events.EventArgs.Warhead.ChangingLeverStatusEventArgs ev)
        {
            if (Warhead.IsDetonated != true)
            {
                switch (Warhead.CanBeStarted)
                {
                    case true:
                        Status = "Готов";
                        break;

                    case false:
                        Status = "Не готов";
                        break;
                }
            }
            else { Status = "Боеголовка взорвана"; };
        }

        private void Player_StayingOnEnvironmentalHazard(Exiled.Events.EventArgs.Player.StayingOnEnvironmentalHazardEventArgs ev)
        {
            var sinkhole = ev.Hazard;
            if (ev.Hazard is not SinkholeHazard)
                return;

            if (ev.Player.IsScp || ev.Player.IsGodModeEnabled)
                return;

            if ((double)Vector3.Distance(ev.Player.Position, sinkhole.Position) / 2 < sinkhole.MaxDistance)
                return;

            ev.Player.DisableEffect<Sinkhole>();
            ev.Player.EnableEffect<Corroding>();
        }

        private void Player_Died(Exiled.Events.EventArgs.Player.DiedEventArgs ev)
        {
            foreach (Player player in Player.List)
            {
                if (Warhead.IsInProgress != true)
                {
                    if (player.Role == RoleTypeId.Scientist && player.IsAlive == true || player.Role == RoleTypeId.FacilityGuard && player.IsAlive == true)
                    {
                        break;
                    }
                    else
                    {
                        Warhead.Start();
                        Warhead.IsLocked = true;
                    }
                }
            }
        }

        private void Scp914_UpgradingPlayer(Exiled.Events.EventArgs.Scp914.UpgradingPlayerEventArgs ev)
        {
            var igrok = ev.Player;
            var efect = Config.ChanceDamage;
            var damage = Config.ChanceDamage;
            var tp = Config.ChanceTp;
            if (Random.Range(0, 101) < efect)
            {
                var effect = Config.NegEffect.RandomItem();
                igrok.EnableEffect(effect);
            }
            if (Random.Range(0, 101) < damage)
            {
                igrok.Hurt(Random.Range(0, 100));
            }
            if (Random.Range(0, 101) < tp)
            {
                ev.IsAllowed = false;
                igrok.RandomTeleport(typeof(Room));
            }
        }
    }
}
