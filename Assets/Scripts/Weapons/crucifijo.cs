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

    void Update()
    {
        if (Input.GetMouseButton(0) && !_Seguro.bloqueado)
        {

            Collider[] colliders = Physics.OverlapCapsule(point.position, point.position + Camera.main.transform.forward * rango, 2f);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Debug.Log("Atacando");
                }
            }
        }

    }

    public void InteractObject()
    {
        FindObjectOfType<SlotSelector>().CollectObject(this.gameObject);
    }


}
