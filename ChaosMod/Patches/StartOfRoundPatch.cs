using ChaosMod.Activator;
using HarmonyLib;

namespace ChaosMod.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        [HarmonyPatch(nameof(StartOfRound.openingDoorsSequence))]
        [HarmonyPrefix]
        static void openingDoorsSequencePatch()
        {
            if(StartOfRound.Instance.currentLevelID != 3 && GameNetworkManager.Instance.isHostingGame && !GameNetworkManager.Instance.localPlayerController.isPlayerDead) TimerSystem.Enable();
        }

        [HarmonyPatch(nameof(StartOfRound.EndGameServerRpc))]
        [HarmonyPostfix]
        static void EndGameServerRpcPatch()
        {
            TimerSystem.Disable();
        }
    }
}
