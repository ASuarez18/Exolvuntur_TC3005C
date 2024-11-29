using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using PlayerController.PUN;
using Photon.Pun;

namespace Enemy.Behaviour
{
    /// <summary>
    /// La maquina de estados del enemigo de Skinwalker.
    /// Se encarga de inicializar el comportamiento o los diferentes estados de nuestro enemigo.
    /// </summary>
    
    public class SkinwalkerStateMachine : StateManager<SkinwalkerStateMachine.EnemyState>
    {
        #region Estados 
        public enum EnemyState
        {
            Idle,
            Search,
            Transform,
            Chasing,
            Attack,
            Stunned,
            Dead
        }
        #endregion

        //Banderas que son utilizadas en toda la maquina de estados
        #region Variables y Atributos
        private EnemySkinWalkerManager manager;
        public Dictionary<int , GameObject> playerInfo = new Dictionary<int, GameObject>();
        public bool PlayerOnAreaClose { get; set; }
        public bool PlayerOnAreaFar { get; set; }
        public Vector3 actualTarget { get; set; }
        public Vector3 PlayerPosition { get; set; }
        public GameObject PlayerGameObject { get; set; }
        public float DistanceToPlayer { get; set; }
        public bool IsTransformed { get; set; }
        public float TimeTransformed { get; set; }
        public bool IsStunned { get; set; }
        public float TimeStunned { get; set; }
        public bool Attacking { get; set; }
        public float TimeOfAttack { get; set; }
        public float AgressiveCounter { get; set; }
        public float AggresiveDuration { get; set; }
        public int AggresiveMode { get; set; }
        public float SearchCounter { get; set; }

        #endregion

        //Estadisticas actuales del enemigo
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
            manager = GetComponent<EnemySkinWalkerManager>();

            // Initialize the state machine
            state.Add(EnemyState.Idle, new SkinwalkerIdle(manager,this));
            state.Add(EnemyState.Search, new SkinwalkerSearch(manager, this));
            state.Add(EnemyState.Chasing, new SkinwalkerChasing(manager, this));
            state.Add(EnemyState.Transform, new SkinwalkerTransform(manager,this));
            state.Add(EnemyState.Attack, new SkinwalkerAttack(manager,this));
            state.Add(EnemyState.Stunned, new SkinwalkerStunned(manager,this));
            state.Add(EnemyState.Dead, new SkinwalkerDead(manager,this));

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
            // base.OnTriggerEnter(other);
            if (other.CompareTag("Player"))
            {
                if (!playerInfo.ContainsKey(other.gameObject.GetPhotonView().Owner.ActorNumber))
                {
                    playerInfo.Add(other.gameObject.GetPhotonView().Owner.ActorNumber, other.gameObject);
                }
                else 
                {
                    playerInfo[other.gameObject.GetPhotonView().Owner.ActorNumber] = other.gameObject;
                }
            }
        }
        public override void OnTriggerStay(Collider other)
        {
            // base.OnTriggerStay(other);
        }
        public override void OnTriggerExit(Collider other)
        {
            // base.OnTriggerExit(other);
            playerInfo[other.gameObject.GetPhotonView().Owner.ActorNumber] = null;
        }
        // public void OnCollisionEnter(Collision other)
        // {  
        //     if(currentState.StateKey == EnemyState.Chasing)
        //     {
        //         if(other.gameObject.tag == "Player")
        //         {
        //             Attacking = true;
        //         }
        //     }
        // }

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
        public void UpdateAttackTime()
        {
            TimeOfAttack += Time.deltaTime;
        }
        public void UpdateSearchTimer()
        {
            SearchCounter += Time.deltaTime;
        }
        public void UpdateTimeTransform()
        {
            TimeTransformed += Time.deltaTime;
            if(TimeTransformed >= 30f)
            {
                TimeTransformed = 0f;
                IsTransformed = false;
                //Activamos el modelo del enemigo y desactivamos su verdadero
                manager.enemyRealModel.SetActive(false);
                //Desactivamos el gameObject que contiene el modelo fantasma del enemigo
                manager.enemyFakeModel.SetActive(true);
            }
        }
        public void EnemyDead()
        {
            Destroy(manager.gameObject);
        }
        #endregion

        #region Update Dictionary

        public void ViewOnAreaFarPlayers()
        {
            //Recorremos el diccionario y revisamos si todos estan nulos o no
            foreach (var player in playerInfo)
            {
                if (player.Value != null)
                {
                    PlayerOnAreaFar = true;
                }
                else
                {
                    PlayerOnAreaFar = false;
                }
            }
        }

        public void ViewOnAreaClosePlayers()
            {
                bool detected = false;
                //Recorremos el diccionario y verificamos las que no son nulas. A cada quien obtenemos y calculamos la distanciapara ver si hay alguien menor o igual a nuestra zona de ataque
                foreach (var player in playerInfo)
                {
                    if (player.Value != null)
                    {
                        DistanceToPlayer = Vector3.Distance(player.Value.transform.position, transform.position);
                        if (DistanceToPlayer <= currentAttackRange)
                        {
                            PlayerOnAreaClose = true;
                            detected = true;
                        }
                        else
                        {
                            if(detected) break;
                            PlayerOnAreaClose = false;
                        }
                    }
                }
            }

        public void NearPlayer()
        {
            var playerdistance = 0f;
            var playerNear = float.MaxValue;
            Vector3 playerPos = Vector3.zero;

            foreach (var player in playerInfo)
            {
                if (player.Value != null)
                {
                    playerdistance = Vector3.Distance(player.Value.transform.position, transform.position);

                    if (playerdistance <= 20f)
                    {
                        Attacking = true;
                        // Usa el ViewID en lugar del ActorNumber
                        photonView.RPC(nameof(SyncAttack), RpcTarget.All, player.Value.GetComponent<PhotonView>().ViewID,player.Value.GetComponent<PhotonView>().Owner.ActorNumber);
                        return;
                    }

                    if (playerdistance <= playerNear)
                    {
                        playerNear = playerdistance;
                        playerPos = player.Value.transform.position;
                    }
                }
            }

            // Actualiza el objetivo más cercano
            actualTarget = playerPos;
            manager.agent.SetDestination(actualTarget);
        }

        #endregion

        #region DamageFuntions


        [PunRPC]
        public void SyncAttack(int viewID, int ActorNumber)
        {
            PhotonView targetView = PhotonView.Find(viewID);
            if (targetView != null && ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                targetView.GetComponent<PUNPlayerSanity>().TakeDamage(10, "SkinWalker");
            }
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
        #endregion

        // Llamamos a la funcion de OnDrwaGizmos
        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawSphere(manager.enemyRealModel.transform.position, manager.enemyStats.AttackRange);
        // }

    }
}
