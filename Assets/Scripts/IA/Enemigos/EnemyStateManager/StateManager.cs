using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Esta clase se decica a manejar la logica entre varios estados.
/// Se encarga de manejar las transiciones entre estados y comenzar el flujo
/// Es el cerebro de organizar y ordenar los diferentes estados
/// </summary>

namespace Enemy.Behaviour
{
    public abstract class StateManager<EState> : MonoBehaviourPunCallbacks where EState : Enum
    {

        //Diccionario que guarda los diferentes estados (Nombre del estado, logica del estado)
        protected Dictionary<EState,BaseState<EState>> state = new Dictionary<EState, BaseState<EState>>();
        //Clase que guarda el estado actual
        public BaseState<EState> currentState;
        protected bool IsTransitionState = false;

        void Start()
        {
            // Llamamos al metodo de estado actual para inicializacion
            if(!PhotonNetwork.IsMasterClient) return;
            currentState.EnterState();
            
        }
        void Update()
        {
            // //Guardamos la conexion del siguiente estado
            // EState nextState = currentState.GetNextState();

            // if(!IsTransitionState && nextState.Equals(currentState.StateKey))
            // {
            //     //Actualizamos el estado actual y revisamos la logica cada frame
            //     currentState.UpdateState();
                
            // }
            // else if(!IsTransitionState)
            // {
            //     TransitionToState(nextState);
            //     Debug.Log(currentState.StateKey);
            // }

            if(!PhotonNetwork.IsMasterClient) return;

            EState nextState = currentState.GetNextState();

        
            if(!IsTransitionState && nextState.Equals(currentState.StateKey))
            {
                //Actualizamos el estado actual y revisamos la logica cada frame
                currentState.UpdateState();
            }
            else if(!IsTransitionState)
            {
                TransitionToState(nextState);
                Debug.Log(currentState.StateKey);
            }
            

        }

        public void TransitionToState(EState stateKey)
        {
            IsTransitionState = true;
            // currentState.ExitState();
            currentState.ExitState();
            currentState = state[stateKey];
            // currentState.EnterState();
            currentState.EnterState();
            IsTransitionState = false;
        }

        public abstract void OnTriggerEnter(Collider other);
        public abstract void OnTriggerStay(Collider other);
        public abstract void OnTriggerExit(Collider other);

        // public virtual void OnTriggerEnter(Collider other)
        // {  
        //     currentState.OnAreaEnter(other);
        // }
        // public virtual void OnTriggerStay(Collider other)
        // {
        //     currentState.OnAreaStay(other);
        // }
        // public virtual void OnTriggerExit(Collider other)
        // {
        //     currentState.OnAreaExit(other);
        // }

        // Creamos varios RPC de sincornizacion de los enemigos
        // [PunRPC]
        // public void SyncronizeEnterState()
        // {
        //     currentState.EnterState();
        // }

        // [PunRPC]
        // public void SyncronizeUpdateState()
        // {
        //     currentState.UpdateState();
        // }

        // [PunRPC]
        // public void SyncronizeExitState()
        // {
        //     currentState.ExitState();
        // }
    }
}