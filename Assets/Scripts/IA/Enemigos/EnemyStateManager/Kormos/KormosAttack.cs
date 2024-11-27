using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using PlayerController.PUN;
using Photon.Pun;


/// <summary>
/// El estado de ataque se mantiene siempre y cuando el juagador se encuentre en el area del enemigo.
/// El estado conecta con : KormosCaution
/// </summary>

namespace Enemy.Behaviour
{
    public class KormosAttack : BaseState<KormosStateMachine.EnemyState>
    {
        //Llamamos a nuestro controlador del enemigo
        private EnemyKormosManager manager;
        private KormosStateMachine kormosSM;

        //Creamos el constructor
        public KormosAttack(EnemyKormosManager manager,KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Attack)
        {
            this.manager = manager;
            this.kormosSM = machine;
        }

        //Inicializamos el estado
        public override void EnterState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            manager.agent.isStopped = true;
            manager.animator.SetTrigger("ataque");
            //Generamos un overslpa sphere para detectar game objets con el tag player justo frente del nuestro transform
            Collider[] hitColliders = Physics.OverlapSphere(new Vector3(manager.transform.position.x,manager.transform.position.y,manager.transform.position.z + 2), 5f);
            foreach (var hitCollider in hitColliders)
            {
                if(hitCollider.gameObject.tag == "Player")
                {
                    hitCollider.GetComponent<PUNPlayerSanity>().TakeDamage(10, "Kormos");
                }
            }
        }

        //Actualizamos el estado en el Update
        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            kormosSM.UpdateAttackTime();
        }

        //Salimos del estado
        public override void ExitState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            manager.agent.isStopped = false;
            kormosSM.TimeOfAttack = 0f;
        }

        //Obtenemos el siguiente estado segun las condiciones
        public override KormosStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return KormosStateMachine.EnemyState.Attack;
            if(kormosSM.IsStunned)
            {
                return KormosStateMachine.EnemyState.Stunned;
            }
            else if (kormosSM.TimeOfAttack >= 2f)
            {
                return KormosStateMachine.EnemyState.Chasing;
            }
            return KormosStateMachine.EnemyState.Attack;
        }

        //Funciones de sensores

        public override void OnAreaEnter(Collider other)
        {
            
        }

        public override void OnAreaStay(Collider other)
        {
            if(!PhotonNetwork.IsMasterClient) return;
            if(other.gameObject.tag == "Player")
            {
                kormosSM.PlayerPosition = other.transform.position;
                kormosSM.PlayerGameObject = other.gameObject;
            }
        }

        public override void OnAreaExit(Collider other)
        {
            
        }


    }

}