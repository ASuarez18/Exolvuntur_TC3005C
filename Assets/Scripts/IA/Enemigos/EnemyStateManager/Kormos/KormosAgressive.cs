using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;


namespace Enemy.Behaviour
{
    public class KormosAgressive : BaseState<KormosStateMachine.EnemyState>
    {
        //Llamamos a la clase de EnemyKormosManager
        private EnemyKormosManager manager;
        private KormosStateMachine kormosSM;

        public KormosAgressive(EnemyKormosManager manager, KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Aggresive)
        {
            this.manager = manager;
            this.kormosSM = machine;
        }

        public override void EnterState()
        {
            manager.animator.SetTrigger("agresividad");
            if(!PhotonNetwork.IsMasterClient) return;
            //Detenemos el movimiento del agente
            manager.agent.isStopped = true;

            //Incrementamos las estadisticas del enemigo en la maquina de estados un 20%
            kormosSM.currentSpeed += kormosSM.currentRunningSpeed;
            kormosSM.currentAcceleration += kormosSM.currentAcceleration;
            kormosSM.currentAttackRange += (kormosSM.currentAttackRange * .20f);
            kormosSM.currentHealth += (kormosSM.currentHealth * .20f);

            //Incrementamos las estadisticas del enemigo en el agente del manager
            manager.agent.speed = kormosSM.currentSpeed;
            manager.agent.acceleration = kormosSM.currentAcceleration;

            //Incrementamos las veces que puede entrar en este modo
            kormosSM.AggresiveMode++;

            //manager.Animatorfuc("agresividad");
            //manager.animator.SetBool("agresividad",true);

        }


        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            //Obtenemos la funcion de Update Aggresive Counter
            kormosSM.UpdateAggressiveDuration();
        }

        public override void ExitState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            kormosSM.AgressiveCounter = 0f;
            kormosSM.AggresiveDuration = 0f;
            manager.agent.isStopped = false;
        }

        public override KormosStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return KormosStateMachine.EnemyState.Aggresive;
            if(kormosSM.AggresiveDuration >= manager.enemyStats.AggroDuration)
            {
                return KormosStateMachine.EnemyState.Chasing;
            }
            return KormosStateMachine.EnemyState.Aggresive;
            
        }

        // public override  void OnAreaEnter(Collider other)
        // {

        // }

        // public override void OnAreaStay(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return;
        //     if(other.gameObject.tag == "Player")
        //     {
        //         kormosSM.PlayerPosition = other.transform.position;
        //         kormosSM.PlayerGameObject = other.gameObject;
        //     }
        // }

        // public override void OnAreaExit(Collider other)
        // {
        //     if(!PhotonNetwork.IsMasterClient) return;
        //     if (other.gameObject.tag == "Player")
        //     {
        //         kormosSM.PlayerOnAreaFar = false;
        //         kormosSM.PlayerPosition = Vector3.zero;
        //         kormosSM.PlayerGameObject = null;
        //     }
        // }
    }
}