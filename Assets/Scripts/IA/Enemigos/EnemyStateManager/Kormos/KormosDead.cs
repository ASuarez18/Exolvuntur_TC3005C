using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

namespace Enemy.Behaviour
{
    public class KormosDead : BaseState<KormosStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
        private EnemyKormosManager manager;
        private KormosStateMachine kormosSM;

        //Constructor del estado (Manager y Machine)
        public KormosDead(EnemyKormosManager manager,KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Dead)
        {
            this.manager = manager;
            this.kormosSM = machine;
        }

        //Inicializa el estado
        public override void EnterState()
        {
            // ? Detenemos el movimiento del agente dado que esta muerto
            manager.agent.isStopped = true;
            // TODO: Settear animacion de muerte con corrutina para que no destruya el objeto al momento
            // ! Destruye el gameObject del enemigo (podria ser desactivado si se usa una pool)
            kormosSM.EnemyDead();
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        //Funcion que revisa si entra en el flujo de un estado o no
        public override KormosStateMachine.EnemyState GetNextState()
        {
            return KormosStateMachine.EnemyState.Dead;
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
            
        }
    }
}