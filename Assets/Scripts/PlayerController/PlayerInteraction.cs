using UnityEngine;
using Interfaces;


namespace PlayerController
{
    public class PlayerInteraction : MonoBehaviour
    {

        private Camera _mainCamera;
        public float rayDistance = 2f;
        private CanvasGroup interactText;


        private void Start()
        {
            interactText.alpha = 0f;
            _mainCamera = Camera.main;
            interactText = GameObject.Find("HUD_Interaction").GetComponent<CanvasGroup>();
            Debug.LogError(interactText);
        }

        void Update()
        {
            if(interactText != null)
            {
                CheckRayInteraction();
            }
            

        }
       
        /// <summary>
        /// Function that verifies and searches for a ray collision with a collider of interactable objectes
        /// </summary>
        void CheckRayInteraction()
        {
            Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

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