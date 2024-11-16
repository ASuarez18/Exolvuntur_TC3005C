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
            Debug.Log("Atacando a la verga");
            //TODO: Animacion de ataque
            manager.agent.isStopped = true;
            manager.animator.SetTrigger("ataque");
        }

        //Actualizamos el estado en el Update
        public override void UpdateState()
        {
           kormosSM.UpdateAttackTime();
            GetNextState();
        }

        //Salimos del estado
        public override void ExitState()
        {
            manager.agent.isStopped = false;
            kormosSM.TimeOfAttack = 0f;
        }

        //Obtenemos el siguiente estado segun las condiciones
        public override KormosStateMachine.EnemyState GetNextState()
        {
            if(kormosSM.IsStunned)
            {
                // ! Fue stuneado
                return KormosStateMachine.EnemyState.Stunned;
            }
            else if (kormosSM.TimeOfAttack >= 2) //TODO: eliminar constante magica de tiempo de ataque
            {
                // ! Se mantiene en ataque hasta que termine la duracion de esta
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