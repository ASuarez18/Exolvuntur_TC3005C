using UnityEngine;
using Interfaces;


namespace PlayerController
{
    public class PlayerInteraction : MonoBehaviour
    {

        private Camera _mainCamera;
        public float rayDistance = 2f;
        public CanvasGroup interactText;


        private void Start()
        {
            interactText.alpha = 0f;
            _mainCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            CheckRayInteraction();
        }

        /// <summary>
        /// Function that verifies and searches for a ray collision with a collider of interactable objectes
        /// </summary>
        void CheckRayInteraction()
        {
            Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction, Color.red);

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                IInteraction interactableObject = hit.transform.GetComponent<IInteraction>();
                if (interactableObject != null)
                {
                    interactText.alpha = 1f;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactableObject.InteractObject();
                    }
                }
                else
                    interactText.alpha = 0f;
            }
            else
                interactText.alpha = 0f;
        }
    }
}