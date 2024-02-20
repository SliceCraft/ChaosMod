using ChaosMod.Activator;
using ChaosMod.Patches;

namespace ChaosMod.Effects
{
    internal class OneHitExplosionsEffect : Effect
    {
        public override string GetEffectName()
        {
            return "One Hit Explode";
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
            PlayerControllerBPatch.setOneHitExplode(true);
        }

        public override void StopEffect()
        {
            PlayerControllerBPatch.setOneHitExplode(false);
        }
    }
}
