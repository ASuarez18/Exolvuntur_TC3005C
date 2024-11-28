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

            //Elegimos un destino aleatorio de la lista de puntos y lo guardamos
            kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;

            //Ejecutamos la animacion de patrullaje
            manager.animator.SetFloat("forward", 1f);
            
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            

            if(!PhotonNetwork.IsMasterClient) return ;

            //Revisamos si algun jugador esta en el area
            kormosSM.ViewOnAreaFarPlayers();

            //Elegimos un nuevo destino cada ve que se acerca a su destino
            if (manager.agent.remainingDistance <= 1f)
            {  
                
                kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;
               
               while(kormosSM.actualTarget == manager.agent.destination)
                {
                    //Sincronizamos el nuevo destino
                    kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count)].position;                 
                }
                
                //Actualizamos la posicion del agente al destino
                manager.agent.SetDestination(kormosSM.actualTarget);
            }
            else
            {
                //Actualizamos la posicion del agente al destino
                manager.agent.SetDestination(kormosSM.actualTarget);
            }
        }

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
        // public override void OnAreaEnter(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return;
        //     //Verificamos que el objeto tenga el tag de player
        //     if (other.gameObject.tag == "Player")
        //     {
        //         kormosSM.PlayerOnAreaFar = true;
        //         kormosSM.PlayerGameObject = other.gameObject;
        //     }
        // }

        // public override void OnAreaStay(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return;
        //     if(other.gameObject.tag == "Player")
        //     {
        //         kormosSM.PlayerPosition = other.transform.position;
        //         kormosSM.PlayerGameObject = other.gameObject;
        //     }
        // }

        // public override void OnAreaExit(Collider other)
        // {
            
        //     //No realizamos nada
        // }

    }
}
