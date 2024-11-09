using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

namespace Enemy.Behaviour
{
    public class KormosCaution : BaseState<KormosStateMachine.EnemyState>
    {
        //Llamamos a la clase de EnemyKormosManager

            private EnemyKormosManager manager;

            //Banderas del estado
            public bool playerOnArea = true;

            public bool playerSound = false;

            public KormosCaution(EnemyKormosManager manager) : base(KormosStateMachine.EnemyState.Caution)
            {
                this.manager = manager;
            }

            //Creamos el flujo del estado de Idle

            public override void EnterState()
            {
                //Detenemos el movimiento del agente
                manager.agent.isStopped = true;
            }

            public override void UpdateState()
            {
                //*Actualizacion de la logica del estado
                manager.agent.isStopped = true;

                //*Revisamos si se activa a otro estado
                GetNextState();
            }

            public override void ExitState()
            {
                //*Debemos de restablecer las banderas
                playerOnArea = false;
                manager.agent.isStopped = false;
            }

            public override KormosStateMachine.EnemyState GetNextState()
            {
                //*Revisamos si la bandera activa otro estado
                if (!playerOnArea)
                {
                    return KormosStateMachine.EnemyState.Idle;
                }
                else if(playerSound)
                {
                    return KormosStateMachine.EnemyState.Hunt;
                }
                return KormosStateMachine.EnemyState.Caution;
            }

            //Metodos de cambio de flujo del estado

            public override void OnAreaEnter(Collider other)
            {
                    //Verificamos que el objeto tenga el tag de player
                    if (other.gameObject.tag == "Player")
                    {
                        playerOnArea = true;
                    }
            }

            public override void OnAreaStay(Collider other)
            {   
                    // if (other.gameObject.tag == "Player")
                    // {
                    //     playerOnArea = true;
                    // }
            }

            public override void OnAreaExit(Collider other)
            {       
                    if (other.gameObject.tag == "Player")
                    {
                        playerOnArea = false;
                    }
            }

    }

}