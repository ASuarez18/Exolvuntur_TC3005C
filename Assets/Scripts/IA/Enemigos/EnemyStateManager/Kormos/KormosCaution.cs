using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;

/// <summary>
/// El estado de alerta detiene el movimiento del enemigo.
/// Mientras que el jugador se mantenga dentro del Sphere Collider del enemigo se mantiene estatico.
/// Estados con los que conecta: KormosIdle, KormosHunt, KormosAggresive.
/// </summary>

namespace Enemy.Behaviour
{
    public class KormosCaution : BaseState<KormosStateMachine.EnemyState>
    {
        //Llamamos a la clase de EnemyKormosManager y la clase de La maquina de estados
        private EnemyKormosManager manager;
        private KormosStateMachine kormosSM;

        //Constructor del estado
        public KormosCaution(EnemyKormosManager manager, KormosStateMachine machine) : base(KormosStateMachine.EnemyState.Caution)
        {
            this.manager = manager;
            this.kormosSM = machine;
        }

        //Inicializamos el estado
        public override void EnterState()
        {
            manager.animator.SetTrigger("alerta");
            if(!PhotonNetwork.IsMasterClient) return;
            //Detenemos el movimiento del agente
            manager.agent.isStopped = true;
            //manager.animator.SetTrigger("alerta");
            //manager.Animatorfuc("alerta");
        }

        //Actualizamos el estado en el Update del Monobehaviour
        public override void UpdateState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            //Revisamos si algun jugador esta en el area
            kormosSM.ViewOnAreaFarPlayers();

            // Update aggressivenes time counter
            kormosSM.UpdateAgressiveCounter();
        
        }

        //Salimos del estado
        public override void ExitState()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            manager.agent.isStopped = false;
            kormosSM.AgressiveCounter = 0f;


            
            
        }


        //Revisamos el siguiente estado a partir de las condiciones de la bandera y el contador agresivo
        public override KormosStateMachine.EnemyState GetNextState()
        {
            if(!PhotonNetwork.IsMasterClient) return KormosStateMachine.EnemyState.Caution;
            //Si el jugador sale del area o se encuentra en el area lejano
            if(kormosSM.IsStunned)
            {
                return KormosStateMachine.EnemyState.Stunned;
            }
            else if (!kormosSM.PlayerOnAreaFar)
            {  
                return KormosStateMachine.EnemyState.Idle;
            }
            //Si el enemigo detecta un sonido
            else if(kormosSM.SoundDetected)
            {
                return KormosStateMachine.EnemyState.Hunt;
            }
            //Si el contador de agresivo supera el tiempo de agresivo y no a entrado 3 veces al mismo estado
            else if(kormosSM.AgressiveCounter >= manager.enemyStats.AggroTime && kormosSM.AggresiveMode <= 2)
            {
                return KormosStateMachine.EnemyState.Aggresive;
            }
            return KormosStateMachine.EnemyState.Caution;
        }

        //Metodos de cambio de flujo del estado
        // public override void OnAreaEnter(Collider other)
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