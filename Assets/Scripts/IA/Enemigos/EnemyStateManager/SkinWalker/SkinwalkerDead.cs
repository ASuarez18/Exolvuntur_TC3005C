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
    public class SkinwalkerDead : BaseState<SkinwalkerStateMachine.EnemyState>
    {
        //Referencia hacia el manager y la maquina de estados del enemigo
        private EnemySkinWalkerManager manager;
        private SkinwalkerStateMachine skinwalkerSM;

        //Constructor del estado (Manager y Machine)
        public SkinwalkerDead(EnemySkinWalkerManager manager, SkinwalkerStateMachine machine) : base(SkinwalkerStateMachine.EnemyState.Dead)
        {
            this.manager = manager;
            this.skinwalkerSM = machine;
        }

        public override void EnterState()
        {
            manager.agent.isStopped = true;
            manager.animator.SetTrigger("Dead");
            skinwalkerSM.EnemyDead();
        }

        public override void UpdateState()
        {
             
        }

        public override void ExitState()
        {
            
        }

        //Función para cambiar de estados
        public override SkinwalkerStateMachine.EnemyState GetNextState()
        {
            return SkinwalkerStateMachine.EnemyState.Dead;
        }

        //Metodos de cambio de flujo del estado
        public override void OnAreaEnter(Collider other)
        {
           
        }

        public override void OnAreaStay(Collider other)
        {
           
        }

        public override void OnAreaExit(Collider other)
        {
            
            
        }
    }

}