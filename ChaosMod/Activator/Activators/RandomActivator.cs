using System;

namespace ChaosMod.Activator.Activators
{
    internal class RandomActivator : Activator
    {
        public string getName()
        {
            return "random";
        }

        public void Start()
        {
            // No starting logic needed
        }

        public void Stop()
        {
            // No stopping logic needed
        }

        public Effect ChooseEffect()
        {
            Random rnd = new Random();
            Array effects = Enum.GetValues(typeof(AllEffects.Effects));
            Effect effect;
            do
            {
                AllEffects.Effects effectType = (AllEffects.Effects)effects.GetValue(rnd.Next(effects.Length));
                effect = AllEffects.InstantiateEffect(effectType);
            } while (effect != null && !effect.IsAllowedToRun());
            return effect;
        }
    }
}
