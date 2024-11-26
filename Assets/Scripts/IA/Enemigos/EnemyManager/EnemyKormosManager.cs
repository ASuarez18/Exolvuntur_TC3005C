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
    public class EnemyKormosManager : MonoBehaviour
    {

        //Atributos de AI
        [SerializeField] public NavMeshAgent agent;
        [SerializeField] public KormosStateMachine enemyMachine;
        [SerializeField] public Animator animator;

        //Atrivutos de sensores
        public SphereCollider areaAlerta;

        //Atributos de sincronizacion en red
        public PhotonView photonView;

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

        void OnEnable()
        {
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

            //Inicializamos su photon view
            photonView = GetComponent<PhotonView>();

            //Ejecutamos el primer estado de nuestra maquina de estados
            enemyMachine.SwitchCase(KormosStateMachine.EnemyState.Idle);

            // if(PhotonNetwork.IsMasterClient)
            // {
            //     //Lanzamos el evento para que todos los clientes se sincronizen con el estado inicial
            //     PhotonNetwork.RPC(PhotonTargets.AllBuffered,nameof(SyncEnemyState), enemyMachine.currentState);
            // }
            // else
            // {
            //     //Desactivamos el componente de NavMeshAgent
            //     agent.enabled = false;
            // }
        }

        //Creamos un RPC para que el master sincronice la maquina de estados del enemigo con los demas clientes
        // [PunRPC]
        // public void SyncEnemyState(KormosStateMachine.EnemyState state)
        // {
        //     enemyMachine.SwitchCase(state);
        // }        

    }
}