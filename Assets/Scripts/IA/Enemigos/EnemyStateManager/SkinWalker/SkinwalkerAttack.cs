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
    public class SkinwalkerAttack : BaseState<SkinwalkerStateMachine.EnemyState>
    {
        //Referencia hacia el manager y la maquina de estados del enemigo
        private EnemySkinWalkerManager manager;
        private SkinwalkerStateMachine skinwalkerSM;

        //Constructor del estado (Manager y Machine)
        public SkinwalkerAttack(EnemySkinWalkerManager manager, SkinwalkerStateMachine machine) : base(SkinwalkerStateMachine.EnemyState.Attack)
        {
            this.manager = manager;
            this.skinwalkerSM = machine;
        }

        public override void EnterState()
        {
            //Detenemos el movimiento del agente y activamos sus animaciones
            manager.agent.isStopped = true;
            manager.animator.SetTrigger("Attack");
        }

        public override void UpdateState()
        {
            //Revisamos el contador de tiempo en que ejecuta el ataque
            skinwalkerSM.UpdateAttackTime();
            //Revisamos la funcion de ya esta transformado
            if(skinwalkerSM.IsTransformed)
            {
                skinwalkerSM.UpdateTimeTransform();
            }
        }

        public override void ExitState()
        {
            //Reanudamos el movimiento del agente
            manager.agent.isStopped = false;
            skinwalkerSM.TimeOfAttack = 0f;
            skinwalkerSM.Attacking = false;
        }

        //Función para cambiar de estados
        public override SkinwalkerStateMachine.EnemyState GetNextState()
        {
            //Revisamos si esta estuneado el enemigo
            if(skinwalkerSM.IsStunned)
            {
                return SkinwalkerStateMachine.EnemyState.Stunned;
            }
            //Revisamos si ya dejo de atacar
            else if(skinwalkerSM.TimeOfAttack >= 2f)
            {
                return SkinwalkerStateMachine.EnemyState.Chasing;
            }

            return SkinwalkerStateMachine.EnemyState.Attack;
        }

        //Metodos de cambio de flujo del estado
        public override void OnAreaEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                skinwalkerSM.PlayerOnAreaFar = true;
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