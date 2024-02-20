using ChaosMod.Activator;
using Unity.Netcode;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class DropHelpfulItemEffect : Effect
    {
        private int itemIndex;

        public DropHelpfulItemEffect() : base()
        {
            System.Random rnd = new System.Random();
            do
            {
                itemIndex = rnd.Next(StartOfRound.Instance.allItemsList.itemsList.Count);
            } while (StartOfRound.Instance.allItemsList.itemsList[itemIndex].isScrap == true);
        }

        public override string GetEffectName()
        {
            return "Drop " + StartOfRound.Instance.allItemsList.itemsList[itemIndex].itemName;
        }

        public override bool IsTimedEffect()
        {
            return false;
        }

        public override void StartEffect()
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(StartOfRound.Instance.allItemsList.itemsList[itemIndex].spawnPrefab, GameNetworkManager.Instance.localPlayerController.thisPlayerBody.position, Quaternion.identity, StartOfRound.Instance.propsContainer);
            gameObject.GetComponent<GrabbableObject>().fallTime = 0f;
            gameObject.GetComponent<NetworkObject>().Spawn(false);
        }
    }
}
