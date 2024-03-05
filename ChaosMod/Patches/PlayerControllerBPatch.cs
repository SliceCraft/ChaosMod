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
        private static bool singleUseFallImmunity = false;
        private static bool isInvincible = false;

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

        [HarmonyPatch("PlayerHitGroundEffects")]
        [HarmonyPrefix]
        static void PlayerHitGroundEffectsPatch()
        {
            if (singleUseFallImmunity)
            {
                GameNetworkManager.Instance.localPlayerController.ResetFallGravity();
                singleUseFallImmunity = false;
            }
        }

        [HarmonyPatch(nameof(PlayerControllerB.AllowPlayerDeath))]
        [HarmonyPrefix]
        static bool AllowPlayerDeathPatch(ref bool __result)
        {
            ChaosMod.getInstance().logsource.LogInfo("Allow player death patch");
            if (isInvincible)
            {
                ChaosMod.getInstance().logsource.LogInfo("Is invincible and should return false");
                __result = false;
                return false;
            }
            ChaosMod.getInstance().logsource.LogInfo("Is not invincible, oh no");
            return true;
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

        public static void SetSingleUseFallImmunity(bool set)
        {
            singleUseFallImmunity = set;
        }

        public static void SetInvincible(bool set)
        {
            isInvincible = set;
        }
    }
}
