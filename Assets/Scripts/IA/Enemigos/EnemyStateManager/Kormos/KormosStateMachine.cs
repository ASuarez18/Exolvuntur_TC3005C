using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using PlayerController.PUN;
using Photon.Pun;

namespace Enemy.Behaviour
{
    /// <summary>
    /// Maquina de estados que hereda de StateManager.
    /// Agregamos los estados del enemigo Kormos y agregamos la clase del controlador del enemigo.
    /// </summary>
    /// holip te vengo a joder 
    public class KormosStateMachine : StateManager<KormosStateMachine.EnemyState> , InterfaceAttacking
    {
        #region Estados
            public enum EnemyState
            {
                Idle,
                Hunt,
                Caution,
                Aggresive,
                Attack,
                Chasing,
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
            public GameObject PlayerGameObject { get; set; }
            public float DistanceToPlayer { get; set; }
            public float AgressiveCounter { get; set; }
            public float AggresiveDuration { get; set; }
            public int AggresiveMode { get; set; }
            public bool IsStunned { get; set; }
            public float TimeStunned { get; set; }
            public bool Attacking { get; set; }
            public float TimeOfAttack { get; set; }
          
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
                state.Add(EnemyState.Chasing, new KormosChasing(manager, this));
                state.Add(EnemyState.Attack, new KormosAttack(manager,this));
                state.Add(EnemyState.Aggresive, new KormosAgressive(manager,this));
                state.Add(EnemyState.Scape, new KormosScape(manager,this));
                state.Add(EnemyState.Heal, new KormosHeal(manager,this));
                state.Add(EnemyState.Stunned, new KormosStunned(manager,this));
                state.Add(EnemyState.Dead, new KormosDead(manager,this));

                currentState = state[EnemyState.Idle];

                // photonView = GetComponent<PhotonView>();

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

            void InterfaceAttacking.Attack(GameObject target)
            {
                Attacking = true;
                target.GetComponent<PUNPlayerSanity>().TakeDamage(10, "Kormos");
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
        public void UpdateStunTime()
        {
            TimeStunned += Time.deltaTime;
        }
        public void HealOverTime()
        {
            currentHealth += 5 * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, manager.enemyStats.Health);
        }
        public void UpdateAttackTime()
        {
            TimeOfAttack += Time.deltaTime;
        }
        public void EnemyDead()
        {
            Destroy(manager.gameObject);
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

        //Dibujamos una esfera con gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, currentAttackRange);
        }

    }
}


