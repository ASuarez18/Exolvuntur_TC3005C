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
    public class SkinwalkerTransform : BaseState<SkinwalkerStateMachine.EnemyState>
    {
        //Referencia hacia el manager y la maquina de estados del enemigo
        private EnemySkinWalkerManager manager;
        private SkinwalkerStateMachine skinwalkerSM;

        //Constructor del estado (Manager y Machine)
        public SkinwalkerTransform(EnemySkinWalkerManager manager, SkinwalkerStateMachine machine) : base(SkinwalkerStateMachine.EnemyState.Transform)
        {
            this.manager = manager;
            this.skinwalkerSM = machine;
        }

        public override void EnterState()
        {
            if(!PhotonNetwork.IsMasterClient) return ;
            //Activamos el gameObject que contiene el modelo real del enemigo
            manager.enemyRealModel.SetActive(true);
            //Desactivamos el gameObject que contiene el modelo fantasma del enemigo
            manager.enemyFakeModel.SetActive(false);
        }

        public override void UpdateState()
        {
        
        }

        public override void ExitState()
        {
            if(!PhotonNetwork.IsMasterClient) return ;
            //Activamos la variable de que si esta transformado
            skinwalkerSM.IsTransformed = true;
            
        }

        //Función para cambiar de estados
        public override SkinwalkerStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return SkinwalkerStateMachine.EnemyState.Transform;
            return SkinwalkerStateMachine.EnemyState.Chasing;
        }

        //Metodos de cambio de flujo del estado
        // public override void OnAreaEnter(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return ;
        //     if(other.CompareTag("Player"))
        //     {
        //         skinwalkerSM.PlayerOnAreaFar = true;
        //     }
        // }

        // public override void OnAreaStay(Collider other)
        // {
        // }

        // public override void OnAreaExit(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return ;   
        //     if(other.CompareTag("Player"))
        //     {
        //         skinwalkerSM.PlayerOnAreaFar = false;
        //     }
        // }
    }
}