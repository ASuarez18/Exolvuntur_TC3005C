using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;


namespace Enemy.Behaviour 
{
    public class KormosScape : BaseState<KormosStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
            private EnemyKormosManager manager;
            private KormosStateMachine kormosSM;
            private bool reachedTarget;

            //Constructor del estado (Manager y Machine)
            public KormosScape(EnemyKormosManager manager,KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Scape)
            {
                this.manager = manager;
                this.kormosSM = machine;
            }

            //Inicializa el estado
            public override void EnterState()
            {
                if(!PhotonNetwork.IsMasterClient) return;
                // ! Aumentar la velocidad al doble
                kormosSM.currentSpeed += kormosSM.currentSpeed;
                kormosSM.currentRunningSpeed += kormosSM.currentRunningSpeed;
                // ? Se busca el punto mas lejano de la que se encuentra el enemigo
                Vector3 farthestWaypoint = Vector3.zero;
                float maxDistance = 0;
                reachedTarget = false;
                foreach (var waypoint in manager.waypoints)
                {
                    if (Vector3.Distance(manager.transform.position, waypoint.position) > maxDistance)
                    {
                        kormosSM.actualTarget = waypoint.position;
                    }
                }

                //Inicializamos la animacion de escape
                manager.animator.SetFloat("forward", 3f);
             }

            //Actualiza el estado en el Update del MonoBehaviour
            public override void UpdateState()
            {
                if(!PhotonNetwork.IsMasterClient) return;
                //Actualizamos la posicion del agente al destino
                manager.agent.SetDestination(kormosSM.actualTarget);

                //Una vez que llego a su destino
                if (manager.agent.remainingDistance <= 1f)
                {  
                    reachedTarget = true;
                }
            
            }

            public override void ExitState()
            {
                
            }

            //Funcion que revisa si entra en el flujo de un estado o no
            public override KormosStateMachine.EnemyState GetNextState()
            {
                if(!PhotonNetwork.IsMasterClient) return KormosStateMachine.EnemyState.Scape;
                if(kormosSM.IsStunned)
                {
                    // ! Manda a estado de stunneado
                    return KormosStateMachine.EnemyState.Stunned;
                }
                //*Revisamos si la bandera activa otro estado
                else if (reachedTarget)
                {
                    // ! Manda a estado de curacion
                    return KormosStateMachine.EnemyState.Heal;
                }

                return KormosStateMachine.EnemyState.Scape;
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
                if(!PhotonNetwork.IsMasterClient) return;
                if (other.gameObject.tag == "Player")
                {
                    kormosSM.PlayerOnAreaFar = false;
                }
            }
    }
}