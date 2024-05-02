using ChaosMod.Activator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace ChaosMod.Utils
{
    internal class PrefabUtil
    {
        public static GameObject SpawnLandmine(Vector3 position)
        {
            SpawnableMapObject[] spawnableObjects = StartOfRound.Instance.currentLevel.spawnableMapObjects;
            if (spawnableObjects.Length > 0)
            {
                foreach (SpawnableMapObject spawnableObject in spawnableObjects)
                {
                    if (spawnableObject.prefabToSpawn.GetComponentInChildren<Landmine>() != null)
                    {
                        GameObject gameObject = UnityEngine.Object.Instantiate(spawnableObject.prefabToSpawn, position, Quaternion.Euler(Vector3.zero));
                        gameObject.SetActive(true);
                        gameObject.GetComponent<NetworkObject>().Spawn(true);
                        return gameObject;
                    }
                }
            }
            return null;
        }
    }
}
