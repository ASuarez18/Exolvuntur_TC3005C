using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;

/// <summary>
/// El estado de Idle genera un patrullaje aleatorio.
/// El enemigo elige un punto de su lista y se dirige hacia alli.
/// Cuando llega elige un nuevo punto
/// Estados con los que conecta : KormosCaution
/// </summary>

namespace Enemy.Behaviour
{
    public class KormosIdle : BaseState<KormosStateMachine.EnemyState>
    {

        //Referencia a clase comportamiento y maquina de estados
        private EnemyKormosManager manager;
        private KormosStateMachine kormosSM;


        //Constructor del estado (Manager y Machine)
        public KormosIdle(EnemyKormosManager manager,KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Idle)
        {
            this.manager = manager;
            this.kormosSM = machine;
        }

        //Inicializa el estado
        public override void EnterState()
        {

            //Verificamos si es el master client
            if(!PhotonNetwork.IsMasterClient) return;
            
            // //Elegimos un destino aleatorio de la lista de puntos y lo guardamos
            kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;

            // //Sincronizamos la variable o lugar para todos
            // manager.photonView.RPC(nameof(SyncGetRandomPosition),RpcTarget.AllBuffered,kormosSM.actualTarget);

            //Ejecutamos la animacion de patrullaje
            manager.animator.SetFloat("forward", 1f);
                
            

            // //Elegimos un destino aleatorio de la lista de puntos y lo guardamos
            // kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;

            
            // if(PhotonNetwork.IsMasterClient)
            // {
                
            // }
            
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            

            if(!PhotonNetwork.IsMasterClient) return ;

            //Elegimos un nuevo destino cada ve que se acerca a su destino
            if (manager.agent.remainingDistance <= 1f)
            {  
                
                kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
               
               while(kormosSM.actualTarget == manager.agent.destination)
                {
                     //Sincronizamos el nuevo destino
                    kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
                    //Lamamos al RPC
                    // manager.photonView.RPC(nameof(SyncGetRandomPosition),RpcTarget.AllBuffered,kormosSM.actualTarget);                    
                }
                
                //Actualizamos la posicion del agente al destino
                manager.agent.SetDestination(kormosSM.actualTarget);
            }
            else
            {
                //Actualizamos la posicion del agente al destino
                manager.agent.SetDestination(kormosSM.actualTarget);
            }

            //Sincronizamos el estado actual
            // manager.photonView.RPC(nameof(SyncGetRandomPosition),RpcTarget.AllBuffered,kormosSM.actualTarget);
            // manager.photonView.RPC(nameof(SyncUpdatedState),RpcTarget.AllBuffered);
            

            

            // if (manager.agent.remainingDistance <= 1f)
            // {  
                
            //     // kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
            //     // //Verificamos que sea aleatorio
            //     // while(kormosSM.actualTarget == manager.agent.destination)
            //     // {
            //     //     kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
            //     // }

            //     //Verificamos si es el master client
            //     if(PhotonNetwork.IsMasterClient)
            //     {
            //         //Sincronizamos el nuevo destino
            //         kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
            //         //Lamamos al RPC
            //         manager.photonView.RPC(nameof(SyncGetRandomPosition),RpcTarget.AllBuffered,kormosSM.actualTarget);
            //         while(kormosSM.actualTarget == manager.agent.destination)
            //         {
            //              //Sincronizamos el nuevo destino
            //             kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
            //             //Lamamos al RPC
            //             manager.photonView.RPC(nameof(SyncGetRandomPosition),RpcTarget.AllBuffered,kormosSM.actualTarget);                    
            //         }
            //     }
               
                // if(PhotonNetwork.IsMasterClient)
                // {
                    //Sincronizamos el nuevo destino
                    // kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
                    // manager.photonView.RPC(nameof(SyncGetRandomPosition),RpcTarget.AllBuffered,kormosSM.actualTarget);
                // }


              
        
            
        }

        // [PunRPC]
        // private void SyncUpdatedState()
        // {
        //     // Elegimos un nuevo destino cada ve que se acerca a su destino
        //     if (manager.agent.remainingDistance <= 1f)
        //     {  
                
        //         kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
               
        //         if(PhotonNetwork.IsMasterClient)
        //         {
        //             //Sincronizamos el nuevo destino
        //             kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
        //             manager.photonView.RPC(nameof(SyncGetRandomPosition),RpcTarget.AllBuffered,kormosSM.actualTarget);
        //         }


        //         //Actualizamos la posicion del agente al destino
        //         manager.agent.SetDestination(kormosSM.actualTarget);
        //     }
        //     else
        //     {
        //         //Actualizamos la posicion del agente al destino
        //         manager.agent.SetDestination(kormosSM.actualTarget);
        //     }

        // }


        public override void ExitState()
        {
            //No realizamos nada
            
        }

        //Funcion que revisa si entra en el flujo de un estado o no
        public override KormosStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return KormosStateMachine.EnemyState.Idle;
            //Revisamos si el enemigo esta estuneado
            if(kormosSM.IsStunned)
            {
                return KormosStateMachine.EnemyState.Stunned;
            }
            //Revisamos si el jugador esta en el area de alerta
            else if (kormosSM.PlayerOnAreaFar)
            {
                return KormosStateMachine.EnemyState.Caution;
            }

            return KormosStateMachine.EnemyState.Idle;
        }

        //Metodos de cambio de flujo del estado
        public override void OnAreaEnter(Collider other)
        {
            if(!PhotonNetwork.IsMasterClient) return;
            //Verificamos que el objeto tenga el tag de player
            if (other.gameObject.tag == "Player")
            {
                kormosSM.PlayerOnAreaFar = true;
            }
        }

        public override void OnAreaStay(Collider other)
        {
            //No realizamos nada
        }

        public override void OnAreaExit(Collider other)
        {
            
            //No realizamos nada
        }

        /// <summary>
        /// Creamos diferentes RPC dentro del estado.
        /// Los RPC deben de ejecutar la misma logica para todos
        /// </summary>
         

        // [PunRPC]
        // private void SyncGetRandomPosition(Vector3 position){
        //     kormosSM.actualTarget = position;  
        // }

    }
}
