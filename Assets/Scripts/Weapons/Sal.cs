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
    private SlotSelector slotSelector;

    private Vector3 escala;
    private float cooldown = 0f;

    private void Start()
    {
        escala = cubo.transform.localScale;
        slotSelector = FindObjectOfType<SlotSelector>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_seguro.bloqueado && cooldown <= 0f)
        {
            cubo.GetComponent<Rigidbody>().isKinematic = false;
            cubo.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Vector3 direccionLanzamiento = Camera.main.transform.forward;
            _seguro.anim.SetTrigger("sal");

            cubo.GetComponent<Rigidbody>().AddForce(direccionLanzamiento * fuerza, ForceMode.Impulse);

            cubo.transform.parent = null;
            cubo.GetComponent<Collider>().enabled = true;
            cubo.transform.localScale = escala;
            cubo.GetComponent<SpriteRenderer>().enabled = false;
            cooldown = 3f;

            if (slotSelector != null)
            {
                slotSelector.RemoveUtility("Salt");
            }

        }

        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }


    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("EnemyKormos"))
        {
            collision.collider.GetComponent<EnemyKormosManager>().StunActive();
        }
        if (collision.collider.CompareTag("EnemyDybbuk"))
        {
            collision.collider.GetComponent<EnemyDybbukManager>().StunActive();
        }
        if (collision.collider.CompareTag("EnemySkinwalker"))
        {
            collision.collider.GetComponent<EnemySkinWalkerManager>().StunActive();
        }

    }

    public void InteractObject()
    {
        //FindObjectOfType<SlotSelector>().CollectObject(this.gameObject);
    }

    public void InteractObject(GameObject player)
    {
        player.GetComponent<SlotSelector>().CollectObject(this.gameObject);
    }
}
