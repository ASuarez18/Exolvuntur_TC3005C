using System.Collections;
using System.Collections.Generic;
using Audio.SoundFX;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace PlayerController.UI
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
        [SerializeField] private SFXManager _sfxManager;
        [SerializeField] private AudioClip _whisperSFX;
        private AudioSource _whisperAudioSource;

        private void Start()
        {
            _camera = Camera.main;
            _whisperAudioSource = _sfxManager.PlayWhisperSFX(_whisperSFX, transform, 0f);
            _sanitySlider.onValueChanged.AddListener(OnSanityValueChanged);
        }

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

        private void OnSanityValueChanged(float value)
        {
            if (_whisperAudioSource != null)
            {
                float normalizedValue = 1 - (value / _sanitySlider.maxValue); // Normalizar el valor
                _whisperAudioSource.volume = normalizedValue; // Ajustar el volumen directamente
            }
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
