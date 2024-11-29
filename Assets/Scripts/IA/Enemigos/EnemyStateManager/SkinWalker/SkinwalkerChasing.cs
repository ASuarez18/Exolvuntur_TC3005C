using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using PlayerController.PUN;
using Photon.Pun;

/// <summary>
/// En este estado nuestor enemigo SkinWalker patrulla alrededor del mapa.
/// Elige un punto aleatorio del mapa y se dirige hacia el.
/// Cuando llega al punto elige otro al azar.
/// Estados con los que conecta: Search, Transform.
/// </summary>

namespace Enemy.Behaviour
{
    public class SkinwalkerChasing : BaseState<SkinwalkerStateMachine.EnemyState>
    {
        //Referencia hacia el manager y la maquina de estados del enemigo
        private EnemySkinWalkerManager manager;
        private SkinwalkerStateMachine skinwalkerSM;

        //Constructor del estado (Manager y Machine)
        public SkinwalkerChasing(EnemySkinWalkerManager manager, SkinwalkerStateMachine machine) : base(SkinwalkerStateMachine.EnemyState.Chasing)
        {
            this.manager = manager;
            this.skinwalkerSM = machine;
        }

        public override void EnterState()
        {
            
        }

        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return ;

            // skinwalkerSM.DistanceToPlayer = Vector3.Distance(manager.transform.position,skinwalkerSM.PlayerPosition);
            // skinwalkerSM.actualTarget = skinwalkerSM.PlayerPosition;
            // manager.agent.SetDestination(skinwalkerSM.actualTarget);
            // if(skinwalkerSM.DistanceToPlayer <= 20)
            // {
            //     skinwalkerSM.Attacking = true;
            // }

            skinwalkerSM.ViewOnAreaFarPlayers();
            skinwalkerSM.NearPlayer();

            //Verificamos la funcion de tiempo transformado
            if(skinwalkerSM.IsTransformed) 
            {
                skinwalkerSM.UpdateTimeTransform();
            }
        }

        public override void ExitState()
        {
            
        }

        //Función para cambiar de estados
        public override SkinwalkerStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return SkinwalkerStateMachine.EnemyState.Chasing;
        //Verificamos si esta estuneado
            if(skinwalkerSM.IsStunned)
            {
                return SkinwalkerStateMachine.EnemyState.Stunned;
            }
            //Si el jugador se aleja de la zona de ataque pasamos a Search
            else if(!skinwalkerSM.PlayerOnAreaFar)
            {
                return SkinwalkerStateMachine.EnemyState.Idle;
            }
            //Si el enemigo detecto una colision
            else if(skinwalkerSM.Attacking)
            {
                // skinwalkerSM.PlayerGameObject.GetComponent<PUNPlayerSanity>().TakeDamage(10, "Skinwalker");
                return SkinwalkerStateMachine.EnemyState.Attack;
            }
            
            return SkinwalkerStateMachine.EnemyState.Chasing; 
        }

        // //Metodos de cambio de flujo del estado
        // public override void OnAreaEnter(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return;
        //     if(other.CompareTag("Player"))
        //     {
        //         skinwalkerSM.PlayerOnAreaFar = true;
        //     }
        // }

        // public override void OnAreaStay(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return;
            
        //     if(other.CompareTag("Player"))
        //     {
        //         skinwalkerSM.actualTarget = other.transform.position;
        //     }
        // }

        // public override void OnAreaExit(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return;
        //     if(other.CompareTag("Player"))
        //     {
        //         skinwalkerSM.PlayerOnAreaFar = false;
        //     }
        // }
    }

}