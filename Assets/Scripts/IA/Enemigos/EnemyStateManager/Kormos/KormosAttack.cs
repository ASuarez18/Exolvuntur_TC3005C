using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;


/// <summary>
/// El estado de ataque se mantiene siempre y cuando el juagador se encuentre en el area del enemigo.
/// El estado conecta con : 
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

        public override void EnterState()
        {
            manager.agent.SetDestination(kormosSM.actualTarget);
        }

        public override void UpdateState()
        {
            //Verificamos si existe alguna condicion del siguiente estado
            GetNextState();
        }

        public override void ExitState()
        {
        }

        public override KormosStateMachine.EnemyState GetNextState()
        {
            //Si el jugador salio del area cambiamos al estado Idle
            if (!kormosSM.PlayerOnAreaClose)
            {
                return KormosStateMachine.EnemyState.Caution;
            }

            return KormosStateMachine.EnemyState.Attack;
        }

        //Funciones de sensores

        public override void OnAreaEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                kormosSM.actualTarget = other.gameObject.transform.position;
            }
        }

        public override void OnAreaStay(Collider other)
        {
            //Verificamos si alguien esta en el area del enemigo
            if (other.CompareTag("Player"))
            {

                kormosSM.actualTarget = other.gameObject.transform.position;
            }
        }

        public override void OnAreaExit(Collider other)
        {
            kormosSM.PlayerOnAreaClose = false;
        }


    }

}