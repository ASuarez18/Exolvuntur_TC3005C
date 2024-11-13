using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;


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
            manager.agent.SetDestination(kormosSM.actualTarget);
        }

        //Actualizamos el estado en el Update
        public override void UpdateState()
        {
            GetNextState();
        }

        //Salimos del estado
        public override void ExitState()
        {
        }

        //Obtenemos el siguiente estado segun las condiciones
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