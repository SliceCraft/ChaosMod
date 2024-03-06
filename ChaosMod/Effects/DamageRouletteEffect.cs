using ChaosMod.Activator;
using System;

namespace ChaosMod.Effects
{
    internal class DamageRouletteEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Damage Roulette";
        }

        public override void StartEffect()
        {
            Random rnd = new Random();
            if (GameNetworkManager.Instance.localPlayerController.health < 25 && rnd.Next(101) < 50)
            {
                Landmine.SpawnExplosion(GameNetworkManager.Instance.localPlayerController.thisPlayerBody.position);
                return;
            }
            int r = rnd.Next(30);
            GameNetworkManager.Instance.localPlayerController.DamagePlayer(11 + r);
        }
    }
}
