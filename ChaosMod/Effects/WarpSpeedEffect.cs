using ChaosMod.Activator;

namespace ChaosMod.Effects
{
    internal class WarpSpeedEffect : Effect
    {
        // Pretty sure this is the default value so I'm putting this here just in case

        // 2nd note: apparently this patch does not work well with other mods that modify movement speed
        // not a high priority since at worst this effect doesn't do anything and this is partially out of our control
        // would still be worth it to investigate in the future to make the mod quality better
        private float oldSpeed = 4.6f;
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
