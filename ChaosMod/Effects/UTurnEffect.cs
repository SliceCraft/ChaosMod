using ChaosMod.Activator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects
{
    internal class UTurnEffect : Effect
    {
        public override string GetEffectName()
        {
            return "U Turn";
        }

        public override void StartEffect()
        {
            GameNetworkManager.Instance.localPlayerController.TeleportPlayer(GameNetworkManager.Instance.localPlayerController.thisPlayerBody.transform.position, true, GameNetworkManager.Instance.localPlayerController.thisPlayerBody.transform.eulerAngles.y + 180);
        }
    }
}
