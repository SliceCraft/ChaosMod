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

        private static void SpawnEnemyOutside(Vector3 enemyPos, int enemyNumber, int enemySpawns = 1)
        {
            // There is no proper handling yet for outside spawning, the game will lag a lot which is a problem
            // Since the SpawnEnemyOustide function is meant for outside spawning eventually it'll now be temporarily used differently
            // When the player is outside (aka when this function is called) spawn the enemy inside of the factory at a random location
            // If/when eventually there is proper code for spawning outside this can easily be modified to have correct behavior

            System.Random rnd = new System.Random();
            int aiLocIndex = rnd.Next(RoundManager.Instance.insideAINodes.Length);
            Vector3 aiLoc = RoundManager.Instance.insideAINodes[aiLocIndex].transform.position;
            SpawnEnemyInside(aiLoc, enemyNumber, enemySpawns);
            return;

            // TODO: Look into a better way of spawning outside
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
