using ChaosMod.Activator;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChaosMod.Effects
{
    internal class RandomOutfitEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Random Outfit";
        }

        public override bool IsTimedEffect()
        {
            return false;
        }

        public override void StartEffect()
        {
            List<UnlockableSuit> suits = UnityEngine.Object.FindObjectsOfType<UnlockableSuit>().ToList();
            System.Random rand = new System.Random();
            int index = rand.Next(suits.Count);
            suits[index].SwitchSuitToThis(GameNetworkManager.Instance.localPlayerController);
        }
    }
}
