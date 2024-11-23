using System.Collections;
using System.Collections.Generic;
using PlayerController.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio.SoundFX
{
    /// <summary>
    /// Class in charge of managing the Sound Effects in the game.
    /// </summary>
    public class SFXManager : MonoBehaviour
    {
        // Singletoning the SFXManager
        public static SFXManager instance;

        [SerializeField] private AudioSource soundFXObject;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public AudioSource PlayWhisperSFX(AudioClip _audioClip, Transform _transform, float volume)
        {
            AudioSource _audioSource = Instantiate(soundFXObject, _transform.position, Quaternion.identity);
            _audioSource.clip = _audioClip;
            _audioSource.volume = volume;
            _audioSource.loop = true;
            _audioSource.Play();

            return _audioSource;
        }
    }
}
