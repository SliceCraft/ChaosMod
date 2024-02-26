using ChaosMod.Activator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod.Effects
{
    internal class EnemiesVoteToLeaveEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Enemies vote to leave";
        }

        public override bool IsTimedEffect()
        {
            return false;
        }

        public override void StartEffect()
        {
            TimeOfDay.Instance.VoteShipToLeaveEarly();
        }

        public override bool IsAllowedToRun()
        {
            return TimeOfDay.Instance.currentDayTime >= 540;
        }
    }
}
