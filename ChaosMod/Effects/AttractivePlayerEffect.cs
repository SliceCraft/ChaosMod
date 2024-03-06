using ChaosMod.Activator;
using System.Collections.Generic;
using System.Linq;

namespace ChaosMod.Effects
{
    internal class AttractivePlayerEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Attractive Player";
        }

        public override void StartEffect()
        {
            bool isInside = GameNetworkManager.Instance.localPlayerController.isInsideFactory;
            List<EnemyAI> list = UnityEngine.Object.FindObjectsOfType<EnemyAI>().ToList();
            foreach (EnemyAI enemy in list)
            {
                if(((isInside && !enemy.isOutside) || (!isInside && enemy.isOutside)) && !enemy.isEnemyDead)
                {
                    enemy.SetMovingTowardsTargetPlayer(GameNetworkManager.Instance.localPlayerController);
                    enemy.SetDestinationToPosition(GameNetworkManager.Instance.localPlayerController.transform.position, true);
                }
            }
        }
    }
}
