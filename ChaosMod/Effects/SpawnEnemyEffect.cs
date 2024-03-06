using ChaosMod.Activator;
using ChaosMod.Utils;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class SpawnEnemyEffect : Effect
    {
        private readonly int enemyNumber;

        public SpawnEnemyEffect() : base(){
            System.Random rnd = new System.Random();
            enemyNumber = rnd.Next(RoundManager.Instance.currentLevel.Enemies.Count);
        }

        public override string GetEffectName()
        {
            return "Spawn " + RoundManager.Instance.currentLevel.Enemies[enemyNumber].enemyType.name;
        }

        public override void StartEffect()
        {
            SpawnEnemyUtil.SpawnEnemy(TimerSystem.GetPositionTracker().GetOldPosition(ChaosMod.ConfigDelayBeforeSpawn.Value), enemyNumber, 1, GameNetworkManager.Instance.localPlayerController.isInsideFactory);
        }

        public override void StopEffect()
        {
            
        }
    }
}
