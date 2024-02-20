using ChaosMod.Activator.Activators;
using ChaosMod.Activator;
using HarmonyLib;
using System;
using Unity.Netcode;
using UnityEngine;
using BepInEx.Configuration;

namespace ChaosMod.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        private static Vector3 spawnEnemyPos;
        private static bool spawnInside = false;
        private static bool spawnEnemy = false;
        private static int enemyNumber;
        private static int enemySpawns;

        private static bool dropAllItems = false;

        private static bool unlockUnlockable = false;
        private static int unlockUnlockableId = -1;

        private static KeyboardShortcut shortcut1 = new KeyboardShortcut(KeyCode.P, new KeyCode[] { KeyCode.LeftControl });
        private static KeyboardShortcut shortcut2 = new KeyboardShortcut(KeyCode.O, new KeyCode[] { KeyCode.LeftControl });

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void UpdatePatch()
        {
            TimerSystem.Update();
            bool isNotGoingToCompany = StartOfRound.Instance.currentLevelID != 3;
            bool isPlayerNotDead = !GameNetworkManager.Instance.localPlayerController.isPlayerDead;
            bool isNotInShip = !StartOfRound.Instance.inShipPhase;
            if (shortcut1.IsDown() && isNotGoingToCompany && isPlayerNotDead && isNotInShip && GameNetworkManager.Instance.isHostingGame)
            {
                TimerSystem.Disable();
                TimerSystem.Enable();
                HUDManager.Instance.DisplayTip("Chaos Mod", "The mod has been reset");
            }

            if (shortcut2.IsDown())
            {
                TimerSystem.Disable();
                HUDManager.Instance.DisplayTip("Chaos Mod", "The mod has been turned off. Turn on with Ctrl + P");
            }

            // LEGACY CODE: Should be replaced by a util function in the future
            if (spawnEnemy)
            {
                if (spawnInside)
                {
                    for (int i = 0; i < enemySpawns; i++)
                    {
                        RoundManager.Instance.SpawnEnemyOnServer(spawnEnemyPos, 0f, enemyNumber);
                    }
                }
                else
                {
                    for (int i = 0; i < enemySpawns; i++)
                    {
                        try
                        {
                            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(RoundManager.Instance.currentLevel.Enemies[enemyNumber].enemyType.enemyPrefab, spawnEnemyPos, Quaternion.Euler(Vector3.zero));
                            gameObject.gameObject.GetComponentInChildren<NetworkObject>().Spawn(true);
                            EnemyAI component = gameObject.GetComponent<EnemyAI>();
                            component.isOutside = true;
                            component.allAINodes = GameObject.FindGameObjectsWithTag("OutsideAINode");
                            if (component.OwnerClientId != GameNetworkManager.Instance.localPlayerController.actualClientId)
                            {
                                ChaosMod.getInstance().logsource.LogInfo("Yup, owner wasn't set correctly, bad.");
                                component.ChangeOwnershipOfEnemy(GameNetworkManager.Instance.localPlayerController.actualClientId);
                            }
                            Transform transform = component.ChooseClosestNodeToPosition(spawnEnemyPos, false, 0);
                            component.serverPosition = transform.position;
                            component.transform.position = component.serverPosition;
                            component.agent.Warp(component.serverPosition);
                            component.SyncPositionToClients();
                            component.EnableEnemyMesh(!StartOfRound.Instance.hangarDoorsClosed || !GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom, false);
                        }
                        catch (Exception ex)
                        {
                            ChaosMod.getInstance().logsource.LogError(ex);
                        }
                    }
                }
                spawnEnemy = false;
            }

            if (dropAllItems)
            {
                GameNetworkManager.Instance.localPlayerController.DropAllHeldItemsServerRpc();
                dropAllItems = false;
            }

            if (unlockUnlockable)
            {
                StartOfRound.Instance.BuyShipUnlockableServerRpc(unlockUnlockableId, UnityEngine.Object.FindObjectOfType<Terminal>().groupCredits);
                unlockUnlockable = false;
            }

            ChaosMod.getInstance().twitchIRCClient?.Update();

            if(TimerSystem.GetActivator() != null && TimerSystem.GetActivator().getName().Equals("twitch"))
            {
                ((TwitchActivator)TimerSystem.GetActivator()).RefreshEffectText();
            }
        }

        public static void spawnEnemyNextFrame(Vector3 playerpos, bool inside, int? monsterid, int amount = 1)
        {
            spawnEnemy = true;
            spawnEnemyPos = playerpos;
            enemySpawns = amount;
            spawnInside = inside;
            if (monsterid != null)
            {
                enemyNumber = monsterid.Value;
                return;
            }

            System.Random random = new System.Random();
            enemyNumber = random.Next(RoundManager.Instance.currentLevel.Enemies.Count);
        }
    }
}
