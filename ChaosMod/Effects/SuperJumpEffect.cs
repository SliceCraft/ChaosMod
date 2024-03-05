using ChaosMod.Activator;
using ChaosMod.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects
{
    internal class SuperJumpEffect : Effect
    {
        private float oldForce = 5f; // 5f should be the default speed
        public override string GetEffectName()
        {
            return "Super Jump";
        }

        public override bool IsTimedEffect()
        {
            return true;
        }

        public override long GetEffectLength()
        {
            return 20000;
        }

        public override void StartEffect()
        {
            oldForce = GameNetworkManager.Instance.localPlayerController.jumpForce;
            GameNetworkManager.Instance.localPlayerController.jumpForce = 40f;
            PlayerControllerBPatch.SetSingleUseFallImmunity(true);
        }

        public override void UpdateEffect()
        {
            PlayerControllerBPatch.SetSingleUseFallImmunity(true);
        }

        public override void StopEffect()
        {
            GameNetworkManager.Instance.localPlayerController.jumpForce = oldForce;
            if (GameNetworkManager.Instance.localPlayerController.thisController.isGrounded) PlayerControllerBPatch.SetSingleUseFallImmunity(false);
        }
    }
}
