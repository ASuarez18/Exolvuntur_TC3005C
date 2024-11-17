using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalPhoton.MainMenu
{
    /// <summary>
    /// Class in charge of the room button info and actions
    /// </summary>
    public class PUNRoomButtonInfo : MonoBehaviour
    {
        [SerializeField] public TMP_Text _roomName;
        private RoomInfo _roomInfo;
        private Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();

            if (_button != null)
                _button.onClick.AddListener(ButtonJoinRoom);
        }

        private void OnDisable()
        {
            if (_button != null)
                _button.onClick.RemoveListener(ButtonJoinRoom);
        }

        // Static event to be called when the user clicks the button to join the room
        public delegate void OnJoinRoom(RoomInfo roomInfo);
        public static OnJoinRoom OnJoinRoomEvent;

        /// <summary>
        /// Sets the button info and the room info
        /// </summary>
        /// <param name="roomInfo"></param>
        public void SetButtonInfo(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;
            _roomName.text = roomInfo.Name;
        }

        /// <summary>
        /// Action to be called when the user clicks the button to join the room
        /// </summary>
        public void ButtonJoinRoom()
        {
            OnJoinRoomEvent?.Invoke(_roomInfo);
        }
    }
}