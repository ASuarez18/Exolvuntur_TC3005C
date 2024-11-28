using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;


namespace Enemy.Behaviour 
{

    public class KormosHeal : BaseState<KormosStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
            private EnemyKormosManager manager;
            private KormosStateMachine kormosSM;

            //Constructor del estado (Manager y Machine)
            public KormosHeal(EnemyKormosManager manager,KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Heal)
            {
                this.manager = manager;
                this.kormosSM = machine;
            }

            //Inicializa el estado
            public override void EnterState()
            {
                if(!PhotonNetwork.IsMasterClient) return;
                //Detenemos el movimiento del agente dado que esta en stunneado
                manager.agent.isStopped = true;

                //Inicializamos la animacion de curacion
                //Debug.Log("heal");
                manager.animator.SetFloat("forward", 0);
        }

            //Actualiza el estado en el Update del MonoBehaviour
            public override void UpdateState()
            {
                if(!PhotonNetwork.IsMasterClient) return;
                kormosSM.HealOverTime();

            }

            public override void ExitState()
            {
                if(!PhotonNetwork.IsMasterClient) return;
                // Se permite que el enemigo vuelva a moverse al terminarse el estado de aturdimiento
                manager.agent.isStopped = false;
            }

            //Funcion que revisa si entra en el flujo de un estado o no
            public override KormosStateMachine.EnemyState GetNextState()
            {
        
                if(!PhotonNetwork.IsMasterClient) return KormosStateMachine.EnemyState.Heal;

                if (kormosSM.currentHealth >= manager.enemyStats.Health)
                {
                    return KormosStateMachine.EnemyState.Idle;
                }
                return KormosStateMachine.EnemyState.Heal;
                
            }

            //Metodos de cambio de flujo del estado
            // public override void OnAreaEnter(Collider other)
            // {
                    
            // }

            // public override void OnAreaStay(Collider other)
            // {
            
            // }

            // public override void OnAreaExit(Collider other)
            // {
            //     if(!PhotonNetwork.IsMasterClient) return;

            //     if (other.gameObject.tag == "Player")
            //     {
            //         kormosSM.PlayerOnAreaFar = false;
            //     }
            // }
    }
}