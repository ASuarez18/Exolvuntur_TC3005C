using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using GamePlay.IA;

namespace Enemy.Behaviour
{
    public class KormosHunt : BaseState<KormosStateMachine.EnemyState>
    {
        //Llamamos a la clase de EnemyKormosManager

        private EnemyKormosManager manager;
        private Vector3 SoundPosition;

        //Banderas del estado

        private bool playerOnSight = false;
        private bool onDestination = false;

        public KormosHunt(EnemyKormosManager manager) : base(KormosStateMachine.EnemyState.Hunt)
        {
            this.manager = manager;
        }

        //Funciones del flujo
        public void Hunt(SoundGame sound)
        {
            //Actualizamos la posicion del sonido
            SoundPosition = sound.pos;
            Debug.Log(SoundPosition);
        }

        //Creamos el flujo del estado de Idle

        public override void EnterState()
        {
            
        }

        public override void UpdateState()
        {
            manager.agent.SetDestination(SoundPosition);
            Debug.Log(SoundPosition + "Jugador");
            //Cuando el agente llega a su destino del sonido cambiamos de estado
            if (manager.agent.remainingDistance == 0f)
            {
                Debug.Log("LLegue al sonido");
                onDestination = true;
            }
            GetNextState();
        }

        public override void ExitState()
        {
            //Reiniciamos nuestras banderas
            playerOnSight = false;
            onDestination = false;
        }

        public override KormosStateMachine.EnemyState GetNextState()
        {
            //*Revisamos si la bandera activa otro estado
            if(playerOnSight)
            {
                return KormosStateMachine.EnemyState.Attack;
            }
            else if(onDestination)
            {
                return KormosStateMachine.EnemyState.Idle;
            }
            return KormosStateMachine.EnemyState.Hunt;
            
        }

        //Metodos de cambio de flujo del estado

        public override void OnAreaEnter(Collider other)
        {
            
                //Verificamos que el objeto tenga el tag de player
                if (other.gameObject.tag == "Player")
                {
                    playerOnSight = true;
                }
        }

        public override void OnAreaStay(Collider other)
        {  
             
                //Verificamos que el objeto tenga el tag de player
                if (other.gameObject.tag == "Player")
                {
                    playerOnSight = true;
                }
        }

        public override void OnAreaExit(Collider other)
        {
             
                //Verificamos que el objeto tenga el tag de player
                if (other.gameObject.tag == "Player")
                {
                    playerOnSight = false;
                }
        }

    }

}
