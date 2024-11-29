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
    public class DybbukAgressive : BaseState<DybbukStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
        private EnemyDybbukManager manager;
        private DybbukStateMachine dybbukSM;

        //Constructor del estado (Manager y Machine)
        public DybbukAgressive(EnemyDybbukManager manager,DybbukStateMachine machine) : base(DybbukStateMachine.EnemyState.Aggresive)
        {
            this.manager = manager;
            this.dybbukSM = machine;
        }

         //Inicializa el estado
        public override void EnterState()
        {   
            if(!PhotonNetwork.IsMasterClient) return;

            //Detenemos el movimiento del agente
            manager.agent.isStopped = true;

            //Incrementamos las estadisticas del enemigo en la maquina de estados un 20%
            dybbukSM.currentSpeed += dybbukSM.currentRunningSpeed;
            dybbukSM.currentAcceleration += dybbukSM.currentAcceleration;
            dybbukSM.currentAttackRange += (dybbukSM.currentAttackRange * .20f);
            dybbukSM.currentHealth += (dybbukSM.currentHealth * .20f);

            //Incrementamos las estadisticas del enemigo en el agente del manager
            manager.agent.speed = dybbukSM.currentSpeed;
            manager.agent.acceleration = dybbukSM.currentAcceleration;
            manager.areaAlerta.radius = dybbukSM.currentAttackRange;

            //Incrementamos las veces que puede entrar en este modo
            dybbukSM.AggresiveMode++;

            // dybbukSM.NearPlayer();

            // if(dybbukSM.PlayerOnAreaClose)
            // {
            //     //Buscamos el jugadro mas cercano y lo perseguimos
            //     dybbukSM.OnView = false;
            //     manager.agent.SetDestination(dybbukSM.actualTarget);
            // }

            
            manager.animator.SetFloat("States",3);

            
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            // dybbukSM.OnView = false;
            // //Actulizamos el destino directamente al juagador
            // manager.agent.SetDestination(dybbukSM.actualTarget);
            //Obtenemos la funcion de Update Aggresive Counter
            dybbukSM.UpdateAggressiveDuration();
            // dybbukSM.NearPlayer();
            
        }

        public override void ExitState()
        { 
            if(!PhotonNetwork.IsMasterClient) return;
            dybbukSM.AggresiveDuration = 0f;
            manager.agent.isStopped = false;
        }

         //Funcion que revisa si entra en el flujo de un estado o no
        public override DybbukStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return DybbukStateMachine.EnemyState.Aggresive;
            //Revisamos si el enemigo esta a la vista
            if(dybbukSM.IsStunned)
            {
                return DybbukStateMachine.EnemyState.Stunned;
            }
            else if(dybbukSM.AggresiveDuration >= dybbukSM.AgroDuration)
            {
                return DybbukStateMachine.EnemyState.Chasing;
            }
            else if(dybbukSM.Attacking)
            {
                return DybbukStateMachine.EnemyState.Attack;
            }
            return DybbukStateMachine.EnemyState.Aggresive;
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
        //     if(!PhotonNetwork.IsMasterClient) return;
        //      if(other.CompareTag("Player"))
        //     {
        //         dybbukSM.actualTarget = other.transform.position;
        //     }
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