using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    /// <summary>
    /// Class in charge of the player sanity UI (Slider)
    /// </summary>
    public class HUDPlayerSanity : MonoBehaviour
    {
        [SerializeField] private Slider _sanitySlider;
        [SerializeField] private Image _sanityFillImage;

        private void Update()
        {
            UpdateSanityColorValue();
        }

        /// <summary>
        /// Sets the slider max value
        /// </summary>
        /// <param name="value"></param>
        public void SetSliderMaxValue(float value)
        {
            _sanitySlider.maxValue = value;
        }

        /// <summary>
        /// Sets the slider current value
        /// </summary>
        /// <param name="value"></param>
        public void SetSliderValue(float value)
        {
            _sanitySlider.value = value;
            UpdateSanityColorValue();
        }

        private void UpdateSanityColorValue()
        {
            // Get the value of the slider and normalize it
            float value = _sanitySlider.value / _sanitySlider.maxValue;
            Color color = Color.Lerp(Color.black, Color.white, value);
            _sanityFillImage.color = color;
        }
    }
}
