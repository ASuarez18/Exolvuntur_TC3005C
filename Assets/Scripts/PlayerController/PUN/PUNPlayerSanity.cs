using System.Collections;
using System.Collections.Generic;
using PlayerController.UI;
using UnityEngine;

namespace PlayerController.PUN
{   
    /// <summary>
    /// Class in charge of managing the player's sanity through the HUD
    /// </summary>
    public class PUNPlayerSanity : MonoBehaviour
    {
        [SerializeField] private float _maxSanity = 100;
        public float Sanity { get; private set; }
        [SerializeField] private HUDPlayerSanity _hudPlayerSanity;

        private void Start()
        {
            // Photon Instance
            // enabled = photonView.IsMine;

            Sanity = _maxSanity;

            _hudPlayerSanity = FindObjectOfType<HUDPlayerSanity>();

            // Set the player's sanity on the HUD
            // if (photonView.IsMine)
            // {
            //     _hudPlayerSanity.SetSliderMaxValue(_maxSanity);
            //     _hudPlayerSanity.SetSliderValue(Sanity);
            // }

            _hudPlayerSanity.SetSliderMaxValue(_maxSanity);
            _hudPlayerSanity.SetSliderValue(Sanity);
        }

        /// <summary>
        /// Method to take damage from an enemy
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="enemyName"></param>
        public void TakeDamage(int damage, string enemyName)
        {
            Sanity -= damage;

            // Update the player's sanity on the HUD
            // if (photonView.IsMine)
            // _hudPlayerSanity.SetSliderValue(Sanity);

            if (Sanity <= 0)
            {
                Sanity = 0;
                // Print a console message to know wich enemy make damage to the player
                Debug.LogFormat($"Player was stunned by {enemyName}");
            }
            else
            {
                // Print a console message to know wich enemy hit to the player
                Debug.LogFormat($"*** Player have been hit by [{enemyName}]! NOW THEIR SANITY IS: {Sanity}");
            }
        }
    }
}