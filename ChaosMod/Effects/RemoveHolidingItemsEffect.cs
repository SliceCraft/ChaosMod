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

        public override bool IsTimedEffect()
        {
            return false;
        }

        public override void StartEffect()
        {
            for(int i = 0; i < GameNetworkManager.Instance.localPlayerController.ItemSlots.Count(); i++)
            {
                if (GameNetworkManager.Instance.localPlayerController.currentItemSlot == i || GameNetworkManager.Instance.localPlayerController.ItemSlots[i] == null) continue;
                GameNetworkManager.Instance.localPlayerController.DestroyItemInSlot(i);
                GameNetworkManager.Instance.localPlayerController.DropAllHeldItemsServerRpc();
            }
        }
    }
}
