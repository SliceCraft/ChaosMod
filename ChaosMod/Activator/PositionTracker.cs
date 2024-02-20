using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosMod.Activator
{
    internal class PositionTracker
    {
        private long lastTimeTracked = 0;
        private List<Vector3> positionHistory = new List<Vector3>();
        private List<bool> insideHistory = new List<bool>();

        public void Update()
        {
            if(lastTimeTracked + 1000 < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                lastTimeTracked = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                PushHistoryDown();
                positionHistory.Insert(0, GameNetworkManager.Instance.localPlayerController.thisPlayerBody.transform.position);
                insideHistory.Insert(0, GameNetworkManager.Instance.localPlayerController.isInsideFactory);
            }
        }

        public Vector3 GetOldPosition(int secondsAgo)
        {
            if (secondsAgo > 10) secondsAgo = 10;
            if (secondsAgo < 1) secondsAgo = 1;
            return positionHistory[secondsAgo - 1];
        }

        public bool GetOldInside(int secondsAgo)
        {
            if (secondsAgo > 10) secondsAgo = 10;
            if (secondsAgo < 1) secondsAgo = 1;
            return insideHistory[secondsAgo - 1];
        }

        private void PushHistoryDown()
        {
            for (int i = positionHistory.Count > 10 ? 9 : positionHistory.Count - 2; i >= 0; i--)
            {
                positionHistory[i + 1] = positionHistory[i];
                insideHistory[i + 1] = insideHistory[i];
            }
        }

        public PositionTracker()
        {
            lastTimeTracked = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
