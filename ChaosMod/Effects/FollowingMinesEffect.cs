using ChaosMod.Activator;
using ChaosMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class FollowingMinesEffect : Effect
    {
        private long lastMinePlacement = 0;

        public override string GetEffectName()
        {
            return "Following Mines";
        }

        public override bool IsTimedEffect()
        {
            return true;
        }

        public override long GetEffectLength()
        {
            return 20000;
        }

        public override bool IsAllowedToRun()
        {
            SpawnableMapObject[] spawnableObjects = StartOfRound.Instance.currentLevel.spawnableMapObjects;
            return spawnableObjects.Length > 0;
        }

        public override void UpdateEffect()
        {
            if (lastMinePlacement + 1000 > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                return;
            }
            lastMinePlacement = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            // If you get some unfortunate timing then getting a position from 1 second ago could actually be 0.1 seconds ago or even less
            // By getting a 2 second old position it'll actually be in the range <1, 2]
            PrefabUtil.SpawnLandmine(TimerSystem.GetPositionTracker().GetOldPosition(2));
        }
    }
}
