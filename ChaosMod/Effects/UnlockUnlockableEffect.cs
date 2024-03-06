using ChaosMod.Activator;
using System;

namespace ChaosMod.Effects
{
    internal class UnlockUnlockableEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Unlock Ship Upgrade";
        }

        public override void StartEffect()
        {
            bool anynotunlocked = false;
            for(int i = 0; i < StartOfRound.Instance.unlockablesList.unlockables.Count; i++)
            {
                if (!StartOfRound.Instance.unlockablesList.unlockables[i].alreadyUnlocked && !StartOfRound.Instance.unlockablesList.unlockables[i].hasBeenUnlockedByPlayer)
                {
                    anynotunlocked = true;
                }
            }
            if (!anynotunlocked)
            {
                HUDManager.Instance.DisplayTip("Random unlock", "All upgrades have already been unlocked so couldn't unlock any more :(");
                return;
            }

            UnlockableItem unlockable;
            int unlockableId;
            do
            {
                Random rnd = new Random();
                unlockableId = rnd.Next(StartOfRound.Instance.unlockablesList.unlockables.Count);
                unlockable = StartOfRound.Instance.unlockablesList.unlockables[unlockableId];
                ChaosMod.getInstance().logsource.LogInfo(unlockable.unlockableName);
            } while (unlockable.alreadyUnlocked || unlockable.hasBeenUnlockedByPlayer);
            StartOfRound.Instance.BuyShipUnlockableServerRpc(unlockableId, UnityEngine.Object.FindObjectOfType<Terminal>().groupCredits);
        }   

        public override void StopEffect()
        {
            
        }
    }
}
