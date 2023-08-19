using Exiled.API.Features;
using Exiled.Events.Extensions;
using Exiled.Events.Handlers;
using MEC;
using System.Collections.Generic;

namespace pohuy
{
    public class Plugin: Plugin<Config>
    {
        public override string Author => "yjjz";
        public override string Name => "pohuy";
        public override string Prefix => "ph";
        public static Plugin Instance { get; } = new();

        public static Config Configs = Instance.Config;
        private Plugin()
        {
        }
        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.Verified += Player_Verified;
        }

        private void Player_Verified(Exiled.Events.EventArgs.Player.VerifiedEventArgs ev)
        {
            Timing.RunCoroutine(MyCoroutine(ev.Player));
        }

        public IEnumerator<float> MyCoroutine(Exiled.API.Features.Player player)
        {
            for (; ; )
            {
                player.Broadcast(Configs.Time,"kek");
                yield return Timing.WaitForSeconds(5f);
            }
        }
    }
}
