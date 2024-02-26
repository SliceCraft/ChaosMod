using ChaosMod.Activator;
using GameNetcodeStuff;
using HarmonyLib;

namespace ChaosMod.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        private static bool infinitSprintEnabled = false;
        private static bool noStaminaEnabled = false;
        private static bool oneHitExplode = false;

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void infiniteSprintPatch(ref float ___sprintMeter)
        {
            if (!(infinitSprintEnabled && noStaminaEnabled)) {
                if(infinitSprintEnabled)
                {
                    ___sprintMeter = 1f;
                }
                else if(noStaminaEnabled)
                {
                    ___sprintMeter = 0f;
                }
            }
        }

        [HarmonyPatch(nameof(PlayerControllerB.KillPlayer))]
        [HarmonyPostfix]
        static void KillPlayerPatch()
        {
            if(GameNetworkManager.Instance.localPlayerController.isPlayerDead) {
                TimerSystem.Disable();
            }
        }

        [HarmonyPatch(nameof(PlayerControllerB.DamagePlayer))]
        [HarmonyPostfix]
        static void DamagePlayerPatch()
        {
            if(oneHitExplode) Landmine.SpawnExplosion(GameNetworkManager.Instance.localPlayerController.thisPlayerBody.position, true, 100, 100);
        }

        public static void setInfiniteSprint(bool set)
        {
            infinitSprintEnabled = set;
        }

        public static void SetNoStamina(bool set)
        {
            noStaminaEnabled = set;
        }

        public static void setOneHitExplode(bool set)
        {
            oneHitExplode = set;
        }
    }
}
