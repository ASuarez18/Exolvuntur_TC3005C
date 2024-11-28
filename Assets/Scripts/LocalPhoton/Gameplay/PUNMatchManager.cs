using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LocalPhoton.Gameplay
{
    public class PUNMatchManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        private int _localPlayerIndex;

        public PUNEventCodes.EventCodes eventCode;
        public PUNEventCodes.GameStates gameState;

        private readonly float _waitTimeAfterEndingMatch = 2f;

        //private const float MATCH_TIME = 380f;
        //private float _currentMatchTime;
        //[SerializeField] private HUDTimer _hudTimer;

        [SerializeField] private bool _keepRunning = true;
        //private float _timerSyncInterval;

        // Used to keep track of the players in the match
        public List<PUNPlayerInfo> playersInGame = new();

        public int TotalObjects { get; set; }
        public int PlayersDead { get; set; }
        private bool _winners;

        //public HUDPlayerScore _hudPlayerScore;

        // Singletoning the PUNMatchManager
        private static PUNMatchManager _instance;
        public static PUNMatchManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PUNMatchManager>();
                }
                return _instance;
            }
        }

        private const int ITEMS_TO_WIN = 4;

        public override void OnEnable()
        {
            base.OnEnable();

            // When an eventcallback happens, we want to listen to it on this script
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            // We remove the callback target when the script is disabled, this way we make sure there's no one listening
            // to the vents when the script is not active.
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void Start()
        {
            // If we are not connected to the Photon network, end the match
            if (!PhotonNetwork.IsConnected) SceneManager.LoadScene(0);

            else  // If we are connected, send the player info
            {
                NewPlayerSent(PhotonNetwork.NickName);
                gameState = PUNEventCodes.GameStates.Waiting;

                //_currentMatchTime = MATCH_TIME;
                //_hudTimer.UpdateTime(_currentMatchTime);
            }
        }

        //private void Update()
        //{
        //    if (PhotonNetwork.IsMasterClient) return;

        //    _timerSyncInterval -= Time.deltaTime;

        //    if (_timerSyncInterval <= 0)
        //    {
        //        _timerSyncInterval = 1f;
        //        UpdateTimerSyncSend();
        //    }

        //    if (_currentMatchTime > 0 && gameState == PUNEventCodes.GameStates.Playing)
        //    {
        //        _currentMatchTime -= Time.deltaTime;
        //        //_hudTimer.UpdateTime(_currentMatchTime);
        //    }
        //    else if (_currentMatchTime <= 0 && gameState == PUNEventCodes.GameStates.Playing)
        //    {
        //        gameState = PUNEventCodes.GameStates.Ending;
        //        _hudTimer.UpdateTime(0);

        //        // Notify players the game has ended
        //        UpdateTimerSyncSend();
        //    }
        //}

        /// <summary>
        /// Metod to be called when the match starts
        /// </summary>
        public void StartMatch()
        {
            Debug.LogFormat($"*** PUNMatchManager: Match starting...");
        }

        /// <summary>
        /// Method to be called when the match ends
        /// </summary>
        public void EndMatch()
        {
            // Stop all coroutines in the PUNPlayerSpawner
            PUNPlayerSpawner.Instance.StopAllCoroutines();

            PhotonNetwork.AutomaticallySyncScene = false;

            // Destroy all player objects in the network
            if (PhotonNetwork.IsMasterClient) PhotonNetwork.DestroyAll();

            // Show the dead screen with the game stats
            //HUDDeadScreen HDS = FindObjectOfType<HUDDeadScreen>();
            //HDS.SetActiveDeadScreen(true, "END OF MATCH");

            // Free the mouse cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            StartCoroutine(EndOfGameCor());
        }

        public void OnEvent(EventData photonEvent)
        {
            /*
             * Events codes can only exist between [0 - 200], codes greater than 200 are reserved for Photon
             * https://doc-api.photonengine.com/en/pun/current/class_photon_1_1_realtime_1_1_event_code.html
             */

            if (photonEvent.Code >= 200) return;

            // We cast the event code to the PUNEventCodes.EventCodes enum
            eventCode = (PUNEventCodes.EventCodes)photonEvent.Code;

            // We cast the data to an object array
            object[] data = (object[])photonEvent.CustomData;

            // Depending on the event code, we call the corresponding method
            switch (eventCode)
            {
                case PUNEventCodes.EventCodes.NewPlayer:
                    NewPlayerReceived(data);
                    break;
                case PUNEventCodes.EventCodes.ListPlayers:
                    ListPlayersReceived(data);
                    break;
                case PUNEventCodes.EventCodes.UpdateStat:
                    UpdateStatReceived(data);
                    break;
                case PUNEventCodes.EventCodes.UpdateGameState:
                    UpdateGameStateReceived(data);
                    break;
                    //case PUNEventCodes.EventCodes.NextMatch:
                    //    UpdateNextMatchReceived();
                    //    break;
                    //case PUNEventCodes.EventCodes.TimerSync:
                    //    UpdateTimerSyncReceived(data);
                    //    break;
            }
        }

        #region PUN EVENTS REGION

        /// <summary>
        /// Sends a new player to the other players in the match
        /// </summary>
        /// <param name="userName"></param>
        public void NewPlayerSent(string userName)
        {
            // Create an object array to send the user info with 4 slots since my PUNPlayerInfo class has 4 fields.
            // Please, be aware that the order of the fields in the array must match the order of the fields in the PUNPlayerInfo class.
            object[] data = new object[4];
            data[0] = userName;
            data[1] = PhotonNetwork.LocalPlayer.ActorNumber;
            data[2] = 0; // Kills

            // Sent the event to the masterclient
            PhotonNetwork.RaiseEvent(
                (byte)PUNEventCodes.EventCodes.NewPlayer,
                data,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                new SendOptions { Reliability = true }
            );
        }

        /// <summary>
        /// Receives a new player
        /// </summary>
        /// <param name="dataReceived"></param>
        public void NewPlayerReceived(object[] dataReceived)
        {
            Debug.LogFormat($"*** PUNMatchManager: New player received");

            // The process of converting from an object array to a value type is called unboxing.
            // Perform the unboxing process to get the values from the object array.
            // Please, be aware that the order of the fields sent array must match the order of the fields in the PUNPlayerInfo class.
            PUNPlayerInfo newPlayer = new(
                (string)dataReceived[0],
                (int)dataReceived[1],
                (int)dataReceived[2]
            );

            playersInGame.Add(newPlayer);

            ListPlayersSent();
        }

        /// <summary>
        /// Sends a list of players to the other players in the match
        /// </summary>
        public void ListPlayersSent()
        {
            object[] players = new object[playersInGame.Count];

            // We iterate through the players in the match and create an object array for each one
            for (int i = 0; i < playersInGame.Count; i++)
            {
                object[] playerData = new object[4];

                playerData[0] = playersInGame[i].name;
                playerData[1] = playersInGame[i].actor;
                playerData[2] = playersInGame[i].kills;

                // We add the player data to the players array. This will create an array of arrays.
                players[i] = playerData;
            }

            // Sent the event to all the players in the room
            PhotonNetwork.RaiseEvent(
                (byte)PUNEventCodes.EventCodes.ListPlayers,
                players,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
            );
        }

        /// <summary>
        /// Reeceives a list of players from another player in the match
        /// </summary>
        /// <param name="dataReceived"></param>
        public void ListPlayersReceived(object[] dataReceived)
        {
            Debug.LogFormat($"*** PUNMatchManager: List players received");
            // We clear the list of players to avoid duplicates
            playersInGame.Clear();

            // Get the list of players from the data received
            for (int i = 0; i < dataReceived.Length; i++)
            {
                object[] playerData = (object[])dataReceived[i];

                PUNPlayerInfo newPlayer = new(
                    (string)playerData[0],
                    (int)playerData[1],
                    (int)playerData[2]
                );

                playersInGame.Add(newPlayer);

                // If the player is the local player, we save the index in the list
                if (PhotonNetwork.LocalPlayer.ActorNumber == newPlayer.actor)
                {
                    _localPlayerIndex = i;
                }
            }

            // If the game state is waiting and all the players are in the match, we start the match
            if (playersInGame.Count == PhotonNetwork.CurrentRoom.PlayerCount && gameState == PUNEventCodes.GameStates.Waiting)
            {
                gameState = PUNEventCodes.GameStates.Playing;

                // Sents the game state to all the players in the match to keep them in sync
                if (PhotonNetwork.IsMasterClient) UpdateGameStateSend();

                StartMatch();
            }
        }

        /// <summary>
        /// Changes a stat of a player and sends it to the other players in the match
        /// </summary>
        public void UpdateStatSent(int sendingActor, PUNEventCodes.PlayerStats statToUpdate, int value)
        {
            Debug.LogFormat($"*** PUNMatchManager: change stat sent by {sendingActor} - Stat {statToUpdate} - {value}");

            // Box the values to send them in an object array
            object[] data = new object[3];
            data[0] = sendingActor;
            data[1] = statToUpdate;
            data[2] = value;

            PhotonNetwork.RaiseEvent(
                (byte)PUNEventCodes.EventCodes.UpdateStat,
                data,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                new SendOptions { Reliability = true }
            );
        }

        /// <summary>
        /// Receives a change of stat from another player in the match
        /// </summary>
        /// <param name="dataReceived"></param>
        public void UpdateStatReceived(object[] dataReceived)
        {
            // Unbox the values to get the data 
            int sendingActor = (int)dataReceived[0];
            var statToUpdate = (PUNEventCodes.PlayerStats)(byte)dataReceived[1];
            int value = (int)dataReceived[2];

            switch (statToUpdate)
            {
                case PUNEventCodes.PlayerStats.TotalObjects:
                    TotalObjects += value;
                    break;
                case PUNEventCodes.PlayerStats.Kills:
                    PlayersDead += value;
                    break;
            }

            /*
            for (int i = 0; i < playersInGame.Count; i++)
            {
                // We find the player that sent the stat change and update their stat accordingly
                if (playersInGame[i].actor == sendingActor)
                {
                    switch (statToUpdate)
                    {
                        case PUNEventCodes.PlayerStats.Kills: // Kills
                            playersInGame[i].kills += value;
                            break;
                            //case PUNEventCodes.PlayerStats.Deaths: // Deaths
                            //    playersInGame[i].deaths += value;
                            //    break;
                    }

                    //Debug.LogFormat($"*** PUNMatchManager: Player {sendingActor} - Kills {playersInGame[i].kills} - Deaths {playersInGame[i].deaths}");

                    if (sendingActor == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        //_hudPlayerScore.SetPlayerScore(playersInGame[i].kills, playersInGame[i].deaths);
                    }

                    break;
                }
            }*/

            ScoreCheck();
        }

        /// <summary>
        /// Used to send the game state across the network on the master client
        /// </summary>
        public void UpdateGameStateSend()
        {
            object[] data = new object[1];
            data[0] = gameState;

            PhotonNetwork.RaiseEvent(
                (byte)PUNEventCodes.EventCodes.UpdateGameState,
                data,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
            );
        }

        /// <summary>
        /// Used to receive the game state across the network
        /// </summary>
        /// <param name="dataReceived"></param>
        public void UpdateGameStateReceived(object[] dataReceived)
        {
            gameState = (PUNEventCodes.GameStates)Enum.ToObject(typeof(PUNEventCodes.GameStates), dataReceived[0]);

            Debug.LogFormat($"*** PUNMatchManager: Game State Received - {gameState}");

            if (gameState == PUNEventCodes.GameStates.Ending) EndMatch();
            //ScoreCheck();
        }

        //public void UpdateNextMatchSend()
        //{
        //    PhotonNetwork.RaiseEvent(
        //        (byte)PUNEventCodes.EventCodes.NextMatch,
        //        null,
        //        new RaiseEventOptions { Receivers = ReceiverGroup.All },
        //        new SendOptions { Reliability = true }
        //    );
        //}

        //public void UpdateNextMatchReceived()
        //{
        //    gameState = PUNEventCodes.GameStates.Playing;

        //    // Free the mouse cursor
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;

        //    // Clean player scores
        //    foreach (var player in playersInGame)
        //    {
        //        player.kills = 0;
        //        //player.deaths = 0;
        //    }

        //    // Hides the dead screen, also, player updates in the UI are done in the HUDDeadScreen script
        //    //HUDDeadScreen HDS = FindObjectOfType<HUDDeadScreen>();
        //    //HDS.SetActiveDeadScreen(false, "");

        //    // Respawn the player across the network
        //    PUNPlayerSpawner.Instance.SpawnPlayer();

        //    // Reset the match time
        //    //_currentMatchTime = MATCH_TIME;
        //    //_hudTimer.UpdateTime(_currentMatchTime);
        //}

        /// <summary>
        /// Sends the current timer in the match across the network
        /// </summary>
        //public void UpdateTimerSyncSend()
        //{
        //    object[] data = new object[1];
        //    data[0] = (int)_currentMatchTime;

        //    PhotonNetwork.RaiseEvent(
        //        (byte)PUNEventCodes.EventCodes.TimerSync,
        //        data,
        //        new RaiseEventOptions { Receivers = ReceiverGroup.All },
        //        new SendOptions { Reliability = true }
        //    );
        //}

        //public void UpdateTimerSyncReceived(object[] dataReceived)
        //{
        //    _currentMatchTime = (int)dataReceived[0];
        //    _hudTimer.UpdateTime(_currentMatchTime);
        //}

        #endregion

        public override void OnLeftRoom()
        {
            Debug.LogWarning("Left room");
            base.OnLeftRoom();
            SceneManager.LoadScene(0);
            // TODO: End match for evryone
        }

        private IEnumerator EndOfGameCor()
        {
            yield return new WaitForSeconds(_waitTimeAfterEndingMatch);
            Debug.LogWarning("End of game coroutine");
            if (!_keepRunning)
            {
                Debug.LogWarning("End of wainting for next match");
                PhotonNetwork.AutomaticallySyncScene = false;
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                 Debug.LogWarning("End of wainting for next match");
                PhotonNetwork.AutomaticallySyncScene = false;
                PhotonNetwork.LeaveRoom();
                // TODO: Logic for aftermath of match

                // Only the master client can restart the match
                //if (PhotonNetwork.IsMasterClient)
                //{
                //    if (PUNConnector.ChangeMapBetweenRounds)
                //    {
                //        // Gets the sceneCount from the build settings
                //        int sceneCount = SceneManager.sceneCountInBuildSettings;
                //        // Select a random scene to load
                //        int selectedScene = UnityEngine.Random.Range(1, sceneCount);

                //        // If the selected scene is the current scene, we restart the match
                //        if (selectedScene == SceneManager.GetActiveScene().buildIndex)
                //        {
                //            UpdateNextMatchSend();
                //            yield break;
                //        }

                //        // Otherwise, we load the selected scene
                //        PhotonNetwork.LoadLevel(selectedScene);
                //    }
                //    else
                //    {
                //        UpdateNextMatchSend();
                //    }
                //}
            }
        }

        private void ScoreCheck()
        {
            Debug.LogWarning("Numero total de objtetos: " + TotalObjects);
            if (TotalObjects >= 4)
            {
                gameState = PUNEventCodes.GameStates.Ending;
                _winners = true;
            }
            if (PlayersDead >= 4)
            {
                gameState = PUNEventCodes.GameStates.Ending;
                _winners = false;
            }

            if (_winners)
            {
                // Notify players the game has ended
                if (PhotonNetwork.IsMasterClient)
                {
                    gameState = PUNEventCodes.GameStates.Ending;
                    Debug.LogFormat($"*** PUNMatchManager: Score check STATE - {gameState}");
                    UpdateGameStateSend();
                }
            }
        }
    }
}