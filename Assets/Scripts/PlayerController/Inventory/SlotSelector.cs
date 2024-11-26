using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjects;
using Photon.Pun;
using LocalPhoton.Gameplay;
// using Photon.Pun;

namespace PlayerController.Inventory
{
    /// <summary>
    /// Class in charge of the slot selector in the inventory of the player
    /// </summary>
    public class SlotSelector : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Animator _animator;
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
        public Transform _inventoryOwnPoint;
        public Transform _inventoryNetworkPoint;

        // Delegate for the slot change event
        public delegate void OnSlotChanged(UtilityScriptableObject utility);
        public OnSlotChanged OnSlotChangedEvent;

        private GameObject[] collectables;
        private int currentCollectableIndex = 0;

        private GameObject currentObjectInteracted;
        // Current slot index
        public int _currentSlotIndex;

        // Max slots
        private const int MAX_SLOTS = 3;

        public Image[] hudSlots;

        /// <summary>
        /// Method to enable the component
        /// </summary>
        public override void OnEnable()
        {
            base.OnEnable();
            // enabled = photonView.IsMine;
            ChangeSlot();
        }


        // Start is called before the first frame update
        private void Start()
        {
            // Enables or disables this component based on the player's ownership
            enabled = photonView.IsMine;

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
           
            //if(!enabled) return;

            // Change the slot with mouse wheel
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                _currentSlotIndex++;
                //photonView.RPC(nameof(PUNChangeUtility), RpcTarget.All, _currentSlotIndex);
                if (_currentSlotIndex >= _utilities.Length)
                {
                    _currentSlotIndex = 0;
                }
                photonView.RPC(nameof(ChangeWeapon),RpcTarget.All, _currentSlotIndex);
                //ChangeSlot();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                _currentSlotIndex--;
                if (_currentSlotIndex < 0)
                {
                    _currentSlotIndex = _utilities.Length - 1;
                }
                //photonView.RPC(nameof(PUNChangeUtility), RpcTarget.All, _currentSlotIndex);
                 photonView.RPC(nameof(ChangeWeapon),RpcTarget.All, _currentSlotIndex);
                //ChangeSlot();
            }

            // Select the slot with the number keys
            for (int i = 1; i <= _utilityData.Length; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    _currentSlotIndex = i - 1;
                     photonView.RPC(nameof(ChangeWeapon),RpcTarget.All, _currentSlotIndex);
                    //ChangeSlot();
                }
            }
        }

        public void CollectObject(GameObject currentCollectable)
        {
            //if (!enabled) return;

            //currentObjectInteracted = currentCollectable;

            if (collectables.Length == 0) return;

            if (_utilities.Length >= MAX_SLOTS)
            {
                Debug.Log("No more slots available.");
                return;
            }

            if(currentCollectable.transform.tag == "Finish"){
                PUNMatchManager.Instance.UpdateStatSent(0, PUNEventCodes.PlayerStats.TotalObjects ,1);
            }
            

            //GameObject currentCollectable = collectables[currentCollectableIndex];
            SpriteRenderer _spriteRenderer = currentCollectable.GetComponent<SpriteRenderer>();

            // Si no hay un objeto en el slot 
            if (_spriteRenderer == null)
            {
                Debug.LogError($"No SpriteRenderer found on {currentCollectable.name}");
                return;
            }

            photonView.RPC(nameof(ParentObjet),RpcTarget.All, currentCollectable.name);

            // Emparenta el objeto con el objeto padre
            if(currentCollectable.TryGetComponent<Seguro>(out Seguro seguro))
            {
                seguro.bloqueado = false;
                seguro.anim = _animator;

            }

            
            

            // Actualiza el índice para el siguiente objeto
            currentCollectableIndex = (currentCollectableIndex + 1) % collectables.Length;

            UpdateHUD();
        }


        [PunRPC]
        public void ParentObjet(string objectName)
        {
            Debug.LogWarning("Nombre: " + objectName);
            
            GameObject collectableObject = GameObject.Find(objectName);
            //Debug.LogWarning("Previo a emparentado: " + collectableObject.transform.parent.name);

            Array.Resize(ref _utilities, _utilities.Length + 1);
            _utilities[_utilities.Length - 1] = new UtilityData
            {
                utilityInstance = collectableObject,
                utilitySprite = collectableObject.GetComponent<SpriteRenderer>().sprite
            };

            collectableObject.transform.SetParent(_inventoryOwnPoint);
            Debug.LogWarning("Posterior a emparentado: " + collectableObject.transform.parent.name);

            collectableObject.transform.localPosition = Vector3.zero;
        } 

        [PunRPC]
        public void ChangeWeapon(int currentObjSlot)
        {
            _currentSlotIndex = currentObjSlot;

            Debug.LogWarning("Cambio de arma: " + _currentSlotIndex);

            ChangeSlot();
        } 

        /// <summary>
        /// Method to change the slot of the inventory
        /// </summary>
        private void ChangeSlot()
        {
            //if (!enabled) return;
            Debug.LogWarning("Tamaño de armas:" + _utilities.Length);
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

            if(photonView.IsMine){
                UpdateHUD();
            }
            
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