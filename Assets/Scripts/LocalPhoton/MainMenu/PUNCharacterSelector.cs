using MainMenu.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LocalPhoton.MainMenu
{
    public class PUNCharacterSelector : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Button _nextCharacter;
        [SerializeField] private Button _prevCharacter;
        [SerializeField] private Button _selectCharacter;

        [SerializeField] private Button _characterImgContainer;
        private int _charIndex;
        [SerializeField] private List<Sprite> _characterSprites;
        private Color _selectedColor;
        private Color _unselectedColor;

        [SerializeField] private List<bool> _characterSelection;
        [SerializeField]  private int _previousSelection;

        private const string SELECTED_CHARACTER = "SelectedCharacter";
        public static int SelectedCharacterIndex { get; private set; }
        // The player properties that will be synced across the network
        ExitGames.Client.Photon.Hashtable _playerProperties = new()
        {
            { SELECTED_CHARACTER, -1 }
        };

        // Singletoning the CharacterSelector
        private static PUNCharacterSelector _instance;
        public static PUNCharacterSelector Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PUNCharacterSelector>();
                }
                return _instance;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            _charIndex = 0;
            _characterImgContainer.GetComponent<Button>().image.sprite = _characterSprites[0];
            _characterImgContainer.GetComponent<Button>().image.color = _unselectedColor;
            _nextCharacter.onClick.AddListener(() => ChangeCharacterSelection(1));
            _prevCharacter.onClick.AddListener(() => ChangeCharacterSelection(-1));
            _selectCharacter.onClick.AddListener(() => SelectCharacter());

        }

        public override void OnDisable()
        {
            base.OnDisable();
            _charIndex = 0; 
            if (_nextCharacter == null || _prevCharacter == null)
                return;

            _nextCharacter.onClick.RemoveListener(() => ChangeCharacterSelection(1));
            _prevCharacter.onClick.RemoveListener(() => ChangeCharacterSelection(-1));
        }

        private void Awake()
        {
            _characterSelection = new List<bool>() { false, false, false, false };
            _selectedColor = _characterImgContainer.GetComponent<Button>().image.color *= .5f;
            _unselectedColor = Color.white;
            _previousSelection = -1;
        }

        /// <summary>
        ///   Change character "selection" only in UI, functions given the arrow buttons
        /// </summary>
        /// <param name="value"></param>
        private void ChangeCharacterSelection(int value)
        {
            _charIndex = _charIndex + value;

            if (_charIndex > _characterSprites.Count - 1)
                _charIndex = 0;
            else if (_charIndex < 0)
                _charIndex = _characterSprites.Count - 1;

            _characterImgContainer.GetComponent<Button>().image.sprite = _characterSprites[_charIndex];

            CharacterSelectionUpdate();
        }

        /// <summary>
        /// Method that will apply selection of character given the one that is in screen
        /// </summary>
        private void SelectCharacter()
        {
            if (_previousSelection != -1)
            {
                if (_previousSelection == _charIndex)
                    return;
                _characterSelection[_previousSelection] = false;
            }

            _playerProperties[SELECTED_CHARACTER] = _charIndex;
            _characterSelection[_charIndex] = true;
            _previousSelection = _charIndex;

            SelectedCharacterIndex = _charIndex;

            _selectCharacter.GetComponent<Button>().interactable = false;
            _characterImgContainer.GetComponent<Button>().image.color *= .5f;

            // Set the player properties to the selectec character and sync it across the network
            PhotonNetwork.LocalPlayer.SetCustomProperties(_playerProperties);
            UpdateSelection();
        }

        /// <summary>
        /// Method that updates the interactable of the selection button and alppha of character image given if it was selected
        /// </summary>
        private void CharacterSelectionUpdate()
        {
            if (_characterSelection[_charIndex] == true)
            {
                _selectCharacter.GetComponent<Button>().interactable = false;
                _characterImgContainer.GetComponent<Button>().image.color = _selectedColor;
            }
            else
            {
                _selectCharacter.GetComponent<Button>().interactable = true;
                _characterImgContainer.GetComponent<Button>().image.color = _unselectedColor;
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

            Debug.LogFormat($"*** OnPlayerPropertiesUpdate : Player {targetPlayer.NickName} changed properties [{SELECTED_CHARACTER}] to [{changedProps[SELECTED_CHARACTER]}]...");

            // Check if the player has selected a character
            if (changedProps.ContainsKey(SELECTED_CHARACTER))
            {
                UpdateSelection();
            }
        }

        /// <summary>
        /// Update the character selection for all players in the room, local & remote
        /// </summary>
        private void UpdateSelection()
        {
            // Keep track of the local player's selected character
            int localPlayerSelectedCharacter = -1;

            // Check if the local player has selected a character
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(SELECTED_CHARACTER))
            {
                // Get the local player's selected character
                localPlayerSelectedCharacter = (int)PhotonNetwork.LocalPlayer.CustomProperties[SELECTED_CHARACTER];

            }

            // For each character button, check if it's selected
            for (int i = 0; i < _characterSelection.Count; i++)
            {
                bool isCharacterSelected = false;

                // Check if the local player has selected this character
                if (localPlayerSelectedCharacter == i)
                {
                    isCharacterSelected = true;
                }

                // Check if another player has selected this character
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    if (player != PhotonNetwork.LocalPlayer && player.CustomProperties.ContainsKey(SELECTED_CHARACTER) && (int)player.CustomProperties[SELECTED_CHARACTER] == i)
                    {
                        isCharacterSelected = true;
                        break;
                    }
                }

                // Update the button interactable state
                _characterSelection[i] = isCharacterSelected;

            }
            CharacterSelectionUpdate();
        }

        public void ReleaseCharacterOnExit()
        {
            if (_previousSelection == -1)
                return;

            //_characterSelection[_previousSelection] = false;

            //// For each character button, check if it's selected
            //for (int i = 0; i < _characterSelection.Count; i++)
            //{
            //    bool isCharacterSelected = false;

            //    // Check if the local player has selected this character
            //    if (localPlayerSelectedCharacter == i)
            //    {
            //        isCharacterSelected = true;
            //    }

            //    // Check if another player has selected this character
            //    foreach (var player in PhotonNetwork.PlayerList)
            //    {
            //        if (player != PhotonNetwork.LocalPlayer && player.CustomProperties.ContainsKey(SELECTED_CHARACTER) && (int)player.CustomProperties[SELECTED_CHARACTER] == i)
            //        {
            //            isCharacterSelected = true;
            //            break;
            //        }
            //    }

            //    // Update the button interactable state
            //    _characterSelection[i] = isCharacterSelected;

            //}
            //CharacterSelectionUpdate();
        }
    }
}