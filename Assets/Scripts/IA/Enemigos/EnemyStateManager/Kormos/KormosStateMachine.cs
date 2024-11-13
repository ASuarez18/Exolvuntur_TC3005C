using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

namespace Enemy.Behaviour
{
    /// <summary>
    /// Maquina de estados que hereda de StateManager.
    /// Agregamos los estados del enemigo Kormos y agregamos la clase del controlador del enemigo.
    /// </summary>
    public class KormosStateMachine : StateManager<KormosStateMachine.EnemyState>
    {
        #region Estados
        public enum EnemyState
        {
            Idle,
            Hunt,
            Caution,
            Aggresive,
            Attack,
            Search,
            Scape,
            Heal,
            Stunned,
            Dead
        }
        #endregion

        #region Variables y Atributos

        private EnemyKormosManager manager;
        public bool PlayerOnAreaClose { get; set; }
        public bool PlayerOnAreaFar { get; set; }
        public bool SoundDetected { get; set; }
        public Vector3 actualTarget { get; set; }
        public Vector3 PlayerPosition{ get; set; }
        public float DistanceToPlayer { get; set; }
        public float AgressiveCounter { get; set; }
        public float AggresiveDuration { get; set; }
        public int AggresiveMode { get; set; }
        public bool IsStunned { get; set; }

        #endregion

        #region Estadisticas

        public float currentSpeed;
        public float currentRunningSpeed;
        public float currentAcceleration;
        public float currentAttackRange;
        public float currentHealth;

        #endregion

    
        #region Unity Callbacks
        public void Awake()
        {
            //Conseguimos el manager del enemigo nuestro GameObject
            manager = GetComponent<EnemyKormosManager>();

            // Initialize the state machine
            state.Add(EnemyState.Idle, new KormosIdle(manager,this));
            state.Add(EnemyState.Caution, new KormosCaution(manager,this));
            state.Add(EnemyState.Hunt, new KormosHunt(manager,this));
            state.Add(EnemyState.Attack, new KormosAttack(manager,this));
            state.Add(EnemyState.Aggresive, new KormosAgressive(manager,this));
            // state.Add(EnemyState.Scape);
            // state.Add(EnemyState.Heal);
            // state.Add(EnemyState.Stunned);
            // state.Add(EnemyState.Dead);

            currentState = state[EnemyState.Idle];

            //Inicializamos las estadisticas
            currentSpeed = manager.enemyStats.Speed;
            currentRunningSpeed = manager.enemyStats.RunningSpeed;
            currentAcceleration = manager.enemyStats.Acceleration;
            currentAttackRange = manager.enemyStats.AttackRange;
            currentHealth = manager.enemyStats.Health;

            //Inicializamos las estadisticas en el componente de agente y sus sensores
            manager.agent.speed = currentSpeed;
            manager.agent.acceleration = currentAcceleration;

        }

        //Funciones que se activan los Trigger de la maquina de estados -> Trigger del current State
        public void OnTriggerEnter(Collider other)
        {
            Debug.LogError("OnTriggerEnter");
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
        #endregion

        //Funcion de transicion
        #region State Transition
        internal void SwitchCase(EnemyState state)
        {
            TransitionToState(state);
        }
        #endregion

        #region BehaviorFunctions
        public void UpdateAgressiveCounter()
        {
            AgressiveCounter += Time.deltaTime;
        }
        public void UpdateAggressiveDuration(){
            AggresiveDuration += Time.deltaTime; 
        }

        #endregion

        #region DamageFuntions
        public void ApplyStun()
        {
            IsStunned = true;
        }
        public void ApplyDamage(int damage)
        {
            if (IsStunned)
            {
                currentHealth -= damage;
            }
        }
        #endregion

        // public void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawSphere(manager.transform.position,currentAttackRange);
        // }

    }
}


