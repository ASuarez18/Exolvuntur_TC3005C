using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

namespace Enemy.Behaviour
{
    public class KormosIdle : BaseState<KormosStateMachine.EnemyState>
    {

        //Llamamos a la clase de EnemyKormosManager
        private EnemyKormosManager manager;
        private Transform actualTarget;
        

        //Banderas del estado
        public bool playerOnArea = false;

       

        public KormosIdle(EnemyKormosManager manager) : base(KormosStateMachine.EnemyState.Idle)
        {
            this.manager = manager;
        }



        public override void EnterState()
        {
           

            actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count-1)];
        }

        public override void UpdateState()
        {
            //*Actualizacion de la logica del estado
            manager.agent.SetDestination(actualTarget.position);
            if (manager.agent.remainingDistance <= 0.5f)
            {  
                // Debug.Log("Entre");
                actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count-1)];
                while(actualTarget.position == manager.agent.destination)
                {
                    actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count-1)];
                }
            }
            // Debug.Log(playerOnArea);
            //*Revisamos si se activa a otro estado
            GetNextState();
        }

        public override void ExitState()
        {
            //*Debemos de restablecer las banderas
            playerOnArea = false;
        }

        public override KormosStateMachine.EnemyState GetNextState()
        {
            //*Revisamos si la bandera activa otro estado
            if (playerOnArea)
            {
                Debug.Log("Entrex3");
                return KormosStateMachine.EnemyState.Caution;
            }

            return KormosStateMachine.EnemyState.Idle;
        }

        //Metodos de cambio de flujo del estado

        public override void OnAreaEnter(Collider other)
        {
                // Debug.Log("Entre");
                //Verificamos que el objeto tenga el tag de player
                if (other.gameObject.tag == "Player")
                {
                    Debug.Log("Entrex2");
                    playerOnArea = true;
                }
        }

        public override void OnAreaStay(Collider other)
        {
           
                // //Verificamos que el objeto tenga el tag de player
                // if (other.gameObject.tag == "Player")
                // {
                //     playerOnArea = true;
                // }
        }

        public override void OnAreaExit(Collider other)
        {
            
                //Verificamos que el objeto tenga el tag de player
                if (other.gameObject.tag == "Player")
                {
                    playerOnArea = false;
                }
        }

    
    }
}
