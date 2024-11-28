using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;

namespace Enemy.Behaviour 
{
    public class DybbukScape : BaseState<DybbukStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
            private EnemyDybbukManager manager;
            private DybbukStateMachine dybbukSM;
            private bool reachedTarget;

            //Constructor del estado (Manager y Machine)
            public DybbukScape(EnemyDybbukManager manager,DybbukStateMachine machine) : base(DybbukStateMachine.EnemyState.Scape)
            {
                this.manager = manager;
                this.dybbukSM = machine;
            }

            //Inicializa el estado
            public override void EnterState()
            {
                if(!PhotonNetwork.IsMasterClient) return;
                //Aumentanmos la velocidad del enemigo y elegimos un punto alejado
                dybbukSM.currentSpeed += dybbukSM.currentSpeed;
                dybbukSM.currentRunningSpeed += dybbukSM.currentRunningSpeed;
                //Se busca el punto mas lejano de la que se encuentra el enemigo
                Vector3 farthestWaypoint = Vector3.zero;
                float maxDistance = 0;
                reachedTarget = false;
                foreach (var waypoint in manager.waypoints)
                {
                    if (Vector3.Distance(manager.transform.position, waypoint.position) > maxDistance)
                    {
                        dybbukSM.actualTarget = waypoint.position;
                    }
                }
             }

            //Actualiza el estado en el Update del MonoBehaviour
            public override void UpdateState()
            {
                if(!PhotonNetwork.IsMasterClient) return;
                //Actualizamos la posicion del agente al destino
                manager.agent.SetDestination(dybbukSM.actualTarget);

                //El enmigo se cura mientras se escapa
                dybbukSM.HealOverTime();

                // Debug.Log(manager.agent.remainingDistance);

                //Una vez que llego a su destino
                if (manager.agent.remainingDistance <= 1f)
                {  
                    reachedTarget = true;
                }
        
            }

            public override void ExitState()
            {
                if(!PhotonNetwork.IsMasterClient) return;
                //Activamos el mesh renderer del enemigo
                manager.enemyRender.enabled = true;
            }

            //Funcion que revisa si entra en el flujo de un estado o no
            public override DybbukStateMachine.EnemyState GetNextState()
            {
                if(!PhotonNetwork.IsMasterClient) return DybbukStateMachine.EnemyState.Scape;
                if (reachedTarget)
                {
                    return DybbukStateMachine.EnemyState.Idle;
                }

                return DybbukStateMachine.EnemyState.Scape;
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
                    dybbukSM.PlayerOnAreaClose = false;
                    dybbukSM.PlayerPosition = Vector3.zero;
                    dybbukSM.PlayerGameObject = null;
                }
            }
    }
}