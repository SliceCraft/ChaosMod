using System;

namespace ChaosMod.Activator
{
    internal abstract class Effect
    {
        private long effectStartTime = 0;

        public Effect()
        {
            ResetEffectStartTime();
        }

        public void ResetEffectStartTime()
        {
            effectStartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public virtual long GetEffectLength()
        {
            return 0;
        }
        public virtual long GetEffectStartTime()
        {
            return effectStartTime;
        }

        public virtual void StartEffect()
        {

        }
        public virtual void StopEffect()
        {

        }
        public virtual void UpdateEffect()
        {

        }
        public virtual bool IsAllowedToRun()
        {
            return true;
        }

        public abstract string GetEffectName();
        public abstract bool IsTimedEffect();
    }
}
