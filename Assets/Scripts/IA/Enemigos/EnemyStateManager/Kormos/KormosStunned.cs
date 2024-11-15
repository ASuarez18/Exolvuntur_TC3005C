using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

namespace Enemy.Behaviour
{
    public class KormosStunned : BaseState<KormosStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
        private EnemyKormosManager manager;
        private KormosStateMachine kormosSM;

        //Constructor del estado (Manager y Machine)
        public KormosStunned(EnemyKormosManager manager,KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Stunned)
        {
            this.manager = manager;
            this.kormosSM = machine;
        }

        //Inicializa el estado
        public override void EnterState()
        {
            //Detenemos el movimiento del agente dado que esta en stunneado
            manager.agent.isStopped = true;
            // TODO: Settear animacion de aturdimiento
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            kormosSM.UpdateStunTime();
            //Revisamos su siguientes estados
            GetNextState();
        }

        public override void ExitState()
        {
            // Se permite que el enemigo vuelva a moverse al terminarse el estado de aturdimiento
            manager.agent.isStopped = false;
            //Reiniciamos la bandera de aturdimiento
            kormosSM.IsStunned = false;
            kormosSM.TimeStunned = 0f;
        }

        //Funcion que revisa si entra en el flujo de un estado o no
        public override KormosStateMachine.EnemyState GetNextState()
        {
            //*Revisamos si la bandera activa otro estado
            if (kormosSM.currentHealth <= 0)
            {
                // ! Manda a estado de Muerte
                return KormosStateMachine.EnemyState.Dead;
            }
            else if (kormosSM.TimeStunned >= 5) // TODO: Definir constante en propiedades o en codigo del tiempo de duracion de stun
            {
                if (kormosSM.currentHealth <= 30 && kormosSM.currentHealth > 0) // TODO: Definir en propiedades del enemigo la vida en la que ya cambiar a estado de huida
                {
                // ! Mandar a estado de Huida
                return KormosStateMachine.EnemyState.Scape;
                }   
                // ! Manda a estado de Alerta
                return KormosStateMachine.EnemyState.Caution;
            }
            // ! Se mantiene en stunned
            return KormosStateMachine.EnemyState.Stunned;
        }

        //Metodos de cambio de flujo del estado
        public override void OnAreaEnter(Collider other)
        {
                
        }

        public override void OnAreaStay(Collider other)
        {
        
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
