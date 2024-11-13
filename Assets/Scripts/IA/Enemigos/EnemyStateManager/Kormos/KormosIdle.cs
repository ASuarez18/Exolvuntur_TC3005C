using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

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
            //Elegimos un destino aleatorio
            kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count-1)].position;
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            //Actualizamos la posicion del agente al destino
            manager.agent.SetDestination(kormosSM.actualTarget);

            //Elegimos un nuevo destino cada ve que se acerca a su destino
            if (manager.agent.remainingDistance <= 0.5f)
            {  
                
                kormosSM.actualTarget = manager.waypoints[Random.Range(0,manager.waypoints.Count-1)].position;
               
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
            //No realizamos nada
        }

        //Funcion que revisa si entra en el flujo de un estado o no
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
