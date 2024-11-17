using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjects;
// using Photon.Pun;

namespace PlayerController.Inventory
{
    /// <summary>
    /// Class in charge of the slot selector in the inventory of the player
    /// </summary>
    public class SlotSelector : MonoBehaviour //PunCallbacks
    {
        // Slot data structure to store the object model
        [Serializable]
        public struct UtilityData
        {
            public GameObject utilityInstance;

            public Sprite utilitySprite;
        }

        // Current utility
        public UtilityScriptableObject _currentUtility { get; private set; }

        // All utility
        [SerializeField] private UtilityScriptableObject[] _utilityData;
        [SerializeField] private UtilityData[] _utilities = new UtilityData[0];

        // Parent object to store the utilities
        public Transform _utilitiesParent;

        // Delegate for the slot change event
        public delegate void OnSlotChanged(UtilityScriptableObject utility);
        public OnSlotChanged OnSlotChangedEvent;

        private GameObject[] collectables;
        private int currentCollectableIndex = 0;
        // Current slot index
        public int _currentSlotIndex;

        // Max slots
        private const int MAX_SLOTS = 3;

        public Image[] hudSlots;

        /// <summary>
        /// Method to enable the component
        /// </summary>
        void OnEnable()
        {
            // enabled = photonView.IsMine;
            ChangeSlot();
        }


        // Start is called before the first frame update
        private void Start()
        {
            // Enables or disables this component based on the player's ownership
            //enabled = photonView.IsMine;

            // Change the weapon in all clients
            //photonView.RPC(nameof(PUNChangeUtility), RpcTarget.All, _currentSlotIndex);
            hudSlots = new Image[MAX_SLOTS];
            collectables = GameObject.FindGameObjectsWithTag("Collectable");
            GameObject HUDReference = GameObject.Find("Canvas_HUDobjs");
            Image[] childImages = HUDReference.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponentsInChildren<Image>();
            Debug.LogError("Numero de childs con image" + childImages.Length);
            for (int i=0;i<=childImages.Length;i++)
            {
                if (i != 0)
                {
                    hudSlots[i-1] = childImages[i];
                }
                
            }
            Debug.LogError("Numero de childs con image" + hudSlots.Length);
        }
        

        private void Update()
        {
           

            // Change the slot with mouse wheel
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                _currentSlotIndex++;
                //photonView.RPC(nameof(PUNChangeUtility), RpcTarget.All, _currentSlotIndex);
                if (_currentSlotIndex >= _utilities.Length)
                {
                    _currentSlotIndex = 0;
                }
                ChangeSlot();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                _currentSlotIndex--;
                if (_currentSlotIndex < 0)
                {
                    _currentSlotIndex = _utilities.Length - 1;
                }
                //photonView.RPC(nameof(PUNChangeUtility), RpcTarget.All, _currentSlotIndex);
                ChangeSlot();
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

        public void CollectObject(GameObject currentCollectable)
        {
            if (collectables.Length == 0) return;

            if (_utilities.Length >= MAX_SLOTS)
            {
                Debug.Log("No more slots available.");
                return;
            }

            //GameObject currentCollectable = collectables[currentCollectableIndex];
            SpriteRenderer _spriteRenderer = currentCollectable.GetComponent<SpriteRenderer>();

            if (_spriteRenderer == null)
            {
                Debug.LogError($"No SpriteRenderer found on {currentCollectable.name}");
                return;
            }

            Array.Resize(ref _utilities, _utilities.Length + 1);
            _utilities[_utilities.Length - 1] = new UtilityData
            {
                utilityInstance = currentCollectable,
                utilitySprite = currentCollectable.GetComponent<SpriteRenderer>().sprite
            };

            // Emparenta el objeto con el objeto padre
            currentCollectable.GetComponent<Seguro>().bloqueado = false;
            currentCollectable.transform.SetParent(_utilitiesParent);
            currentCollectable.transform.localPosition = Vector3.zero;

            // Actualiza el índice para el siguiente objeto
            currentCollectableIndex = (currentCollectableIndex + 1) % collectables.Length;

            UpdateHUD();
        }

        /// <summary>
        /// Method to change the slot of the inventory
        /// </summary>
        private void ChangeSlot()
        {
            if (_utilities.Length == 0) return;

            if (_currentSlotIndex < 0)
            {
                _currentSlotIndex = _utilities.Length - 1;
            }
            else if (_currentSlotIndex >= _utilities.Length)
            {
                _currentSlotIndex = 0;
            }

            // Lógica de cambio de ranura
            OnSlotChangedEvent?.Invoke(_utilityData[_currentSlotIndex]);

            foreach (var utility in _utilities)
            {
                utility.utilityInstance.SetActive(false);
            }

            _utilities[_currentSlotIndex].utilityInstance.SetActive(true);

            UpdateHUD();
        }

        private void UpdateHUD()
        {
            for (int i = 0; i < hudSlots.Length; i++)
            {
                if (i < _utilities.Length)
                {
                    hudSlots[i].sprite = _utilities[i].utilitySprite;
                    hudSlots[i].enabled = true;

                    if (i == _currentSlotIndex)
                    {
                        hudSlots[i].color = Color.white;
                        hudSlots[i].rectTransform.localScale = Vector3.one;
                    }
                    else
                    {
                        hudSlots[i].color = Color.gray;
                        hudSlots[i].rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    }
                }
                else
                {
                    hudSlots[i].enabled = false;
                }
            }
        }


        // Get the data of the current slot
        public UtilityData GetCurrentSlotData()
        {
            return _utilities[_currentSlotIndex];
        }

        /// <summary>
        /// Method to update the slot selector in all clients
        /// </summary>
        // [PunRPC]
        public void PUNChangeUtility(int slotIndex)
        {
            _currentSlotIndex = slotIndex;
            ChangeSlot();
        }
    }
}