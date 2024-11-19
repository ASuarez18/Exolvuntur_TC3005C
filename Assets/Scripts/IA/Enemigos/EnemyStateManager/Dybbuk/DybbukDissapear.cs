using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

namespace Enemy.Behaviour
{
    public class DybbukDissapear : BaseState<DybbukStateMachine.EnemyState>
    {
        //Referencia a clase comportamiento y maquina de estados
        private EnemyDybbukManager manager;
        private DybbukStateMachine dybbukSM;

        //Constructor del estado (Manager y Machine)
        public DybbukDissapear(EnemyDybbukManager manager,DybbukStateMachine machine) : base(DybbukStateMachine.EnemyState.Dissappear)
        {
            this.manager = manager;
            this.dybbukSM = machine;
        }

        //Inicializa el estado
        public override void EnterState()
        {
            //Desactivamos el render o modelo del enemigo
            manager.gameObject.GetComponent<MeshRenderer>().enabled  = false;
        }

        //Actualiza el estado en el Update del MonoBehaviour
        public override void UpdateState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        //Funcion que revisa si entra en el flujo de un estado o no
        public override DybbukStateMachine.EnemyState GetNextState()
        {
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
            if (other.gameObject.tag == "Player")
                {
                    dybbukSM.PlayerOnAreaClose = false;
                }
        }
    }
}