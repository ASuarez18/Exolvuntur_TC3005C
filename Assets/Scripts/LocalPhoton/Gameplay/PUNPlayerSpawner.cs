using Gameplay.Spawn;
using LocalPhoton.MainMenu;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace LocalPhoton.Gameplay
{
    /// <summary>
    /// Class in charge of spawning the player in the scene via Photon
    /// </summary>
    public class PUNPlayerSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _playerPrefabs;

        private GameObject _localPlayer;
        public GameObject LocalPlayer => _localPlayer;

        // Singletoning the PUNPlayerSpawner
        private static PUNPlayerSpawner _instance;
        public static PUNPlayerSpawner Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PUNPlayerSpawner>();
                }
                return _instance;
            }
        }

        private void Start()
        {
            // Check if we are connected to the Photon Network
            if (PhotonNetwork.IsConnected) SpawnPlayer();
        }

        /// <summary>
        /// Insantiates the player in the scene across the network for all players to see and interact with
        /// </summary>
        public void SpawnPlayer()
        {
            /*
             * Remember, position and rotation are synced across the network using PhotonView + PhotonTransformView components
             * inside the playter prefab, so we don't need to sync the player's position and rotation manually.
             */

            if (_localPlayer != null)
            {
                Debug.LogFormat($"*** PUNPlayerSpawner: Player already spawned");
                return;
            }

            Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint(PUNCharacterSelector.SelectedCharacterIndex);
            //Debug.LogError($"Char index {PUNCharacterSelector.SelectedCharacterIndex}");
            //Debug.LogError($"List size{_playerPrefabs.Count }");
            //// Spawn the player in the scene using Photon
            _localPlayer = PhotonNetwork.Instantiate(_playerPrefabs[PUNCharacterSelector.SelectedCharacterIndex].name, spawnPoint.position, spawnPoint.rotation);
            _localPlayer.SetActive(true);
            Debug.LogWarning($"*** PUNPlayerSpawn: Spawning the player...");
        }

        /// <summary>
        /// Method to spawn the coroutine that will spawn the player after a delay
        /// </summary>
        //public void SpawnPlayerDelayed()
        //{
        //    StartCoroutine(SpawnPlayerCorr(3f));
        //}

        /// <summary>
        /// Spawn the player after a delay
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        //private IEnumerator SpawnPlayerCorr(float delay)
        //{
        //    Debug.LogFormat($"*** Spawning the player in {delay} seconds...");
        //    yield return new WaitForSeconds(delay);
        //    Debug.LogFormat($"*** Spawning the player now...");
        //    SpawnPlayer();
        //}
    }
}