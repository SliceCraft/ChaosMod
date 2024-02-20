using ChaosMod.Activator;

namespace ChaosMod.Effects
{
    internal class WarpSpeedEffect : Effect
    {
        private float oldSpeed = 4.6f; // Pretty sure this is the default value so I'm putting this here just in case
        public override string GetEffectName()
        {
            return "Warp Speed";
        }

        public override bool IsTimedEffect()
        {
            return true;
        }

        public override long GetEffectLength()
        {
            return 30000;
        }

        public override void StartEffect()
        {
            oldSpeed = GameNetworkManager.Instance.localPlayerController.movementSpeed;
            GameNetworkManager.Instance.localPlayerController.movementSpeed = 20f;
        }

        public override void StopEffect()
        {
            GameNetworkManager.Instance.localPlayerController.movementSpeed = oldSpeed;
        }
    }
}
