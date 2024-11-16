using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

namespace Enemy.Behaviour
{
    public class DybbukStateMachine : StateManager<DybbukStateMachine.EnemyState>
    {
        #region Estados

        public enum EnemyState
        {
            Idle,
            Still,
            Chasing,
            Attack,
            Aggresive,
            Stunned,
            Dissappear,
            Scape,
            Dead
        }

        #endregion

        #region Variables y Atributos
        //Variables y Atributos de distancia 
        private EnemyDybbukManager manager;
        public Vector3 actualTarget { get; set; }
        public bool PlayerOnAreaClose { get; set; }
        public bool OnView { get; set; }

        #endregion

        #region Estadisticas
            public float currentSpeed;
            public float currentRunningSpeed;
            public float currentAcceleration;
            public float currentAttackRange;
            public float currentHealth;
            public float AgroTime;
            public float AgroDuration;

        #endregion

        #region Unity Callbacks
            public void Awake()
            {
                //Conseguimos el manager del enemigo nuestro GameObject
                manager = GetComponent<EnemyDybbukManager>();

                // Initialize the state machine
                state.Add(EnemyState.Idle, new DybbukIdle(manager,this));
                state.Add(EnemyState.Still, new DybbukStill(manager,this));
                // state.Add(EnemyState.Hunt, new KormosHunt(manager,this));
                // state.Add(EnemyState.Chasing, new KormosChasing(manager, this));
                // state.Add(EnemyState.Attack, new KormosAttack(manager,this));
                // state.Add(EnemyState.Aggresive, new KormosAgressive(manager,this));
                // state.Add(EnemyState.Scape, new KormosScape(manager,this));
                // state.Add(EnemyState.Heal, new KormosHeal(manager,this));
                // state.Add(EnemyState.Stunned, new KormosStunned(manager,this));
                // state.Add(EnemyState.Dead, new KormosDead(manager,this));

                currentState = state[EnemyState.Idle];

                //Inicializamos las estadisticas
                currentSpeed = manager.enemyStats.Speed;
                currentRunningSpeed = manager.enemyStats.RunningSpeed;
                currentAcceleration = manager.enemyStats.Acceleration;
                currentAttackRange = manager.enemyStats.AttackRange;
                currentHealth = manager.enemyStats.Health;
                AgroTime = manager.enemyStats.AggroTime;
                AgroDuration = manager.enemyStats.AggroDuration;

                //Inicializamos las estadisticas en el componente de agente y sus sensores
                manager.agent.speed = currentSpeed;
                manager.agent.acceleration = currentAcceleration;

            }

            //Funciones que se activan los Trigger de la maquina de estados -> Trigger del current State
            public void OnTriggerEnter(Collider other)
            {
                base.OnTriggerEnter(other);
            }
            public void OnTriggerStay(Collider other)
            {
                base.OnTriggerStay(other);
            }
            public void OnTriggerExit(Collider other)
            {
                base.OnTriggerExit(other);
            }
            public void OnCollisionEnter(Collision other)
            {  
                // if(currentState.StateKey == EnemyState.Chasing)
                // {
                //     if(other.gameObject.tag == "Player")
                //     {
                //         Attacking = true;
                //     }
                // }
            }

        #endregion

        //Funcion de transicion
        #region State Transition
            internal void SwitchCase(EnemyState state)
            {
                TransitionToState(state);
            }
        #endregion
    }
}
