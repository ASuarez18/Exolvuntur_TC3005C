using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using Unity.AI.Navigation;
using PlayerController.Inventory;

public class biblia : MonoBehaviour, IInteraction
{
    [SerializeField] private Seguro _Seguro;
    [SerializeField] private float tiempo= 5f;
    [SerializeField] private float radio = 5f;

    private float cooldown = 0f;

    float tiempoActual = 0f;



    
    private void Update()
    {

        if(Input.GetMouseButton(0) && !_Seguro.bloqueado && cooldown <= 0f)
        {
            //Cuenta regresiva cuando se activa el ataque
            tiempoActual += Time.deltaTime;
            if(tiempoActual >= tiempo)
            {
                //Ataque y cooldown
                
                Ataque();
                tiempoActual = 0f;
                cooldown = 3f;

            }

            

        }
        else
        {
            tiempoActual = 0f;
            cooldown = 0f;

        }

        if(cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }

        
        
    }

    private void Ataque()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radio);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Debug.Log("Atacando");
            }
        }
    }

    public void InteractObject()
    {
        FindObjectOfType<SlotSelector>().CollectObject(this.gameObject);
    }


}
