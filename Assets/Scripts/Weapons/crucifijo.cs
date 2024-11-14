using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using PlayerController.Inventory;
public class crucifijo : MonoBehaviour, IInteraction
{
    [SerializeField] private Seguro _Seguro;
    public GameObject cubo;
    public Transform point;

    [SerializeField] private float rango = 2f;

    private float cooldown = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && !_Seguro.bloqueado && cooldown <= 0f)
        {

            Collider[] colliders = Physics.OverlapCapsule(point.position, point.position + Camera.main.transform.forward * rango, 2f);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Debug.Log("Atacando");
                }
            }

            cooldown = 3f;
        }

        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }

    }

    public void InteractObject()
    {
        FindObjectOfType<SlotSelector>().CollectObject(this.gameObject);
    }


}
