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
            public Dictionary<int , GameObject> playerInfo = new Dictionary<int, GameObject>();
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
                //Primero verificamos si nuestro diccionario contiene el actor number
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
                //Recorremos los jugadores en el diccionario y updateamos su posicion unicamente siempre y cuando su posicion no sea null
            }
                
            public override void OnTriggerExit(Collider other)
            {
                // base.OnTriggerExit(other);
                playerInfo[other.gameObject.GetPhotonView().Owner.ActorNumber] = null;
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

        [PunRPC]
        public void SyncAttack(int viewID, int ActorNumber)
        {
            PhotonView targetView = PhotonView.Find(viewID);
            if (targetView != null && ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                targetView.GetComponent<PUNPlayerSanity>().TakeDamage(10, "Kormos");
            }
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


