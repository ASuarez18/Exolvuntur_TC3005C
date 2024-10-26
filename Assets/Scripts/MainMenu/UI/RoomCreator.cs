using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using LocalPhoton.MainMenu;

namespace MainMenu.UI
{
    /// <summary>
    /// Class in chearge of creating rooms and populate hte room list
    /// </summary>
    public class RoomCreator : MonoBehaviour
    {
        [SerializeField] private PUNRoomButtonInfo _roomButtonPrefab;
        [SerializeField] private RectTransform _roomListParent;
        private List<RoomInfo> _roomsList = new List<RoomInfo>();

        // Singletoning the RoomCreator
        private static RoomCreator _instance;
        public static RoomCreator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RoomCreator>();
                }

                if (_instance == null)
                {
                    // Create a new GameObject and add the RoomCreator to it
                    GameObject go = new GameObject("RoomCreator");
                    go.AddComponent<RoomCreator>();
                    _instance = go.GetComponent<RoomCreator>();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Populates the room list with the rooms received
        /// </summary>
        /// <param name="roomListInfo"></param>
        public void PopulateRoomList(List<RoomInfo> roomListInfo)
        {
            // Clean the room list parent from previous rooms
            foreach (Transform child in _roomListParent)
            {
                Destroy(child.gameObject);
            }

            foreach (RoomInfo roomInfo in roomListInfo)
            {
                // If the room is not full and is not removed from the list
                if (roomInfo.PlayerCount != roomInfo.MaxPlayers && !roomInfo.RemovedFromList)
                {
                    Debug.Log($"*** Room in loby {roomInfo.Name}");
                    PUNRoomButtonInfo roomButton = Instantiate(_roomButtonPrefab, _roomListParent);
                    roomButton.SetButtonInfo(roomInfo);
                    roomButton.transform.parent = _roomListParent;
                    roomButton.transform.localScale = Vector3.one;
                    roomButton.transform.localRotation = Quaternion.identity;

                    _roomsList.Add(roomInfo);
                }
            }
        }
    }
}