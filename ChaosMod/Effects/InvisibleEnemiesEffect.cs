using ChaosMod.Activator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class InvisibleEnemiesEffect : Effect
    {
        List<EnemyAI> enemies = new List<EnemyAI>();

        public override string GetEffectName()
        {
            return "Invisible Enemies";
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
            List<EnemyAI> list = UnityEngine.Object.FindObjectsOfType<EnemyAI>().ToList();
            foreach (EnemyAI enemy in list)
            {
                if (!enemy.isEnemyDead)
                {
                    enemies.Add(enemy);
                    for (int i = 0; i < enemy.skinnedMeshRenderers.Length; i++)
                    {
                        enemy.skinnedMeshRenderers[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        public override void StopEffect()
        {
            foreach (EnemyAI enemy in enemies)
            {
                if (!enemy.isEnemyDead)
                {
                    for (int i = 0; i < enemy.skinnedMeshRenderers.Length; i++)
                    {
                        enemy.skinnedMeshRenderers[i].gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
