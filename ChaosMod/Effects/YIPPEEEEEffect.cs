using ChaosMod.Activator;
using ChaosMod.Utils;

namespace ChaosMod.Effects
{
    internal class YIPPEEEEEffect : Effect
    {
        public override string GetEffectName()
        {
            return "YIPPEEEE";
        }

        public override bool IsTimedEffect()
        {
            return false;
        }

        public override void StartEffect()
        {
            if (!GameNetworkManager.Instance.localPlayerController.isInsideFactory)
            {
                HUDManager.Instance.DisplayTip("You're lucky", "If it wasn't for the fact that you were outside you would've been fucked rn");
                return;
            }
            int? yippeeeid = null;
            for (int i = 0; i < RoundManager.Instance.currentLevel.Enemies.Count; i++)
            {
                SpawnableEnemyWithRarity enemy = RoundManager.Instance.currentLevel.Enemies[i];
                if(enemy.enemyType.enemyName.Equals("Hoarding bug"))
                {
                    yippeeeid = i;
                }
            }
            if (yippeeeid.HasValue) { SpawnEnemyUtil.SpawnEnemy(GameNetworkManager.Instance.localPlayerController.thisPlayerBody.position, yippeeeid, 10, GameNetworkManager.Instance.localPlayerController.isInsideFactory); }
        }
    }
}
