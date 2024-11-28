using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;

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
            if(!PhotonNetwork.IsMasterClient) return;
            //Detenemos el movimeinto del agente
            manager.agent.isStopped = true;
            manager.animator.SetFloat("States", 0f);
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            dybbukSM.UpdateAgressiveCounter();
        }

        public override void ExitState()
        { 
            if(!PhotonNetwork.IsMasterClient) return;
            //Reanudamos el movimiento del agente
            manager.agent.isStopped = false;
            dybbukSM.AgressiveCounter = 0f;

        }

         //Funcion que revisa si entra en el flujo de un estado o no
        public override DybbukStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return DybbukStateMachine.EnemyState.Still;
            //Revisamos si el enemigo esta a la vista
            if(dybbukSM.IsStunned)
            {
                return DybbukStateMachine.EnemyState.Stunned;
            }
            else if(!dybbukSM.actorViews.ContainsValue(true))
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
            if(!PhotonNetwork.IsMasterClient) return;
            if(other.CompareTag("Player"))
            {
                dybbukSM.PlayerOnAreaClose = true;
                dybbukSM.PlayerPosition = other.transform.position;
                dybbukSM.PlayerGameObject = other.gameObject;
            }
        }

        public override void OnAreaStay(Collider other)
        {
            //No realizamos nada
            
        }

        public override void OnAreaExit(Collider other)
        {
            if(!PhotonNetwork.IsMasterClient) return;   
            if(other.CompareTag("Player"))
            {
                dybbukSM.PlayerOnAreaClose = false;
                dybbukSM.PlayerPosition = Vector3.zero;
                dybbukSM.PlayerGameObject = null;
            }
        }
    }
}