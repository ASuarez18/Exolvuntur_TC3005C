using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnObjects : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private Transform[] spawnPoints;
    
    /// <summary>
    /// this function is used to spawn objects in the scene
    /// all the objects are spawned in the scene at least once in the scene
    /// </summary>
    void Start()
    {
        //list of available
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        //loop through all the objects to spawn
        foreach (GameObject obj in objectsToSpawn)
        {
            if(availableSpawnPoints.Count == 0)
            {
                break;
            }

            //choose a random spawn point
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            //instantiate the object through photon network
            PhotonNetwork.Instantiate(obj.name, spawnPoint.position, spawnPoint.rotation);

            //remove the spawn point from the list
            availableSpawnPoints.RemoveAt(randomIndex);
        }

        while (availableSpawnPoints.Count > 0)
        {
            //choose a random spawn point
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            int randomObjectIndex = Random.Range(0, objectsToSpawn.Length);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            //instantiate the object through photon network
            PhotonNetwork.Instantiate(objectsToSpawn[randomObjectIndex].name, spawnPoint.position, spawnPoint.rotation);

            //remove the spawn point from the list
            availableSpawnPoints.RemoveAt(randomIndex);
        }
   
    }

}
