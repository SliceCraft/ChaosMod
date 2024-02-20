using ChaosMod.Activator;

namespace ChaosMod.Effects
{
    internal class DropEverythingEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Drop Everything";
        }

        public override bool IsTimedEffect()
        {
            return false;
        }

        public override void StartEffect()
        {
            GameNetworkManager.Instance.localPlayerController.DropAllHeldItemsServerRpc();
        }
    }
}
