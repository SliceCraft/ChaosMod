using ChaosMod.Activator;
using Unity.Netcode;
using UnityEngine;

namespace ChaosMod.Effects
{
    internal class DropScrapEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Drop Scrap";
        }

        public override bool IsTimedEffect()
        {
            return false;
        }

        public override void StartEffect()
        {
            for(int i = 0; i < 2; i++)
            {
                System.Random rnd = new System.Random();
                int itemIndex;
                do
                {
                    itemIndex = rnd.Next(StartOfRound.Instance.allItemsList.itemsList.Count);
                } while (StartOfRound.Instance.allItemsList.itemsList[itemIndex].isScrap == false);
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(StartOfRound.Instance.allItemsList.itemsList[itemIndex].spawnPrefab, GameNetworkManager.Instance.localPlayerController.thisPlayerBody.position, Quaternion.identity, StartOfRound.Instance.propsContainer);
                gameObject.GetComponent<GrabbableObject>().fallTime = 0f;
                gameObject.GetComponent<GrabbableObject>().scrapValue = 50;
                gameObject.GetComponent<NetworkObject>().Spawn(false);
            }
        }
    }
}
