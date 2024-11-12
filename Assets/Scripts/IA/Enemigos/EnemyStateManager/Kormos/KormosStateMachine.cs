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
            // state.Add(EnemyState.Aggresive);
            // state.Add(EnemyState.Search);
            // state.Add(EnemyState.Scape);
            // state.Add(EnemyState.Heal);
            // state.Add(EnemyState.Stunned);
            // state.Add(EnemyState.Dead);

            currentState = state[EnemyState.Idle];
        }

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
        #endregion

        #region State Transition
        internal void SwitchCase(EnemyState state)
        {
            TransitionToState(state);
        }
        #endregion

    }
}


