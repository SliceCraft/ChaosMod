using ChaosMod.Activator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class FakeTeleportEffect : Effect
    {
        private Vector3 oldPosition;
        private bool oldInside;
        private string effectName = "Random Teleport";

        public override string GetEffectName()
        {
            return effectName;
        }

        public override bool IsTimedEffect()
        {
            return true;
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
            oldPosition = GameNetworkManager.Instance.localPlayerController.transform.position;
            oldInside = GameNetworkManager.Instance.localPlayerController.isInsideFactory;
            System.Random rnd = new System.Random();
            int teleportLocIndex = rnd.Next(RoundManager.Instance.insideAINodes.Length);
            Vector3 teleportLoc = RoundManager.Instance.insideAINodes[teleportLocIndex].transform.position;
            GameNetworkManager.Instance.localPlayerController.isInsideFactory = true;
            GameNetworkManager.Instance.localPlayerController.TeleportPlayer(teleportLoc);
        }

        public override void StopEffect()
        {
            GameNetworkManager.Instance.localPlayerController.isInsideFactory = oldInside;
            GameNetworkManager.Instance.localPlayerController.TeleportPlayer(oldPosition);
            effectName = "Fake Teleport";
        }
    }
}
