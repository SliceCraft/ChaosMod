using ChaosMod.Activator;
using ChaosMod.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects
{
    internal class NoStaminaEffect : Effect
    {
        public override string GetEffectName()
        {
            return "No Stamina";
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
            PlayerControllerBPatch.SetNoStamina(true);
        }

        public override void StopEffect()
        {
            PlayerControllerBPatch.SetNoStamina(false);
        }
    }
}
