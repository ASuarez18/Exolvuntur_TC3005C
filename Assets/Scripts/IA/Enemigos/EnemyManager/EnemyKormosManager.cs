using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enemy.Behaviour;
using Enemy.Stats;
using Photon.Pun;

namespace Enemy.Manager
{
    /// <summary>
    /// El enemigo hereda de la clase abstracta de EnemyBlueprint.
    /// Define su propia maquina de estados mientras utiliza metodos abstractos de la interfaz.
    /// </summary>
    public class EnemyKormosManager : MonoBehaviourPunCallbacks
    {

        //Atributos de AI
        [SerializeField] public NavMeshAgent agent;
        [SerializeField] public KormosStateMachine enemyMachine;
        [SerializeField] public Animator animator;

        //Atrivutos de sensores
        public SphereCollider areaAlerta;

        //Atributos de sincronizacion en red
        public PhotonView photonView;

        public PhotonView photonAni;

        //Atrivutos de estadisticas
        [SerializeField] public EnemyScriptableObject enemyStats;
        [SerializeField] public List<Transform> waypoints;

        //Generamos un constructor de la instancia de la clase
        public EnemyKormosManager(NavMeshAgent myAgent,EnemyScriptableObject myEnemyStats, KormosStateMachine myEnemyMachine, Animator myEnemyAnimation)
        {
            //Asignamos los valores a los atributos
            agent = myAgent;
            enemyMachine = myEnemyMachine;
            enemyStats = myEnemyStats;
            animator = myEnemyAnimation;
        }

        //Inicializamos la suscripcion y desuscricion de los eventos
        [SerializeField] private TriggerSensor sensor;

        public override void OnEnable()
        {
            base.OnEnable();
            //Suscribimos los eventos
            sensor.TriggerEventEnter += enemyMachine.OnTriggerEnter;
            sensor.TriggerEventStay += enemyMachine.OnTriggerStay;
            sensor.TriggerEventExit += enemyMachine.OnTriggerExit;
        }

        void OnDisable()
        {
            //Desuscribimos los eventos
            sensor.TriggerEventEnter -= enemyMachine.OnTriggerEnter;
            sensor.TriggerEventStay -= enemyMachine.OnTriggerStay;
            sensor.TriggerEventExit -= enemyMachine.OnTriggerExit;
        }



        //Inicializamos la funcion

        void Start()
        {
            
            //Accedemos al hijo y obtenemos el componenete de collider
            areaAlerta = transform.GetChild(0).GetComponent<SphereCollider>();
            areaAlerta.radius = enemyStats.ViewRange;

            // Inicializamos su photon view
            photonView = GetComponent<PhotonView>();

            if(!PhotonNetwork.IsMasterClient) return;
            //Ejecutamos el primer estado de nuestra maquina de estados
            enemyMachine.SwitchCase(KormosStateMachine.EnemyState.Idle);
        }

        #region Remote Procedural Calls (Interactions)

        public void dectedsound(Vector3 soundPos)
        {
            photonView.RPC(nameof(ActivateSound), RpcTarget.MasterClient, soundPos);
        }

        public void StunActive()
        {
            photonView.RPC(nameof(StunActiveSync), RpcTarget.MasterClient);
        }

        public void ApplyDamageRemote(int value)
        {
            photonView.RPC(nameof(SyncDamage), RpcTarget.MasterClient,value);
        }

        public void Animatorfuc(string name)
        {
            photonView.RPC(nameof(AnimatorSync), RpcTarget.All, name);
        }

        [PunRPC]
        public void AnimatorSync(string name)
        {
            Debug.Log("Animator Sync");
            animator.SetTrigger(name);
        }


        [PunRPC]
        public void ActivateSound(Vector3 soundPos)
        {
            //Primero verificamos si el enemigo se encuntra en Idle
            if(enemyMachine.currentState is KormosCaution CautionState)
            { 
                enemyMachine.SoundDetected =true;
            }
            else if(enemyMachine.currentState is KormosHunt HuntState)
            {
                HuntState.Hunt(soundPos);
            }
        }

        [PunRPC]
        public void StunActiveSync()
        {
            enemyMachine.ApplyStun();
        }

        [PunRPC]
        public void SyncDamage(int value)
        {
            enemyMachine.ApplyDamage(value);
        }
    
        #endregion
    }
}