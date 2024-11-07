using Data;
using Photon.Pun;
using LocalPhoton.MainMenu;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace MainMenu.UI
{
    /// <summary>
    /// Class in charge of the main menu UI
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        // Game Title Canvas
        //[SerializeField] private CanvasGroup _gameTittleCanvasGroup;

        // Main Menu Canvas
        [SerializeField, Space] private CanvasGroup _mainMenuCanvasGroup;

        // Create Room Canvas
        [SerializeField, Space] private CanvasGroup _createRoomCanvasGroup;
        [SerializeField] private TMP_InputField _createdRoomNameInputField;

        // Joined room Canvas
        [SerializeField, Space] private CanvasGroup _joinedRoomCanvasGroup;
        [SerializeField] private TMP_Text _joinedRoomName;

        // Room List canvas
        [SerializeField, Space] private CanvasGroup _roomListCanvasGroup;

        // Loading canvas
        [SerializeField, Space] private CanvasGroup _loadingCanvasGroup;
        //[SerializeField] private TMP_Text _loadingText;

        // Error Canvas 
        [SerializeField, Space] private CanvasGroup _errorCanvasGroup;
        [SerializeField] private TMP_Text _errorText;

        // Username Input Canvas
        [SerializeField, Space] private CanvasGroup _usernameInputCanvasGroup;
        [SerializeField] private TMP_InputField _usernameInputField;
        //private PlayerData _playerData;

        // Options Canvas
        [SerializeField, Space] private CanvasGroup _optionsCanvasGroup;
        //[SerializeField] private Slider _volumeSlider;
        //[SerializeField] private Slider _brightnessSlider;
        //private List<int> screenResolutions;

        // Start Game Button 
        [SerializeField, Space] private GameObject _buttonStartGame;
        public GameObject ButtonStartGame => _buttonStartGame;

        // Player Data 
        private PlayerData _playerData;
        string dataFilePath = Path.Combine(Application.dataPath, "GameData", "PlayerData.txt");

        // Singletoning the UIManager
        private static UIManager _instance;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<UIManager>();
                }
                return _instance;
            }
        }


        // In Menu Actions
        public delegate void OnCreateRoom(string roomName);
        public OnCreateRoom OnCreateRoomEvent;

        public delegate void OnLeaveRoom();
        public OnLeaveRoom OnLeaveRoomEvent;

        public void PrepareCanvas()
        {
            // Disables all canvas groups, JIC
            //SetGameTitleCanvasGroup(true);
            SetMainMenuCanvasGroup(true);
            SetCreateRoomCanvasGroup(false);
            SetJoinedRoomCanvasGroup(false);
            SetRoomListCanvasGroup(false);
            SetLoadingCanvasGroup(false);
            SetErrorCanvasGroup(false);
            SetOptionsCanvasGroup(false);
            SetUsernameInputCanvasGroup(false);
        }

        /// <summary>
        /// Method to avoiding repetiotion of code
        /// </summary>
        /// <param name="canvasGroup"></param>
        /// <param name="activeStatus"></param>
        public void SetCanvasGroupState(CanvasGroup canvasGroup, bool activeStatus)
        {
            // Target Alpha based on the status of canvas
            float targetAlpha = activeStatus ? 1f : 0f;

            // Fade using LeanTween
            LeanTween.alphaCanvas(canvasGroup, targetAlpha, 0.5f);

            // If is activating, sets values to true instantly
            if (activeStatus)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else 
            {
                // If not, waits fade to end to set values to false
                LeanTween.delayedCall(0.5f, () =>
                {
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                });
            }
        }


        /// <summary>
        /// Sets the game title canvas group active status
        /// </summary>
        /// <param name="activeStatus"></param>
        //public void SetGameTitleCanvasGroup(bool activeStatus)
        //{
        //    SetCanvasGroupState(_gameTittleCanvasGroup, activeStatus);
        //}

        /// <summary>
        /// Sets the Username Input canvas group active status
        /// </summary>
        /// <param name="activeStatus"></param>
        public void SetUsernameInputCanvasGroup(bool activeStatus)
        {
            _usernameInputField.text = "";
            SetCanvasGroupState(_usernameInputCanvasGroup, activeStatus);
        }

        /// <summary>
        /// Sets the main menu canvas group active status
        /// </summary>
        /// <param name="activeStatus"></param>
        public void SetMainMenuCanvasGroup(bool activeStatus)
        {
            SetCanvasGroupState(_mainMenuCanvasGroup, activeStatus);
        }

        /// <summary>
        /// Sets the options canvas group active status
        /// </summary>
        /// <param name="activeStatus"></param>
        public void SetOptionsCanvasGroup(bool activeStatus)
        {
            // Target Alpha based on the status of canvas
            SetCanvasGroupState(_optionsCanvasGroup, activeStatus);
        }

        /// <summary>
        /// Sets the create room canvas group active status
        /// </summary>
        /// <param name="activeStatus"></param>
        public void SetCreateRoomCanvasGroup(bool activeStatus)
        {
            // Clear room name after creating a room
            _createdRoomNameInputField.text = "";

            SetCanvasGroupState(_createRoomCanvasGroup, activeStatus);
        }

        /// <summary>
        /// Sets the joined room canvas group active status
        /// </summary>
        /// <param name="activeStatus"></param>
        /// <param name="roomName"></param>
        public void SetJoinedRoomCanvasGroup(bool activeStatus, string roomName = "")
        {
            _joinedRoomName.text = roomName;

            SetCanvasGroupState(_joinedRoomCanvasGroup, activeStatus);
        }

        /// <summary>
        /// Sets the room list canvas group active status
        /// </summary>
        /// <param name="activeStatus"></param>
        public void SetRoomListCanvasGroup(bool activeStatus)
        {
            SetCanvasGroupState(_roomListCanvasGroup, activeStatus);
        }

        /// <summary>
        /// Sets the loading canvas group active status
        /// </summary>
        /// <param name="activeStatus"></param>
        /// <param name="loadingText"></param>
        public void SetLoadingCanvasGroup(bool activeStatus, string loadingText = "Loading...")
        {
            //_loadingText.text = loadingText;

            SetCanvasGroupState(_loadingCanvasGroup, activeStatus);
        }

        /// <summary>
        /// Sets the error canvas group active status
        /// </summary>
        /// <param name="activeStatus"></param>
        /// <param name="errorText"></param>
        public void SetErrorCanvasGroup(bool activeStatus, string errorText = "Error : ")
        {
            _errorText.text = errorText;

            SetCanvasGroupState(_errorCanvasGroup, activeStatus);
        }


        /// <summary>
        /// Action when the create room button is clicked
        /// </summary>
        public void ButtonOnCreateRoomClicked()
        {
            if (String.IsNullOrEmpty(_createdRoomNameInputField.text))
            {
                Debug.LogErrorFormat($"*** UIManager: Room Name is empty!");
                return;
            }

            OnCreateRoomEvent?.Invoke(_createdRoomNameInputField.text);
        }

        /// <summary>
        /// Action when the leave room button is clicked
        /// </summary>
        public void ButtonLeaveRoomClicked()
        {
            OnLeaveRoomEvent?.Invoke();
            PUNCharacterSelector.Instance.ReleaseCharacterOnExit();
        }

        /// <summary>
        /// Action when the close error on the Error canvas button is clicked
        /// </summary>
        public void ButtonCloseErrorCanvasGroup()
        {
            SetErrorCanvasGroup(false);
        }

        /// <summary>
        /// Action when the Confirm Username PlayerName canvas button is clicked
        /// </summary>
        public void ButtonConfirmUsernameClicked()
        {
            // Creates and sets with the canvas information a new Player Data
            _playerData = new PlayerData
            {
                playerNickName = _usernameInputField.text,
                newPlayer = false
            };

            // Json Serialization of data & write the info into the file
            string jsonData = JsonUtility.ToJson(_playerData);
            File.WriteAllText(dataFilePath, jsonData);

            // Assign the username to the Nickname of Photon
            PhotonNetwork.NickName = _usernameInputField.text;

            // Manage Canvas
            SetUsernameInputCanvasGroup(false);
            SetMainMenuCanvasGroup(true);
        }

        
        /// <summary>
        /// Action when any of the resolution buttons is clicked
        /// </summary>
        /// <param name="resIndex"></param>
        public void ButtonResolutionClicked(int resIndex)
        {
            // Sets the screen resolution given the resolution button clicked
            //Screen.SetResolution(screenResolutions[resIndex], screenResolutions[++resIndex], false);
        }

        /// <summary>
        /// Action when the reset settings button is clicked
        /// </summary>
        public void ButtonResetSettingsClicked()
        {
            //// Creates a new SettingsData Object to get the original values
            //SettingsData _originalSettings = new SettingsData();
            //// And it assigns it in the UI and Screen
            //_volumeSlider.value = _originalSettings.volume;
            //_brightnessSlider.value = _originalSettings.brightness;
            //Screen.SetResolution(_originalSettings.resolutionX, _originalSettings.resolutionY, false);
        }

        /// <summary>
        /// Action when the save and exit settings button is clicked
        /// </summary>
        public void ButtonSaveAndExitClicked()
        {
            //// Creates a new SettingsData Object with the current values of UI
            //SettingsData settingsData = new SettingsData
            //{
            //    volume = (int)_volumeSlider.value,
            //    brightness = (int)_brightnessSlider.value,
            //    resolutionX = Screen.currentResolution.width,
            //    resolutionY = Screen.currentResolution.height
            //};

            //// Searchs for the File
            //string dataFilePath = Path.Combine(Application.dataPath, "GameData", "SettingsData.txt");
            //// Converts to JSON
            //string jsonData = JsonUtility.ToJson(settingsData, true);
            //// Writes the values in file 
            //File.WriteAllText(dataFilePath, jsonData);

            // Sets Canvas
            SetOptionsCanvasGroup(false);
            SetMainMenuCanvasGroup(true);
        }

    }
}