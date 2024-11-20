using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

/// <summary>
/// Estado de Patrullaje: Aqui el estado del enemigo Dybbuk , se encarga de patrullar diferentes puntos.
/// Cuando llega a un punto elige otro de manera aleatoria.
/// </summary>

namespace Enemy.Behaviour
{
    public class DybbukStill : BaseState<DybbukStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
        private EnemyDybbukManager manager;
        private DybbukStateMachine dybbukSM;

        //Constructor del estado (Manager y Machine)
        public DybbukStill(EnemyDybbukManager manager,DybbukStateMachine machine) : base(DybbukStateMachine.EnemyState.Still)
        {
            this.manager = manager;
            this.dybbukSM = machine;
        }

         //Inicializa el estado
        public override void EnterState()
        {
            //Detenemos el movimeinto del agente
            manager.agent.isStopped = true;
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            dybbukSM.UpdateAgressiveCounter();
        }

        public override void ExitState()
        { 
            //Reanudamos el movimiento del agente
            manager.agent.isStopped = false;
            dybbukSM.AgressiveCounter = 0f;

        }

         //Funcion que revisa si entra en el flujo de un estado o no
        public override DybbukStateMachine.EnemyState GetNextState()
        {
            //Revisamos si el enemigo esta a la vista
            if(dybbukSM.IsStunned)
            {
                return DybbukStateMachine.EnemyState.Stunned;
            }
            else if(!dybbukSM.OnView)
            {
                return DybbukStateMachine.EnemyState.Idle;
            }
            else if(dybbukSM.AgressiveCounter >= dybbukSM.AgroTime && dybbukSM.AggresiveMode <= 2)
            {
                return DybbukStateMachine.EnemyState.Aggresive;
            }
            return DybbukStateMachine.EnemyState.Still;
        }

        //Metodos de cambio de flujo del estado
        public override void OnAreaEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                dybbukSM.PlayerOnAreaClose = true;
            }
        }

        public override void OnAreaStay(Collider other)
        {
            //No realizamos nada
        }

        public override void OnAreaExit(Collider other)
        {
            
            if(other.CompareTag("Player"))
            {
                dybbukSM.PlayerOnAreaClose = false;
            }
        }
    }
}