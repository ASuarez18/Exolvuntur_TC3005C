using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerController.Inventory;
using Interfaces;
using Unity.VisualScripting;

public class Sal : MonoBehaviour, IInteraction
{
    public GameObject cubo;
    public Transform mano;
    public float fuerza;

    private bool activo;
    private bool enMano;
    private Vector3 escala;

    private void Start()
    {
        escala = cubo.transform.localScale;
        
    }

    void Update()
    {
        

        if(Input.GetMouseButtonDown(0))
        {
           

           
                // Cambiar a la dirección de la cámara
                cubo.GetComponent<Rigidbody>().isKinematic = false;
                cubo.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Vector3 direccionLanzamiento = Camera.main.transform.forward;
                cubo.GetComponent<Rigidbody>().AddForce(direccionLanzamiento * fuerza, ForceMode.Impulse);
          
            
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            activo = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            activo = false;
        }
    }

    public void InteractObject()
    {
        FindObjectOfType<SlotSelector>().CollectObject(this.gameObject);
    }

    
}
