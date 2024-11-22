using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using Unity.AI.Navigation;
using PlayerController.Inventory;
using Enemy.Manager;

public class biblia : MonoBehaviour, IInteraction
{
    [SerializeField] private Seguro _seguro;
    [SerializeField] private float tiempo= 5f;
    [SerializeField] private float radio = 5f;


    private float cooldown = 0f;

    float tiempoActual = 0f;



    
    private void Update()
    {
        if(Input.GetMouseButton(0) && !_seguro.bloqueado && cooldown <= 0f)
        {
            _seguro.anim.SetTrigger("biblia");
            //Cuenta regresiva cuando se activa el ataque
            tiempoActual += Time.deltaTime;
            if(tiempoActual >= tiempo)
            {
                //Ataque y cooldown
                cooldown = 10f;
                Ataque();
                tiempoActual = 0f;
                
            }

        }
        else
        { 
            tiempoActual = 0f;
            
        }

        if(cooldown > 0f)
        {
            Debug.Log("Enfriando");
            cooldown -= Time.deltaTime;
        }

        
        
    }

    private void Ataque()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radio);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out EnemyKormosManager enemyState))
            {
                enemyState.enemyMachine.ApplyDamage(50);
                Debug.Log("enemyattack");
            }
        }
    }

    public void InteractObject()
    {
        FindObjectOfType<SlotSelector>().CollectObject(this.gameObject);
    }


}
