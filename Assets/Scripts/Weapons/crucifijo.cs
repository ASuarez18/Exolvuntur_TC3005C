using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using PlayerController.Inventory;
using Enemy.Manager;
using Enemy.Behaviour;
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
                if (collider.TryGetComponent(out EnemyKormosManager enemyState))
                { 
                    enemyState.enemyMachine.ApplyDamage(10);
                    Debug.Log("enemyattack");
                }
            }

            cooldown = 3f;
        }

        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }

        if(cooldown < 0f)
        { cooldown = 0f; }
        

    }

    public void InteractObject()
    {
        FindObjectOfType<SlotSelector>().CollectObject(this.gameObject);
    }


}
