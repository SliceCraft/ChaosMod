using ChaosMod.Activator;

namespace ChaosMod.Effects
{
    internal class FasterEffectsEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Faster Effects";
        }

        public override bool IsTimedEffect()
        {
            return true;
        }

        public override long GetEffectLength()
        {
            return 40000;
        }

        public override void StartEffect()
        {
            TimerSystem.SetHalfEffectTime(true);
        }

        public override void StopEffect()
        {
            // TODO: Calculate new start time to make sure the effect time stays halved during the remainder of that cycle
            TimerSystem.SetHalfEffectTime(false);
        }
    }
}
