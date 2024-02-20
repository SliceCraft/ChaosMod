using ChaosMod.Activator;
using ChaosMod.Patches;

namespace ChaosMod.Effects
{
    internal class InfiniteSprintEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Infinite Sprint";
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
            PlayerControllerBPatch.setInfiniteSprint(true);
        }

        public override void StopEffect()
        {
            PlayerControllerBPatch.setInfiniteSprint(false);
        }
    }
}
