using ChaosMod.Activator;

namespace ChaosMod.Effects
{
    internal class DropEverythingEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Drop Everything";
        }

        public override void StartEffect()
        {
            GameNetworkManager.Instance.localPlayerController.DropAllHeldItemsAndSync();
        }
    }
}
