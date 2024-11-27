using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;

/// <summary>
/// EEstado de persecucion al jugador.
/// Este estado se activa cuando el jugador ingresa en el area mas cercana.
/// Estado con los que conecta: Idle, Still, Attack, Stunned.
/// </summary>

namespace Enemy.Behaviour
{
    public class DybbukChasing : BaseState<DybbukStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
        private EnemyDybbukManager manager;
        private DybbukStateMachine dybbukSM;

        //Constructor del estado (Manager y Machine)
        public DybbukChasing(EnemyDybbukManager manager,DybbukStateMachine machine) : base(DybbukStateMachine.EnemyState.Chasing)
        {
            this.manager = manager;
            this.dybbukSM = machine;
        }

         //Inicializa el estado
        public override void EnterState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            manager.animator.SetFloat("States",2);
           
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            //Actualizamos el movimiento hacia el jugador
            manager.agent.SetDestination(dybbukSM.actualTarget);
        }

        public override void ExitState()
        {

        }

         //Funcion que revisa si entra en el flujo de un estado o no
        public override DybbukStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return DybbukStateMachine.EnemyState.Chasing;
            //Revisamos si el enemigo esta a la vista
            if(dybbukSM.IsStunned)
            {
                return DybbukStateMachine.EnemyState.Stunned;
            }
            else if(dybbukSM.actorViews.ContainsValue(true))
            {
                return DybbukStateMachine.EnemyState.Still;
            }
            else if(dybbukSM.Attacking)
            {
                return DybbukStateMachine.EnemyState.Attack;
            }
            else if(!dybbukSM.PlayerOnAreaClose)
            {
                return DybbukStateMachine.EnemyState.Idle;
            }
            return DybbukStateMachine.EnemyState.Chasing;
        }

        //Metodos de cambio de flujo del estado
        public override void OnAreaEnter(Collider other)
        {
            if(!PhotonNetwork.IsMasterClient) return;
            if(other.CompareTag("Player"))
            {
                dybbukSM.PlayerOnAreaClose = true;
            }
        }

        public override void OnAreaStay(Collider other)
        {
            if(!PhotonNetwork.IsMasterClient) return;
            if(other.CompareTag("Player"))
            {
                dybbukSM.actualTarget = other.transform.position;
            }
        }

        public override void OnAreaExit(Collider other)
        {
            if(!PhotonNetwork.IsMasterClient) return;
            if(other.CompareTag("Player"))
            {
                dybbukSM.PlayerOnAreaClose = false;
            }
        }
    }
}