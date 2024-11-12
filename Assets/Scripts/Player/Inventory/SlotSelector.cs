using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using Photon.Pun;

namespace Player.Inventory
{
    /// <summary>
    /// Class in charge of the slot selector in the inventory of the player
    /// </summary>
    public class SlotSelector : MonoBehaviourPunCallbacks
    {
        // Slot data structure to store the object model
        [SerializeField]
        public struct SlotData
        {
            public GameObject utilityModel;
        }

        // Current slot selected
        public UtilityScriptableObject CurrentSlot {get; private set;}

        // All objects
        [SerializeField] private UtilityScriptableObject[] _utilityData;
        [SerializeField] private UtilityData[] _utilities;

        // Delegate for the slot change event
        public delegate void OnSlotChange(UtilityScriptableObject utility);
        public OnSlotChange onSlotChangeEvent;

        // Current slot selected
        public int _currentSlotIndex;

        /// <summary>
        /// Method to enable the component
        /// </summary>
        void OnEnable()
        {
            // Photon
            // enabled = photonView.IsMine;
            ChangeSlot();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Enables or disables this component based on the player's ownership
            //enabled = photonView.IsMine;

            // Change the weapon in all clients
            //photonView.RPC(nameof(PUNChangeUtility), RpcTarget.All, _currentSlotIndex);
        }

        private void Update()
        {
            // Change the slot with mouse wheel
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                _currentSlotIndex++;
                //photonView.RPC(nameof(PUNChangeUtility), RpcTarget.All, _currentSlotIndex);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                _currentSlotIndex--;
                //photonView.RPC(nameof(PUNChangeUtility), RpcTarget.All, _currentSlotIndex);
            }

            // Select the slot with the number keys
            for (int i = 1; i <= _utilityData.Length; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    _currentSlotIndex = i - 1;
                    ChangeSlot();
                }
            }
        }

        /// <summary>
        /// Method to change the slot of the inventory
        /// </summary>
        private void ChangeSlot()
        {
            if (_currentSlotIndex < 0)
            {
                _currentSlotIndex = _utilityData.Length - 1;
            }
            else if (_currentSlotIndex >= _utilityData.Length)
            {
                _currentSlotIndex = 0;
            }

            // Notify the change of the slot to the player
            onSlotChangeEvent?.Invoke(_utilityData[_currentSlotIndex]);

            // Visually change the slot
            foreach (var utility in _utilities)
            {
                utility.utilityModel.SetActive(false);
            }

            _utilities[_currentSlotIndex].utilityModel.SetActive(true);
        }

        // Get the data of the current slot
        public SlotData GetCurrentSlotData()
        {
            return _utilities[_currentSlotIndex];
        }

        /// <summary>
        /// Method to update the slot selector in all clients
        /// </summary>
        [PunRPC]
        public void PUNChangeUtility(int slotIndex)
        {
            _currentSlotIndex = slotIndex;
            ChangeSlot();
        }
    }
}