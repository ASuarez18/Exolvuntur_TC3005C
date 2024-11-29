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
    public class DybbukStunned : BaseState<DybbukStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
        private EnemyDybbukManager manager;
        private DybbukStateMachine dybbukSM;

        //Constructor del estado (Manager y Machine)
        public DybbukStunned(EnemyDybbukManager manager,DybbukStateMachine machine) : base(DybbukStateMachine.EnemyState.Stunned)
        {
            this.manager = manager;
            this.dybbukSM = machine;
        }

         //Inicializa el estado
        public override void EnterState()
        {
            manager.animator.SetTrigger("Stunning");
            if(!PhotonNetwork.IsMasterClient) return;
            //Detenemos el movimeinto del agente
            manager.agent.isStopped = true;
            //manager.Animatorfuc("Stunning");
            manager.animator.SetBool("Stun",true);
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            dybbukSM.UpdateStunTime();
        }

        public override void ExitState()
        { 
            if(!PhotonNetwork.IsMasterClient) return;
            //Reanudamos el movimiento del agente
            manager.agent.isStopped = false;
            dybbukSM.IsStunned = false;
            dybbukSM.TimeStunned = 0f;
            manager.animator.SetBool("Stun",false);

        }

         //Funcion que revisa si entra en el flujo de un estado o no
        public override DybbukStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return DybbukStateMachine.EnemyState.Stunned;
            //Revisamos si el enemigo sigue stunned
            if(dybbukSM.currentHealth <= 0)
            {
                return DybbukStateMachine.EnemyState.Dead;
            }
            else if(dybbukSM.TimeStunned >= 10f)
            {
               
                if(dybbukSM.actorViews.ContainsValue(true))
                {
                    return DybbukStateMachine.EnemyState.Still;
                }
                else if(dybbukSM.currentHealth > 0 && dybbukSM.currentHealth <= 30)
                {
                    return DybbukStateMachine.EnemyState.Dissappear;
                }
                else
                {
                    return DybbukStateMachine.EnemyState.Idle;
                }
                
            }
            return DybbukStateMachine.EnemyState.Stunned;
        }

        //Metodos de cambio de flujo del estado
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