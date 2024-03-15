using ChaosMod.Activator;
using System.Linq;

namespace ChaosMod.Effects
{
    internal class RemoveHolidingItemsEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Remove Holding Items";
        }

        public override void StartEffect()
        {
            for(int i = 0; i < GameNetworkManager.Instance.localPlayerController.ItemSlots.Count(); i++)
            {
                if (GameNetworkManager.Instance.localPlayerController.ItemSlots[i] == null) continue;
                GameNetworkManager.Instance.localPlayerController.DestroyItemInSlotAndSync(i);
            }
            GameNetworkManager.Instance.localPlayerController.DropAllHeldItemsAndSync();
            GameNetworkManager.Instance.localPlayerController.carryWeight = 1f;
        }
    }
}
