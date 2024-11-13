using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
        [SerializeField] private Volume _postProcessingVolume;
        [SerializeField] private Camera _camera;

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

            // Update the post processing volume
            float sanityPercentage = _sanitySlider.value / _sanitySlider.maxValue;
            _postProcessingVolume.weight = 1 - sanityPercentage;

            // Update the camera rotation in the z axis
            float maxRotationZ = 30f; // Define el máximo ángulo de rotación en el eje Z
            float rotationZ = (1 - sanityPercentage) * maxRotationZ;
            _camera.transform.rotation = Quaternion.Euler(_camera.transform.rotation.eulerAngles.x, _camera.transform.rotation.eulerAngles.y, rotationZ);
        }
    }
}
