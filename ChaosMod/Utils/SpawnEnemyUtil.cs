using System;
using Unity.Netcode;
using UnityEngine;

namespace ChaosMod.Utils
{
    internal class SpawnEnemyUtil
    {
        public static void SpawnEnemy(Vector3 enemyPos, int? enemyNumber, int enemySpawns = 1, bool spawnInside = true)
        {
            if(!enemyNumber.HasValue)
            {
                System.Random random = new System.Random();
                enemyNumber = random.Next(RoundManager.Instance.currentLevel.Enemies.Count);
            }

            if (spawnInside) SpawnEnemyInside(enemyPos, enemyNumber.Value, enemySpawns);
            else SpawnEnemyOutside(enemyPos, enemyNumber.Value, enemySpawns);
        }

        private static void SpawnEnemyInside(Vector3 enemyPos, int enemyNumber, int enemySpawns = 1)
        {
            for (int i = 0; i < enemySpawns; i++)
            {
                RoundManager.Instance.SpawnEnemyOnServer(enemyPos, 0f, enemyNumber);
            }
        }

        // The outside code is still pretty broken
        private static void SpawnEnemyOutside(Vector3 enemyPos, int enemyNumber, int enemySpawns = 1)
        {
            for (int i = 0; i < enemySpawns; i++)
            {
                try
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(RoundManager.Instance.currentLevel.Enemies[enemyNumber].enemyType.enemyPrefab, enemyPos, Quaternion.Euler(Vector3.zero));
                    gameObject.gameObject.GetComponentInChildren<NetworkObject>().Spawn(true);
                    EnemyAI component = gameObject.GetComponent<EnemyAI>();
                    component.isOutside = true;
                    component.allAINodes = GameObject.FindGameObjectsWithTag("OutsideAINode");
                    if (component.OwnerClientId != GameNetworkManager.Instance.localPlayerController.actualClientId)
                    {
                        ChaosMod.getInstance().logsource.LogInfo("Yup, owner wasn't set correctly, bad.");
                        component.ChangeOwnershipOfEnemy(GameNetworkManager.Instance.localPlayerController.actualClientId);
                    }
                    Transform transform = component.ChooseClosestNodeToPosition(enemyPos, false, 0);
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
    }
}
