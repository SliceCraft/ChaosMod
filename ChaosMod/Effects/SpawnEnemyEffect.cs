using ChaosMod.Activator;
using ChaosMod.Patches;
using ChaosMod.Utils;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class SpawnEnemyEffect : Effect
    {
        private Vector3 playerpos;
        private bool blockSpawn = false;
        private bool isInside = false;
        private int enemyNumber;

        public SpawnEnemyEffect() : base(){
            System.Random rnd = new System.Random();
            enemyNumber = rnd.Next(RoundManager.Instance.currentLevel.Enemies.Count);
        }

        public override string GetEffectName()
        {
            return "Spawn " + RoundManager.Instance.currentLevel.Enemies[enemyNumber].enemyType.name;
        }

        public override bool IsTimedEffect()
        {
            return false;
        }

        public override void StartEffect()
        {
            // TODO: Disable this message
            // Enemy outside spawning is WIP, it can spawn but will not handle the AI properly
            // For more info on why this message is still necessary look in RoundManagerPatch -> UpdatePatch -> SpawnEnemy part -> TODO Comment
            if (!IsAllowedToRun())
            {
                HUDManager.Instance.DisplayTip("You're lucky", "If it wasn't for the fact that you were outside you would've been fucked rn");
                blockSpawn = true;
                return;
            }
            SpawnEnemyUtil.SpawnEnemy(TimerSystem.GetPositionTracker().GetOldPosition(ChaosMod.ConfigDelayBeforeSpawn.Value), enemyNumber, 1, GameNetworkManager.Instance.localPlayerController.isInsideFactory);
        }

        public override void StopEffect()
        {
            
        }

        public override bool IsAllowedToRun()
        {
            return GameNetworkManager.Instance.localPlayerController.isInsideFactory;
        }
    }
}
