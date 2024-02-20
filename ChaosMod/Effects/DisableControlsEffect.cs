using ChaosMod.Activator;

namespace ChaosMod.Effects
{
    internal class DisableControlsEffect : Effect
    {
        public override string GetEffectName()
        {
            return "Disable Controls";
        }

        public override bool IsTimedEffect()
        {
            return true;
        }

        public override long GetEffectLength()
        {
            return 10000;
        }

        public override void StartEffect()
        {
            GameNetworkManager.Instance.localPlayerController.playerActions.Movement.Disable();
            IngamePlayerSettings.Instance.playerInput.actions.Disable();
        }

        public override void StopEffect()
        {
            GameNetworkManager.Instance.localPlayerController.playerActions.Movement.Enable();
            IngamePlayerSettings.Instance.playerInput.actions.Enable();
        }
    }
}
