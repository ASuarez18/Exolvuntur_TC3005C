using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerController.Inventory;
using Interfaces;
using Enemy.Manager;

/// <summary>
/// this class is responsible for the logic of the salt weapon
/// only used when the lock is not active and the cooldown is 0
/// </summary>

public class Sal : MonoBehaviour, IInteraction
{
    [SerializeField] private Seguro _seguro;
    public GameObject cubo;
    public float fuerza;

    
    private Vector3 escala;
    private float cooldown = 0f;

    private void Start()
    {
        escala = cubo.transform.localScale;
        
    }

    void Update()
    {
        

        if(Input.GetMouseButtonDown(0) && !_seguro.bloqueado && cooldown <= 0f)
        {
                cubo.GetComponent<Rigidbody>().isKinematic = false;
                cubo.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Vector3 direccionLanzamiento = Camera.main.transform.forward;
                Debug.Log("ANIMACION SAL entrada");
                _seguro.anim.SetTrigger("sal");
                Debug.Log("ANIMACION SAL salida");

                cubo.GetComponent<Rigidbody>().AddForce(direccionLanzamiento * fuerza, ForceMode.Impulse);

                cubo.transform.parent = null;
                cubo.transform.localScale = escala;

                cooldown = 3f;

        }

        if(cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }

        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<EnemyKormosManager>().enemyMachine.ApplyStun();
        }
    }

    public void InteractObject()
    {
        FindObjectOfType<SlotSelector>().CollectObject(this.gameObject);
    }

    
}
