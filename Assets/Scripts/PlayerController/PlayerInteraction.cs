using UnityEngine;
using Interfaces;
using Photon.Pun;


namespace PlayerController
{
    public class PlayerInteraction : MonoBehaviourPunCallbacks
    {

        private Camera _mainCamera;
        private float rayDistance;
        private CanvasGroup interactText;


       
        public override void OnEnable()
        {
            base.OnEnable();
            rayDistance = 20f;
            _mainCamera = Camera.main;
            interactText = GameObject.Find("HUD_Interaction").GetComponent<CanvasGroup>();
            interactText.alpha = 0f;
            //Debug.LogError(interactText);
        }

        private void Update()
        {
            if(!photonView.IsMine) return;

            if(interactText != null)
            {
                CheckRayInteraction();
            }
            else
            {
                _mainCamera = Camera.main;
                interactText = GameObject.Find("HUD_Interaction").GetComponent<CanvasGroup>();
                Debug.LogError(interactText);
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
                        interactableObject.InteractObject(this.gameObject);
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