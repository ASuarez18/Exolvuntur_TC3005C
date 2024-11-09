using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

namespace Enemy.Behaviour
{
    public class KormosStateMachine : StateManager<KormosStateMachine.EnemyState>
    {
        #region Variables
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

        private EnemyKormosManager manager;

        #endregion

        #region Unity Callbacks
        public void Awake()
        {
            //Conseguimos el manager del enemigo
            manager = GetComponent<EnemyKormosManager>();

            // Initialize the state machine
            state.Add(EnemyState.Idle, new KormosIdle(manager));
            state.Add(EnemyState.Hunt, new KormosHunt(manager));
            state.Add(EnemyState.Caution, new KormosCaution(manager));
            // state.Add(EnemyState.Aggresive);
            // state.Add(EnemyState.Attack);
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


