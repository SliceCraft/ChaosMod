using ChaosMod.Activator;

namespace ChaosMod.Effects
{
    internal class HealEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Heal";
        }

        public override void StartEffect()
        {
            GameNetworkManager.Instance.localPlayerController.health += 40;
            if (GameNetworkManager.Instance.localPlayerController.health > 100) GameNetworkManager.Instance.localPlayerController.health = 100;
            GameNetworkManager.Instance.localPlayerController.HealServerRpc();
            HUDManager.Instance.UpdateHealthUI(GameNetworkManager.Instance.localPlayerController.health, false);
        }
    }
}
