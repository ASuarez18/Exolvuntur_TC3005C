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
    public class DybbukIdle : BaseState<DybbukStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
        private EnemyDybbukManager manager;
        private DybbukStateMachine dybbukSM;

        //Constructor del estado (Manager y Machine)
        public DybbukIdle(EnemyDybbukManager manager,DybbukStateMachine machine) : base(DybbukStateMachine.EnemyState.Idle)
        {
            this.manager = manager;
            this.dybbukSM = machine;
        }

         //Inicializa el estado
        public override void EnterState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            //Elegimos un destino aleatorio de la lista de puntos y lo guardamos
            dybbukSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
            manager.animator.SetFloat("States",1);
            
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            //Elegimos un nuevo destino cada ve que se acerca a su destino
            if (manager.agent.remainingDistance <= 1f)
            {  
                
                dybbukSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
               
                while(dybbukSM.actualTarget == manager.agent.destination)
                {
                    dybbukSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
                }

                //Actualizamos la posicion del agente al destino
                manager.agent.SetDestination(dybbukSM.actualTarget);
            }
        
        }

        public override void ExitState()
        {
            //No realizamos nada
        }

         //Funcion que revisa si entra en el flujo de un estado o no
        public override DybbukStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return DybbukStateMachine.EnemyState.Idle;
            if(dybbukSM.IsStunned)
            {
                return DybbukStateMachine.EnemyState.Stunned;
            }
            else if(dybbukSM.actorViews.ContainsValue(true))
            {
                return DybbukStateMachine.EnemyState.Still;
            }
            else if(dybbukSM.PlayerOnAreaClose)
            {
                return DybbukStateMachine.EnemyState.Chasing;
            }
            return DybbukStateMachine.EnemyState.Idle;
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
            
            //No realizamos nada
        }
    }
}