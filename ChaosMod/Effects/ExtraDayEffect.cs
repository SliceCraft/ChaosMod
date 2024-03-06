using ChaosMod.Activator;
using ChaosMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects
{
    // I think this effect should be disabled if the random activator is used for now
    // This effect would appear too often for it to make sense
    // In the future rarity changes should be added and this effect should become more rare
    internal class ExtraDayEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Extra Day";
        }

        public override void StartEffect()
        {
            TimeOfDay.Instance.timeUntilDeadline += TimeOfDay.Instance.totalTime;
            HUDManager.Instance.DisplayDaysLeft((int)Mathf.Floor(TimeOfDay.Instance.timeUntilDeadline / TimeOfDay.Instance.totalTime));
            ReflectionUtil.CallFunction(TimeOfDay.Instance, "SyncGlobalTimeOnNetwork", null);
        }
    }
}
