using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Photon.Pun;

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
        public bool Attacking { get; set; }
        public float TimeOfAttack { get; set; }
        public float AgressiveCounter { get; set; }
        public float AggresiveDuration { get; set; }
        public int AggresiveMode { get; set; }
        public bool IsStunned { get; set; }
        public float TimeStunned { get; set; }

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
                state.Add(EnemyState.Chasing, new DybbukChasing(manager,this));
                state.Add(EnemyState.Attack, new DybbukAttack(manager,this));
                state.Add(EnemyState.Aggresive, new DybbukAgressive(manager,this));
                state.Add(EnemyState.Scape, new DybbukScape(manager,this));
                state.Add(EnemyState.Dissappear, new DybbukDissapear(manager,this));
                state.Add(EnemyState.Stunned, new DybbukStunned(manager,this));
                state.Add(EnemyState.Dead, new DybbukDead(manager,this));

                currentState = state[EnemyState.Idle];

                // photonView = GetComponent<PhotonView>();

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
            public override void OnTriggerEnter(Collider other)
            {
                base.OnTriggerEnter(other);
            }
            public override void OnTriggerStay(Collider other)
            {
                base.OnTriggerStay(other);
            }
            public override void OnTriggerExit(Collider other)
            {
                base.OnTriggerExit(other);
            }
            public void OnCollisionEnter(Collision other)
            {  
                if(currentState.StateKey == EnemyState.Chasing || currentState.StateKey == EnemyState.Aggresive)
                {
                    if(other.gameObject.tag == "Player")
                    {
                        Attacking = true;
                    }
                }
            }

        #endregion

        //Funcion de transicion
        #region State Transition
            internal void SwitchCase(EnemyState state)
            {
                TransitionToState(state);
            }
        #endregion

        #region Update Timer Functions
            public void UpdateAttackTime()
            {
                TimeOfAttack += Time.deltaTime;
            }
            public void UpdateAgressiveCounter()
            {
                AgressiveCounter += Time.deltaTime;
            }
            public void UpdateAggressiveDuration()
            {
                AggresiveDuration += Time.deltaTime; 
            }
            public void UpdateStunTime()
            {
                TimeStunned += Time.deltaTime;
            }
        #endregion

        #region Global Behavioir Functions
             public void HealOverTime()
            {
                Debug.Log("Me estoy curando" + currentHealth);
                currentHealth += 5 * Time.deltaTime; 
                currentHealth = Mathf.Clamp(currentHealth, 0, manager.enemyStats.Health);
            }
            public void ApplyStun()
            {
                IsStunned = true;
            }
            public void ApplyDamage(int damage)
            {
                if (IsStunned)
                {
                    Debug.LogWarning($"Aplico {damage} a enemigo");
                    currentHealth -= damage;
                }
            }
            public void EnemyDead()
            {
                Destroy(manager.gameObject);
            }
        #endregion
    }
}
