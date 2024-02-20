using ChaosMod.Activator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChaosMod.Effects
{
    // This effect doesn't work that well, might have to investigate more into this or remove it
    internal class LockAllDoorsEffect : Effect
    {
        private List<DoorLock> unlockDoors = new List<DoorLock>();
        private List<DoorLock> openDoors = new List<DoorLock>();
        public override string GetEffectName()
        {
            return "Lock All Doors";
        }

        public override bool IsTimedEffect()
        {
            return true;
        }

        public override long GetEffectLength()
        {
            return 10000;
        }

        public override void StartEffect()
        {
            Random rand = new Random();
            List<DoorLock> list = UnityEngine.Object.FindObjectsOfType<DoorLock>().ToList<DoorLock>();
            for (int i = 0; i < list.Count; i++)
            {
                DoorLock doorLock = list[i];
                if (!doorLock.isLocked)
                {
                    unlockDoors.Add(doorLock);
                    doorLock.LockDoor();
                    if (doorLock.gameObject.GetComponent<AnimatedObjectTrigger>().boolValue)
                    {
                        openDoors.Add(doorLock);
                        doorLock.OpenOrCloseDoor(GameNetworkManager.Instance.localPlayerController);
                    }
                }
            }
        }

        public override void StopEffect()
        {
            for (int i = 0; i < unlockDoors.Count; i++)
            {
                DoorLock doorLock = unlockDoors[i];
                doorLock.UnlockDoor();
            }
            unlockDoors = new List<DoorLock>();

            for (int i = 0; i < openDoors.Count; i++)
            {
                DoorLock doorLock = openDoors[i];
                if (!doorLock.gameObject.GetComponent<AnimatedObjectTrigger>().boolValue)
                {
                    doorLock.OpenOrCloseDoor(GameNetworkManager.Instance.localPlayerController);
                }
            }
            openDoors = new List<DoorLock>();
        }
    }
}
