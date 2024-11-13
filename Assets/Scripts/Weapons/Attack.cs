using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;

namespace PlayerController.Inventory
{
    public class Attack : MonoBehaviour
    {
        [SerializeField]
        private UtilityScriptableObject _weaponData;

        public GameObject cubo;
        public Transform mano;
        public float fuerza;
        private bool enMano;
        private Vector3 escala;
        public Transform point;

        private float tiempoActual = 0f;

        private const float TIEMPO = 5f;


        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                cubo.transform.SetParent(mano);
                cubo.transform.position = mano.position;
                cubo.transform.rotation = mano.rotation;
                cubo.GetComponent<Rigidbody>().isKinematic = true;
                enMano = true;
            }

            if(Input.GetMouseButtonDown(0))
            {
                switch (_weaponData.weaponType)
                {
                    case WeaponsEnum.Sal:
                        ataqueSal();
                        break;
                    case WeaponsEnum.Biblia:
                        if (Input.GetMouseButtonDown(0))
                        {
                            tiempoActual += Time.deltaTime;
                            if (tiempoActual >= TIEMPO)
                            {
                                ataqueBiblia();
                                tiempoActual = 0f;
                            }
                        }
                        else
                        {
                            tiempoActual = 0f;
                        }
                        break;
                    case WeaponsEnum.Crucifijo:
                        ataqueCrucifijo();
                        break;
                    case WeaponsEnum.AguaBendita:
                        ataqueSal();
                        break;
                        
                }
            }
            
            
        }



        void ataqueSal()
        {
            if(Input.GetMouseButtonDown(0))
            {
                cubo.transform.SetParent(null);
                cubo.GetComponent<Rigidbody>().isKinematic = false;
                cubo.transform.localScale = escala;

                if(enMano == true)
                {
                    // Cambiar a la dirección de la cámara
                    Vector3 direccionLanzamiento = Camera.main.transform.forward;
                    cubo.GetComponent<Rigidbody>().AddForce(direccionLanzamiento * _weaponData.Force, ForceMode.Impulse);
                    enMano = false;
                }
            }      
        }


        void ataqueBiblia()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _weaponData.Range);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Debug.Log("Atacando");
                }
            }
        }

        void ataqueCrucifijo()
        {
            Collider[] colliders = Physics.OverlapCapsule(point.position, point.position + Camera.main.transform.forward * _weaponData.Range, 2f);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
         
                }
            }

        }
    }



}
