using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using GamePlay.IA;

///<summary>
///El estado de caza se activa con reaccion al sonido.
///Este se dirige a la pósicion donde localizo algun sonido.
///Sus conexiones a otros estados son : Kormosattack, KormosIdle

namespace Enemy.Behaviour
{
    public class KormosHunt : BaseState<KormosStateMachine.EnemyState>
    {
        //Llamamos a la clase de EnemyKormosManager y KormosStateMachine
        private EnemyKormosManager manager;
        private KormosStateMachine kormosSM;

        //Atributos especificos del estado
        private bool onDestination = false;

        //Constructor del estado
        public KormosHunt(EnemyKormosManager manager, KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Hunt)
        {
            this.manager = manager;
            this.kormosSM = machine;
        }

        //Inicializmos el estado
        public override void EnterState()
        {
            
        }

        //Actualizamos el estado en el Update
        public override void UpdateState()
        {
            //Asiganmos el destino a la posicion del sonido
            manager.agent.SetDestination(kormosSM.actualTarget);
            
            //Verificamos la distancia actual del agente y la del jugador
            kormosSM.DistanceToPlayer = Vector3.Distance(manager.transform.position,kormosSM.PlayerPosition);

            if(kormosSM.DistanceToPlayer <= kormosSM.currentAttackRange)
            {
                //Si el jugador se encuentra en el segundo rango entra en ataque
                kormosSM.PlayerOnAreaClose = true;
            }

            //Cuando el agente llega a su destino del sonido cambiamos de estado
            if(manager.agent.remainingDistance == 0f)
            {
                //Si el agente llega a su destino cambiamos de estado
                onDestination = true;
            }

            //Verificamos si entra a otro estados
            GetNextState();
        }

        //Salimos del estado
        public override void ExitState()
        {
            onDestination = false;
            kormosSM.SoundDetected = false;
        }

        //Funcion que conecta con otros estados
        public override KormosStateMachine.EnemyState GetNextState()
        {
            //Si el jugador se encuentra en el segundo rango entra en ataque
            if(kormosSM.IsStunned)
            {
                return KormosStateMachine.EnemyState.Stunned;
            }
            else if(kormosSM.PlayerOnAreaClose)
            {
                return KormosStateMachine.EnemyState.Chasing;
            }
            //Si el agente llega a su destino cambiamos de estado
            else if(onDestination || !kormosSM.PlayerOnAreaFar)
            {
                return KormosStateMachine.EnemyState.Idle;
            }
            //Si no entra a ninguno de los estados anteriores entra en caza de nuevo
            return KormosStateMachine.EnemyState.Hunt;
            
        }

        //Metodos de cambio de flujo del estado
        public void Hunt(SoundGame sound)
        {
            //Actualizamos la posicion del sonido
            kormosSM.actualTarget = sound.pos;
        }

        public override void OnAreaEnter(Collider other)
        {
            
        }

        public override void OnAreaStay(Collider other)
        {  
            
            if (other.gameObject.tag == "Player")
            {
                kormosSM.PlayerPosition = other.transform.position;
            }
        }

        public override void OnAreaExit(Collider other)
        {  
            //Verificamos que el objeto tenga el tag de player
            if (other.gameObject.tag == "Player")
            {
                kormosSM.PlayerOnAreaFar = false;
            }
        }

    }

}