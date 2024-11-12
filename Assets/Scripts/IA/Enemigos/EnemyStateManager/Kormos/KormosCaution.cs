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
        private KormosStateMachine kormosSM;

        public KormosCaution(EnemyKormosManager manager, KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Caution)
        {
            this.manager = manager;
            this.kormosSM = machine;
        }

        public override void EnterState()
        {
            //Detenemos el movimiento del agente
            manager.agent.isStopped = true;
        }

        public override void UpdateState()
        {
            //Revisamos los estados
            GetNextState();
        }

        public override void ExitState()
        {
            manager.agent.isStopped = false;
        }

        public override KormosStateMachine.EnemyState GetNextState()
        {
            //*Revisamos si la bandera activa otro estado
            if (!kormosSM.PlayerOnAreaFar)
            {
                
                return KormosStateMachine.EnemyState.Idle;
            }
            else if(kormosSM.SoundDetected)
            {
                return KormosStateMachine.EnemyState.Hunt;
            }
            return KormosStateMachine.EnemyState.Caution;
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
            if (other.gameObject.tag == "Player")
            {
                Debug.LogError("Jugador salio del area");
                kormosSM.PlayerOnAreaFar = false;
            }
        }

    }

}