using ChaosMod.Activator;
using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Unity.Netcode;
using UnityEngine;

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

        private static Landmine explodeQueued = null;

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
            if(explodeQueued != null)
            {
                ChaosMod.getInstance().logsource.LogInfo("EXPLODE MOTHERFUCKER");
                explodeQueued.ExplodeMineServerRpc();
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
            if(oneHitExplode)
            {
                SpawnableMapObject[] spawnableObjects = StartOfRound.Instance.currentLevel.spawnableMapObjects;
                if (spawnableObjects.Length > 0)
                {
                    foreach (SpawnableMapObject spawnableObject in spawnableObjects)
                    {
                        if (spawnableObject.prefabToSpawn.GetComponentInChildren<Landmine>() != null)
                        {
                            GameObject gameObject = Object.Instantiate(spawnableObject.prefabToSpawn, GameNetworkManager.Instance.localPlayerController.thisPlayerBody.transform.position, Quaternion.Euler(Vector3.zero));
                            gameObject.SetActive(true);
                            gameObject.GetComponent<NetworkObject>().Spawn(true);
                            gameObject.GetComponentInChildren<Landmine>().ExplodeMineServerRpc();
                            break;
                        }
                    }
                }
            }
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
