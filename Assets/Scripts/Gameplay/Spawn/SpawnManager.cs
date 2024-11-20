using LocalPhoton.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Spawn
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;

        private static SpawnManager _instance;
        public static SpawnManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SpawnManager>();
                }
                if (_instance == null)
                {
                    _instance = new GameObject("SpawnManager").AddComponent<SpawnManager>();
                }
                return _instance;
            }
        }

        // TODO: Instead of spawning at random position, each character should spawn at an exclusive spawnpoint, could be given its index
        /// <summary>
        /// Get a random spawn point
        /// </summary>
        /// <returns></returns>
        public Transform GetSpawnPoint(int charIndex)
        {
            return _spawnPoints[charIndex];
        }

        private void Start()
        {
            // Deactivate all spawn points just in case
            foreach (Transform spawnPoint in _spawnPoints)
            {
                spawnPoint.gameObject.SetActive(false);
            }
        }
    }
}