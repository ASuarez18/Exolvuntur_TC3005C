using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;

/// <summary>
/// Estado de ataque se activa cuando el enemigo detecta una colision con el jugador.
/// Se ejecuta cada ve que entra en colision con el jugador.
/// Estados con los que conecta: Chasing, Stunned.
/// </summary>

namespace Enemy.Behaviour
{
    public class DybbukAttack : BaseState<DybbukStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
        private EnemyDybbukManager manager;
        private DybbukStateMachine dybbukSM;

        //Constructor del estado (Manager y Machine)
        public DybbukAttack(EnemyDybbukManager manager,DybbukStateMachine machine) : base(DybbukStateMachine.EnemyState.Attack)
        {
            this.manager = manager;
            this.dybbukSM = machine;
        }

         //Inicializa el estado
        public override void EnterState()
        {
            manager.animator.SetTrigger("Attack");
            if(!PhotonNetwork.IsMasterClient) return;
            //Detenemos el movimiento del agente y activamos sus animaciones
            manager.agent.isStopped = true;
            manager.Animatorfuc("Attack");
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            //Revisamos el contador de tiempo en que ejecuta el ataque
            dybbukSM.UpdateAttackTime();

        }

        public override void ExitState()
        { 
            if(!PhotonNetwork.IsMasterClient) return;
            //Reanudamos el movimiento del agente
            manager.agent.isStopped = false;
            dybbukSM.TimeOfAttack = 0f;
            dybbukSM.Attacking = false;
        }

         //Funcion que revisa si entra en el flujo de un estado o no
        public override DybbukStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return DybbukStateMachine.EnemyState.Attack;
            //Revisamos si el enemigo esta a la vista
            if(dybbukSM.IsStunned)
            {
                return DybbukStateMachine.EnemyState.Stunned;
            }
            else if(dybbukSM.TimeOfAttack >= 2f)
            {
                return DybbukStateMachine.EnemyState.Chasing;
            }
            return DybbukStateMachine.EnemyState.Attack;
        }

        // //Metodos de cambio de flujo del estado
        // public override void OnAreaEnter(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return;
        //     if(other.CompareTag("Player"))
        //     {
        //         dybbukSM.PlayerOnAreaClose = true;
        //         dybbukSM.PlayerPosition = other.transform.position;
        //         dybbukSM.PlayerGameObject = other.gameObject;
        //     }
        // }

        // public override void OnAreaStay(Collider other)
        // {
        //     //No realizamos nada
        // }

        // public override void OnAreaExit(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return;
        //     if(other.CompareTag("Player"))
        //     {
        //         dybbukSM.PlayerOnAreaClose = false;
        //         dybbukSM.PlayerPosition = Vector3.zero;
        //         dybbukSM.PlayerGameObject = null;
        //     }
        // }   
    }
}