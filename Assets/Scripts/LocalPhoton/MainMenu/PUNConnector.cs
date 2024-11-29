using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MainMenu.UI;
using Data;
using System.IO;

namespace LocalPhoton.MainMenu
{
    /// <summary>
    /// Class in charge of connecting to photon
    /// </summary>
    public class PUNConnector : MonoBehaviourPunCallbacks
    {
        private PlayerData _playerData;
        string dataFilePath = Path.Combine(Application.dataPath, "GameData", "PlayerData.txt");

        public override void OnEnable()
        {
            UIManager.Instance.OnCreateRoomEvent += CreateRoom;
            UIManager.Instance.OnLeaveRoomEvent += LeaveRoom;

            PUNRoomButtonInfo.OnJoinRoomEvent += JoinRoom;

            // Saving Player's Nickname
            if (!File.Exists(dataFilePath))
            {
                _playerData = new PlayerData { playerNickName = "Player" + Random.Range(0, 1000).ToString("0000") };
                string jsonData = JsonUtility.ToJson(_playerData);
                Directory.CreateDirectory(Path.GetDirectoryName(dataFilePath));
                File.WriteAllText(dataFilePath, jsonData);
            }
            else
                Debug.Log("Player information found");

            // Base of PUN OnEnable method
            base.OnEnable();

        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (UIManager.Instance == null)
                return;

            UIManager.Instance.OnCreateRoomEvent -= CreateRoom;
            UIManager.Instance.OnLeaveRoomEvent -= LeaveRoom;

            if (PUNRoomButtonInfo.OnJoinRoomEvent == null)
                return;

            PUNRoomButtonInfo.OnJoinRoomEvent -= JoinRoom;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            UIManager.Instance.PrepareCanvas();
            UIManager.Instance.SetLoadingCanvasGroup(true, "Connecting to Photon Network...");

            // Start connection to PhotonNetwork
            if (!PhotonNetwork.IsConnected)
            {
                Debug.LogFormat($"*** PUNConnector: Connecting to Photon Network...");
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            Debug.LogFormat($"*** PUNConnector: Connected to Photon Network!");
            UIManager.Instance.SetLoadingCanvasGroup(true, "Connected to Photon Network! Joining Lobby...");

            Debug.LogFormat($"*** PUNConnector: Joining Lobby...");
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.LogFormat($"*** PUNConnector: Joined Lobby!");

            UIManager.Instance.SetLoadingCanvasGroup(false);
            UIManager.Instance.SetMainMenuCanvasGroup(true);

            // Set the PlayerNickName in the Network with the local player data
            string loadInfoText = File.ReadAllText(dataFilePath);
            PlayerData loadInfo = JsonUtility.FromJson<PlayerData>(loadInfoText);
            PhotonNetwork.NickName = loadInfo.playerNickName;

            // Verifies if the player is a new player
            if (loadInfo.newPlayer)
            {
                UIManager.Instance.SetUsernameInputCanvasGroup(true);
            }                         
            else                      
            {                         
                UIManager.Instance.SetUsernameInputCanvasGroup(false);
                UIManager.Instance.SetMainMenuCanvasGroup(true);
            }
        }

        /// <summary>
        /// Used to create a room when the player clicks the Create Room button, Action is created in the UIManager
        /// and we subscribe to it in the OnEnable method
        /// </summary>
        /// <param name="roomName"></param>
        private void CreateRoom(string roomName)
        {
            Debug.LogFormat($"*** PUNConnector: Creating Room {roomName}...");
            UIManager.Instance.SetLoadingCanvasGroup(true, "Creating Room...");

            RoomOptions roomOptions = new RoomOptions();
            // Max Players in a room will be 4
            roomOptions.MaxPlayers = 4;
            roomOptions.BroadcastPropsChangeToAll = true;
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

        public override void OnJoinedRoom()
        {
            Debug.LogFormat($"*** PUNConnector: Joined Room [{PhotonNetwork.CurrentRoom.Name}]!");
            UIManager.Instance.SetLoadingCanvasGroup(false);
            UIManager.Instance.SetJoinedRoomCanvasGroup(true, PhotonNetwork.CurrentRoom.Name);

            PlayerCreator.Instance.CreatePlayersInRoom(PhotonNetwork.PlayerList);

            // If the player is the master client, show the start game button   
            if (PhotonNetwork.IsMasterClient)
            {
                UIManager.Instance.ButtonStartGame.SetActive(true);
                Debug.LogFormat($"*** PUNConnector: Im master");
            }
            else
            {
                UIManager.Instance.ButtonStartGame.SetActive(false);
                Debug.LogFormat($"*** PUNConnector: Im master");
            }
        }

        /// <summary>
        /// Used to leave a room when the player clicks the Leave Room button, Action is created in the UIManager
        /// and we subscribe to it in the OnEnable method 
        /// </summary>
        private void LeaveRoom()
        {
            Debug.LogFormat($"*** PUNConnector: Left Room [{PhotonNetwork.CurrentRoom.Name}]!");
            UIManager.Instance.SetJoinedRoomCanvasGroup(false);
            UIManager.Instance.SetLoadingCanvasGroup(true, "Leaving Room...");
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            UIManager.Instance.SetCreateRoomCanvasGroup(false);
            UIManager.Instance.SetLoadingCanvasGroup(false);
            UIManager.Instance.SetMainMenuCanvasGroup(true);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.LogFormat($"*** PUNConnector: Room List Updated!");
            // Populate the room in all clients
            RoomCreator.Instance.PopulateRoomList(roomList);
        }

        /// <summary>
        /// Event to be called when the user clicks the button to join the room
        /// </summary>
        /// <param name="roomInfo"></param>
        private void JoinRoom(RoomInfo roomInfo)
        {
            Debug.LogFormat($"*** PUNConnector: Joining Room [{roomInfo.Name}]...");
            UIManager.Instance.SetLoadingCanvasGroup(true, "Joining Room...");
            UIManager.Instance.SetRoomListCanvasGroup(false);
            PhotonNetwork.JoinRoom(roomInfo.Name);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogFormat($"*** PUNConnector: Join Room Failed! [{message}]");

            StringBuilder sb = new StringBuilder(" Failed to join Room : " + message);

            UIManager.Instance.SetLoadingCanvasGroup(false);
            UIManager.Instance.SetErrorCanvasGroup(true, sb.ToString());
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat($"*** PUNConnector: Player [{newPlayer.NickName}] entered the room!");
            PlayerCreator.Instance.AddPlayer(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat($"*** PUNConnector: Player [{otherPlayer.NickName}] left the room!");
            PlayerCreator.Instance.RemovePlayer(otherPlayer);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Debug.LogFormat($"*** PUNConnector: Player [{newMasterClient.NickName}] is the new Master Client!");
            if (PhotonNetwork.IsMasterClient)
            {
                UIManager.Instance.ButtonStartGame.SetActive(true);
            }
            else
            {
                UIManager.Instance.ButtonStartGame.SetActive(false);
            }
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        public void QuitGame()
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                            Application.Quit(); 
            #endif
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void StartGame()
        {
            // Loads Main Scene that will be index 1

            PhotonNetwork.LoadLevel(2);
        }
    }
}