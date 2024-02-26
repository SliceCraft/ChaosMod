using ChaosMod.Activator;
using System;

namespace ChaosMod.Effects
{
    internal class RandomEffectEffect : Effect
    {
        private bool effectStarted = false;
        private Effect effect = null;

        public RandomEffectEffect() : base()
        {
            Random rnd = new Random();
            Array effects = Enum.GetValues(typeof(AllEffects.Effects));
            Effect effect;
            do
            {
                AllEffects.Effects effectType = (AllEffects.Effects)effects.GetValue(rnd.Next(effects.Length));
                effect = AllEffects.InstantiateEffect(effectType);
            } while (effect != null && !effect.IsAllowedToRun());
            this.effect = effect;
        }

        public override void ResetEffectStartTime()
        {
            effect?.ResetEffectStartTime();
        }

        public override long GetEffectLength()
        {
            if(effectStarted) return effect.GetEffectLength();
            return 0;
        }

        public override long GetEffectStartTime()
        {
            if(effectStarted) return effect.GetEffectStartTime();
            return 0;
        }

        public override void StartEffect()
        {
            effectStarted = true;
            effect.StartEffect();
        }

        public override void StopEffect()
        {
            if(effectStarted) effect.StopEffect();
        }

        public override void UpdateEffect()
        {
            if(effectStarted) effect.UpdateEffect();
        }

        public override bool IsAllowedToRun()
        {
            if(effectStarted) return effect.IsAllowedToRun();
            return true;
        }

        public override string GetEffectName()
        {
            if(effectStarted) return effect.GetEffectName();
            return "Random Effect";
        }

        public override bool IsTimedEffect()
        {
            if (effectStarted) return effect.IsTimedEffect();
            return false;
        }


    }
}
