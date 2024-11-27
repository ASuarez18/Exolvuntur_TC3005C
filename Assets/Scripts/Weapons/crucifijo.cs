using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using PlayerController.Inventory;
using Enemy.Manager;
using Enemy.Behaviour;
public class crucifijo : MonoBehaviour, IInteraction
{
    [SerializeField] private Seguro _seguro;

    public GameObject cubo;

    [SerializeField] private float rango = 2f;

    private float cooldown = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && !_seguro.bloqueado && cooldown <= 0f)
        {

            _seguro.anim.SetTrigger("crucifijo");
            Collider[] colliders = Physics.OverlapCapsule(transform.position, transform.position + Camera.main.transform.forward * rango, 2f);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out EnemyKormosManager enemyState))
                { 
                    enemyState.ApplyDamageRemote(20);
                    Debug.Log("enemyattack");
                }
                if (collider.TryGetComponent(out EnemyDybbukManager enemyState2))
                {
                    enemyState2.ApplyDamageRemote(20);
                    Debug.Log("enemyattack");
                }
                if (collider.TryGetComponent(out EnemySkinWalkerManager enemyState3))
                {
                    enemyState3.ApplyDamageRemote(20);
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
