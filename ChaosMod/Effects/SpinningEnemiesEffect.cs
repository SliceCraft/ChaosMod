﻿using ChaosMod.Activator;
using ChaosMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class SpinningEnemiesEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Spinning Enemies";
        }

        public override bool IsTimedEffect()
        {
            return true;
        }

        public override long GetEffectLength()
        {
            return 20000;
        }

        public override void UpdateEffect()
        {
            List<EnemyAI> list = UnityEngine.Object.FindObjectsOfType<EnemyAI>().ToList();
            foreach (EnemyAI enemy in list)
            {
                if (!enemy.isEnemyDead)
                {
                    enemy.gameObject.transform.eulerAngles = new Vector3(enemy.gameObject.transform.eulerAngles.x, enemy.gameObject.transform.eulerAngles.y + 50, enemy.gameObject.transform.eulerAngles.z);
                }
            }
        }
    }
}
