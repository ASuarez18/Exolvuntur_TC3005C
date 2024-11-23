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
    public class SkinwalkerStunned : BaseState<SkinwalkerStateMachine.EnemyState>
    {
        //Referencia hacia el manager y la maquina de estados del enemigo
        private EnemySkinWalkerManager manager;
        private SkinwalkerStateMachine skinwalkerSM;

        //Constructor del estado (Manager y Machine)
        public SkinwalkerStunned(EnemySkinWalkerManager manager, SkinwalkerStateMachine machine) : base(SkinwalkerStateMachine.EnemyState.Stunned)
        {
            this.manager = manager;
            this.skinwalkerSM = machine;
        }

        public override void EnterState()
        {
            //Detenemos el movimiento del enemigo
            manager.agent.isStopped = true;
            manager.animator.SetTrigger("Stunning");
            manager.animator.SetBool("Stun", true);
        }

        public override void UpdateState()
        {
             skinwalkerSM.UpdateStunTime();
             //Revisamos la funcion de ya esta transformado
            if(skinwalkerSM.IsTransformed)
            {
                skinwalkerSM.UpdateTimeTransform();
            }
        }

        public override void ExitState()
        {
            //Detenemos el movimiento del enemigo
            manager.animator.SetBool("Stun", false);
            manager.agent.isStopped = false;
            skinwalkerSM.IsStunned = false;
            skinwalkerSM.TimeStunned = 0f;
        }

        //Función para cambiar de estados
        public override SkinwalkerStateMachine.EnemyState GetNextState()
        {
            //Revisamos si el enemigo sigue stunned
            if(skinwalkerSM.currentHealth <= 0)
            {
                return SkinwalkerStateMachine.EnemyState.Dead;
            }
            else if(skinwalkerSM.TimeStunned >= 10f)
            {
                return  SkinwalkerStateMachine.EnemyState.Idle; 
            }
    
            return SkinwalkerStateMachine.EnemyState.Stunned;
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