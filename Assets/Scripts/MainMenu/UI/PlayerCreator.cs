using UnityEngine;

namespace MainMenu.UI
{
    /// <summary>
    ///  Class in charge of creating the player for the player inst in the room
    /// </summary>
    public class PlayerCreator : MonoBehaviour
    {
        [SerializeField] private PlayerInfo _playerPrefab;
        [SerializeField] private RectTransform _playerParent;

        // Singletoning the PlayerCreator

        private static PlayerCreator _instance;
        public static PlayerCreator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PlayerCreator>();
                }

                if (_instance == null)
                {
                    // Create a new GameObject and add the PlayerCreator to it
                    GameObject go = new GameObject("PlayerCreator");
                    go.AddComponent<PlayerCreator>();
                    _instance = go.GetComponent<PlayerCreator>();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Creates the players in the room
        /// </summary>
        /// <param name="playerList"></param>
        public void CreatePlayersInRoom(Photon.Realtime.Player[] playerList)
        {
            // Clean the player parent from previous players
            foreach (Transform child in _playerParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var player in playerList)
            {
                // Instantiate the player prefab
                PlayerInfo p = Instantiate(_playerPrefab, _playerParent);

                p.transform.localPosition = Vector3.zero;
                p.transform.localRotation = Quaternion.identity;
                p.transform.localScale = Vector3.one;

                p.SetPlayerInfo(player.NickName);
            }
        }

        /// <summary>
        /// Adds a new player to the room
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(Photon.Realtime.Player player)
        {
            PlayerInfo p = Instantiate(_playerPrefab, _playerParent);

            p.transform.localPosition = Vector3.zero;
            p.transform.localRotation = Quaternion.identity;
            p.transform.localScale = Vector3.one;

            p.SetPlayerInfo(player.NickName);
        }

        /// <summary>
        /// Removes a player form the room
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public void RemovePlayer(Photon.Realtime.Player player)
        {
            foreach (Transform child in _playerParent)
            {
                if (child.GetComponent<PlayerInfo>().Name == player.NickName)
                {
                    Destroy(child.gameObject);
                    return;
                }
            }
        }
    }
}