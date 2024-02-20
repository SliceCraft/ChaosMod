using ChaosMod.Activator;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class RandomTeleportEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Random Teleport";
        }

        public override bool IsTimedEffect()
        {
            return false;
        }

        public override void StartEffect()
        {
            System.Random rnd = new System.Random();
            int teleportLocIndex = rnd.Next(RoundManager.Instance.insideAINodes.Length);
            Vector3 teleportLoc = RoundManager.Instance.insideAINodes[teleportLocIndex].transform.position;
            GameNetworkManager.Instance.localPlayerController.isInsideFactory = true;
            GameNetworkManager.Instance.localPlayerController.TeleportPlayer(teleportLoc);
        }
    }
}
