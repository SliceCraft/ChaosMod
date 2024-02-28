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
            // This should maybe use DropAllHeldItemsAndSync
            // I have seen one instance of this effect breaking and that could be fixed by using that function instead
            GameNetworkManager.Instance.localPlayerController.DropAllHeldItemsServerRpc();
        }
    }
}
