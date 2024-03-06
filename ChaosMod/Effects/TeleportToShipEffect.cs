using ChaosMod.Activator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects
{
    internal class TeleportToShipEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Teleport to Ship";
        }

        public override void StartEffect()
        {
            GameNetworkManager.Instance.localPlayerController.isInsideFactory = false;
            GameNetworkManager.Instance.localPlayerController.TeleportPlayer(StartOfRound.Instance.playerSpawnPositions[0].position);
        }
    }
}
