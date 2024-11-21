using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

/// <summary>
/// En este estado nuestor enemigo SkinWalker patrulla alrededor del mapa.
/// Elige un punto aleatorio del mapa y se dirige hacia el.
/// Cuando llega al punto elige otro al azar.
/// Estados con los que conecta: Search, Transform.
/// </summary>

namespace Enemy.Behaviour
{
    public class SkinwalkerSearch : BaseState<SkinwalkerStateMachine.EnemyState>
    {
        //Referencia hacia el manager y la maquina de estados del enemigo
        private EnemySkinWalkerManager manager;
        private SkinwalkerStateMachine skinwalkerSM;
        private bool OnDestination = false;

        //Constructor del estado (Manager y Machine)
        public SkinwalkerSearch(EnemySkinWalkerManager manager, SkinwalkerStateMachine machine) : base(SkinwalkerStateMachine.EnemyState.Search)
        {
            this.manager = manager;
            this.skinwalkerSM = machine;
        }

        public override void EnterState()
        {
            //Realizamos una busqueda a objetos con el Tag player
            skinwalkerSM.actualTarget = GameObject.FindGameObjectWithTag("Player").transform.position;
            //Actualizamos la posicion del agente al destino
            manager.agent.SetDestination(skinwalkerSM.actualTarget);
        }

        public override void UpdateState()
        {
            //Verificamos que el enemigoi llego a la posicion del jugador
            if (manager.agent.remainingDistance <= 0.5f)
            {
                OnDestination = true;
            }

            //Si el enemigo no esta transformado y el jugador esta cerca pasamos a transform
            if(!skinwalkerSM.IsTransformed)
            {
                //Actualizamos el area del enemigo hacia el jugador (DistanceToPlayer)
                skinwalkerSM.DistanceToPlayer = Vector3.Distance(manager.transform.position,skinwalkerSM.PlayerPosition);
                Debug.Log(skinwalkerSM.DistanceToPlayer + " Search");
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
            OnDestination = false;
        }

        //Función para cambiar de estados
        public override SkinwalkerStateMachine.EnemyState GetNextState()
        {
            //Verificamos si esta estuneado
            if(OnDestination)
            {
                return SkinwalkerStateMachine.EnemyState.Idle;
            }
            else if(skinwalkerSM.IsStunned)
            {
                return SkinwalkerStateMachine.EnemyState.Stunned;
            }
            //Si el jugador se acerca a la zona de ataque pasamos a Transform
            else if(skinwalkerSM.PlayerOnAreaClose && !skinwalkerSM.IsTransformed)
            {
                return SkinwalkerStateMachine.EnemyState.Transform;
            }
            //Si esta en el area y esta transformado lo perseguimos
            else if(skinwalkerSM.PlayerOnAreaFar && skinwalkerSM.IsTransformed)
            {
                return SkinwalkerStateMachine.EnemyState.Chasing;
            }
            
            return SkinwalkerStateMachine.EnemyState.Search;
        }

        //Metodos de cambio de flujo del estado
        public override void OnAreaEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                skinwalkerSM.PlayerOnAreaFar = true;
                skinwalkerSM.PlayerPosition = other.transform.position;
            }
        }

        public override void OnAreaStay(Collider other)
        {
        }

        public override void OnAreaExit(Collider other)
        {
            
            if(other.CompareTag("Player"))
            {
                skinwalkerSM.PlayerOnAreaFar = false;
            }
        }
    }

}