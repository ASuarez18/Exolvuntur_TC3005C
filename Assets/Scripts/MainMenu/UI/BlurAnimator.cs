using UnityEngine;

namespace UI.MainMenu
{   
    /// <summary>
    /// Class in charge of animating the blur effect of the UI's background.
    /// </summary>
    public class BlurAnimator : MonoBehaviour
    {
        public Material material;
        public float minBlurSize = 90.0f;
        public float maxBlurSize = 150.0f;
        public float animationSpeed = 1.0f;

        /// <summary>
        /// Updates and animates the blur size of the material.
        /// </summary>
        private void Update()
        {
            float blurSize = Mathf.PingPong(Time.time * animationSpeed, maxBlurSize - minBlurSize) + minBlurSize;
            material.SetFloat("_BlurSize", blurSize);
        }
    }
}
