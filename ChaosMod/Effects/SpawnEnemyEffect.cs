using ChaosMod.Activator;
using ChaosMod.Patches;
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
            // TODO: Use util
            RoundManager.Instance.SpawnEnemyOnServer(TimerSystem.GetPositionTracker().GetOldPosition(ChaosMod.ConfigDelayBeforeSpawn.Value), 0, enemyNumber);
        }

        public override void StopEffect()
        {
            
        }

        public override bool IsAllowedToRun()
        {
            return GameNetworkManager.Instance.localPlayerController.isInsideFactory;
        }

        private async void SpawnEnemy()
        {
            ChaosMod.getInstance().logsource.LogInfo("Waiting for enemy spawn");
            await Task.Delay(ChaosMod.ConfigDelayBeforeSpawn.Value * 1000);
            ChaosMod.getInstance().logsource.LogInfo("Spawning enemy if not blocked");
            if (blockSpawn) return;
            RoundManagerPatch.spawnEnemyNextFrame(playerpos, isInside, null);
            ChaosMod.getInstance().logsource.LogInfo("Spawning enemy next frame");
        }
    }
}
