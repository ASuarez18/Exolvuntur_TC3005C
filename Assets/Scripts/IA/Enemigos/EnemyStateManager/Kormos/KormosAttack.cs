using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
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
            else if (kormosSM.TimeOfAttack >= 2)
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
            
        }

        public override void OnAreaExit(Collider other)
        {
            
        }


    }

}