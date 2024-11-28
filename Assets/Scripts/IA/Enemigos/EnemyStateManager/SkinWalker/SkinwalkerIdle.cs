using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;

/// <summary>
/// En este estado nuestor enemigo SkinWalker patrulla alrededor del mapa.
/// Elige un punto aleatorio del mapa y se dirige hacia el.
/// Cuando llega al punto elige otro al azar.
/// Estados con los que conecta: Search, Transform.
/// </summary>

namespace Enemy.Behaviour
{
    public class SkinwalkerIdle : BaseState<SkinwalkerStateMachine.EnemyState>
    {
        //Referencia hacia el manager y la maquina de estados del enemigo
        private EnemySkinWalkerManager manager;
        private SkinwalkerStateMachine skinwalkerSM;

        //Constructor del estado (Manager y Machine)
        public SkinwalkerIdle(EnemySkinWalkerManager manager, SkinwalkerStateMachine machine) : base(SkinwalkerStateMachine.EnemyState.Idle)
        {
            this.manager = manager;
            this.skinwalkerSM = machine;
        }

        public override void EnterState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            //Elegimos un destino al azar
            skinwalkerSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
            manager.animator.SetFloat("Move",1f);
        }

        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            //Ejecutamos la funcion o el contador para activar la busqueda
            skinwalkerSM.UpdateSearchTimer();

            //Verificamos nuestro patrullaje
            if (manager.agent.remainingDistance <= 1f)
            {  
                
                skinwalkerSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
                
                while(skinwalkerSM.actualTarget == manager.agent.destination)
                {
                    skinwalkerSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
                }

                //Actualizamos la posicion del agente al destino
                manager.agent.SetDestination(skinwalkerSM.actualTarget);
            }

            //Si el enemigo no esta transformado y el jugador esta cerca pasamos a transform
            if(!skinwalkerSM.IsTransformed)
            {
                //Actualizamos el area del enemigo hacia el jugador (DistanceToPlayer)
                skinwalkerSM.DistanceToPlayer = Vector3.Distance(manager.transform.position,skinwalkerSM.PlayerPosition);
                // Debug.Log(skinwalkerSM.DistanceToPlayer + " Idle");
                Debug.Log(manager.transform.name + " " + skinwalkerSM.PlayerPosition);
                //Verificamos si esta muy cerca el jugador

                if(skinwalkerSM.DistanceToPlayer <= skinwalkerSM.currentAttackRange)
                {
                    skinwalkerSM.PlayerOnAreaClose = true;
                }
            }

            //Revisamos la funcion de ya esta transformado
            if(skinwalkerSM.IsTransformed)
            {
                skinwalkerSM.UpdateTimeTransform();
            }
        }

        public override void ExitState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            //Reiniciamos la banderas o contadores
            skinwalkerSM.SearchCounter = 0f;
        }

        //Función para cambiar de estados
        public override SkinwalkerStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return SkinwalkerStateMachine.EnemyState.Idle;
            //Verificamos si esta estuneado
            if(skinwalkerSM.IsStunned)
            {
                return SkinwalkerStateMachine.EnemyState.Stunned;
            }
            //Verificamos si ya el contador llego a 100 segundos
            else if(skinwalkerSM.SearchCounter >= 50)
            {
                return SkinwalkerStateMachine.EnemyState.Search;
            }
            //Si el jugador se acerca a la zona de ataque pasamos a Transform
            else if(skinwalkerSM.PlayerOnAreaClose && !skinwalkerSM.IsTransformed)
            {
                return SkinwalkerStateMachine.EnemyState.Transform;
            }
            //Si el enemigo ya esta transformado y entro a la zona de ataque lo perseguimos
            else if(skinwalkerSM.PlayerOnAreaFar && skinwalkerSM.IsTransformed)
            {
                return SkinwalkerStateMachine.EnemyState.Chasing;
            }
            return SkinwalkerStateMachine.EnemyState.Idle;
        }

        //Metodos de cambio de flujo del estado
        public override void OnAreaEnter(Collider other)
        {
            if(!PhotonNetwork.IsMasterClient) return;
            if(other.CompareTag("Player"))
            {
                skinwalkerSM.PlayerOnAreaFar = true;
                skinwalkerSM.PlayerPosition = other.transform.position;
                skinwalkerSM.PlayerGameObject = other.gameObject;
            }
        }

        public override void OnAreaStay(Collider other)
        {
            if(!PhotonNetwork.IsMasterClient) return ;
            skinwalkerSM.PlayerPosition = other.transform.position;
        }

        public override void OnAreaExit(Collider other)
        {
            if(!PhotonNetwork.IsMasterClient) return ;
            if(other.CompareTag("Player"))
            {
                skinwalkerSM.PlayerOnAreaFar = false;
            }
        }
    }

}