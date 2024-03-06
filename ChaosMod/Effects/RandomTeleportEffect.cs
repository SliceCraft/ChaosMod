using ChaosMod.Activator;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class RandomTeleportEffect : Effect
    {
        private bool fakeTeleport = false;
        private Vector3 oldPosition;
        private bool oldInside;
        private string effectName = "Random Teleport";

        public RandomTeleportEffect() : base() {
            System.Random random = new System.Random();
            int a = random.Next(100);
            if(a > 80)
            {
                fakeTeleport = true;
            }
        }

        public override string GetEffectName()
        {
            return effectName;
        }

        public override bool IsTimedEffect()
        {
            return fakeTeleport;
        }

        public override long GetEffectLength()
        {
            return 3000;
        }

        public override bool HideEffectTimer()
        {
            return true;
        }

        public override void StartEffect()
        {
            if(fakeTeleport)
            {
                oldPosition = GameNetworkManager.Instance.localPlayerController.transform.position;
                oldInside = GameNetworkManager.Instance.localPlayerController.isInsideFactory;
            }
            System.Random rnd = new System.Random();
            int teleportLocIndex = rnd.Next(RoundManager.Instance.insideAINodes.Length);
            Vector3 teleportLoc = RoundManager.Instance.insideAINodes[teleportLocIndex].transform.position;
            GameNetworkManager.Instance.localPlayerController.isInsideFactory = true;
            GameNetworkManager.Instance.localPlayerController.TeleportPlayer(teleportLoc);
        }

        public override void StopEffect()
        {
            if (fakeTeleport)
            {
                GameNetworkManager.Instance.localPlayerController.isInsideFactory = oldInside;
                GameNetworkManager.Instance.localPlayerController.TeleportPlayer(oldPosition);
                effectName = "Fake Teleport";
            }
        }
    }
}
