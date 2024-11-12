using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sal : MonoBehaviour
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
        if(Input.GetKeyDown(KeyCode.E))
        {
            cubo.transform.SetParent(mano);
            cubo.transform.position = mano.position;
            cubo.transform.rotation = mano.rotation;
            cubo.GetComponent<Rigidbody>().isKinematic = true;
            enMano = true;
        }

        if(Input.GetMouseButtonDown(0))
        {
            cubo.transform.SetParent(null);
            cubo.GetComponent<Rigidbody>().isKinematic = false;
            cubo.transform.localScale = escala;

            if(enMano == true)
            {
                // Cambiar a la dirección de la cámara
                Vector3 direccionLanzamiento = Camera.main.transform.forward;
                cubo.GetComponent<Rigidbody>().AddForce(direccionLanzamiento * fuerza, ForceMode.Impulse);
                enMano = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            cubo.transform.SetParent(null);
            cubo.GetComponent<Rigidbody>().isKinematic = false;
            cubo.transform.localScale = escala;
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
}
