using ChaosMod.Activator;
using ChaosMod.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class TeleportToHeavenEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Teleport To Heaven";
        }

        public override bool IsTimedEffect()
        {
            return false;
        }

        public override void StartEffect()
        {
            Vector3 currentpos = GameNetworkManager.Instance.localPlayerController.thisPlayerBody.position;
            GameNetworkManager.Instance.localPlayerController.TeleportPlayer(new Vector3(currentpos.x, currentpos.y + 700, currentpos.z));
            PlayerControllerBPatch.SetSingleUseFallImmunity(true);
        }
    }
}
