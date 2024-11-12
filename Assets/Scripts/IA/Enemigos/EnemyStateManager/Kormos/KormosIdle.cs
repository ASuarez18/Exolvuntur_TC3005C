using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

namespace Enemy.Behaviour
{
    public class KormosIdle : BaseState<KormosStateMachine.EnemyState>
    {

        //Referencia a clase comportamiento y maquina de estados
        private EnemyKormosManager manager;
        private KormosStateMachine kormosSM;

        //Constructor

        public KormosIdle(EnemyKormosManager manager,KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Idle)
        {
            this.manager = manager;
            this.kormosSM = machine;
        }

        public override void EnterState()
        {
            //Elegimos un destino aleatorio
            kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count-1)].position;
            
        }

        public override void UpdateState()
        {
            //Actualizamos la posicion del agente al destino
            manager.agent.SetDestination(kormosSM.actualTarget);

            //Elegimos un nuevo destino cada ve que se acerca a su destino
            if (manager.agent.remainingDistance <= 0.5f)
            {  
                
                kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count-1)].position;
                // Debug.LogWarning($"Nuevo destino {kormosSM.actualTarget}");
                while(kormosSM.actualTarget == manager.agent.destination)
                {
                    kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count-1)].position;
                }
            }
        
            //Revisamos su siguientes estados
            GetNextState();
        }

        public override void ExitState()
        {
            // Debug.Log(kormosSM.PlayerOnAreaFar);
        }

        public override KormosStateMachine.EnemyState GetNextState()
        {
            //*Revisamos si la bandera activa otro estado
            if (kormosSM.PlayerOnAreaFar)
            {
                return KormosStateMachine.EnemyState.Caution;
            }

            return KormosStateMachine.EnemyState.Idle;
        }

        //Metodos de cambio de flujo del estado

        public override void OnAreaEnter(Collider other)
        {
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

    
    }
}
