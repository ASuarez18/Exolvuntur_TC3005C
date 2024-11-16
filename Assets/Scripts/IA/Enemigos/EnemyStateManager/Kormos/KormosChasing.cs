using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

namespace Enemy.Behaviour
{
    public class KormosChasing : BaseState<KormosStateMachine.EnemyState>
    {
        //Llamamos a nuestro controlador del enemigo
        private EnemyKormosManager manager;
        private KormosStateMachine kormosSM;

        //Creamos el constructor
        public KormosChasing(EnemyKormosManager manager,KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Chasing)
        {
            this.manager = manager;
            this.kormosSM = machine;
        }

        //Inicializamos el estado
        public override void EnterState()
        {
            manager.agent.SetDestination(kormosSM.actualTarget);

            //Inicializamos la animacion de persecucion
            manager.animator.SetFloat("forward", 2f);
        }

        //Actualizamos el estado en el Update
        public override void UpdateState()
        {
            //Verificamos que el jugador esta en nuestra area de ataque o cercana
            manager.agent.SetDestination(kormosSM.actualTarget);
            kormosSM.DistanceToPlayer = Vector3.Distance(manager.transform.position,kormosSM.actualTarget);

            if (kormosSM.DistanceToPlayer > kormosSM.currentAttackRange)
            {
                kormosSM.PlayerOnAreaClose = false;
            }
            GetNextState();
        }

        //Salimos del estado
        public override void ExitState()
        {
            kormosSM.Attacking = false;
        }

        //Obtenemos el siguiente estado segun las condiciones
        public override KormosStateMachine.EnemyState GetNextState()
        {
            //Si el jugador salio del area cambiamos al estado Idle
            if(kormosSM.IsStunned)
            {
                return KormosStateMachine.EnemyState.Stunned;
            }
            else if (!kormosSM.PlayerOnAreaClose)
            {
                return KormosStateMachine.EnemyState.Caution;
            }
            else if(kormosSM.Attacking)
            {
                return KormosStateMachine.EnemyState.Attack;
            }

            return KormosStateMachine.EnemyState.Chasing;
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
            if (other.gameObject.tag == "Player")
            {
                kormosSM.PlayerOnAreaFar = false;
            }
        }

    }
}
