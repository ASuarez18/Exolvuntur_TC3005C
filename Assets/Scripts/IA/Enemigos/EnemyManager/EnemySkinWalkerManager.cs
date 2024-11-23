using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enemy.Behaviour;
using Enemy.Stats;

/// <summary>
/// El enemigo Skinwalker es administrado y configurado por este script.
/// El enemigo Skinwalker es un enemigo que se transforma en un modelo de cualquiera de nuestros personajes principales.
/// </summary>

namespace Enemy.Manager
{
        
    public class EnemySkinWalkerManager : MonoBehaviour
    {
        //Atributos de AI
        [SerializeField] public NavMeshAgent agent;
        [SerializeField] public SkinwalkerStateMachine enemyMachine;
        [SerializeField] public GameObject enemyRealModel;
        [SerializeField] public GameObject enemyFakeModel;
       
        [SerializeField] public Animator animator; 

        //Atrivutos de sensores
        public SphereCollider areaAlerta;

        //Atrivutos de estadisticas
        [SerializeField] public EnemyScriptableObject enemyStats;
        [SerializeField] public List<Transform> waypoints;

        //Crear un constructor
        public EnemySkinWalkerManager(NavMeshAgent myAgent, EnemyScriptableObject myEnemyStats, SkinwalkerStateMachine myEnemyMachine, Animator myAnimator)
        {
            //Asignar valores a los atributos
            agent = myAgent;
            enemyMachine = myEnemyMachine;
            enemyStats = myEnemyStats;
            animator = myAnimator;
        }

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

         void Start()
        {
            //Accedemos al hijo y obtenemos el componenete de collider
            areaAlerta = transform.GetChild(0).GetComponent<SphereCollider>();
            areaAlerta.radius = enemyStats.ViewRange;

            //Ejecutamos el primer estado de nuestra maquina de estados
            enemyMachine.SwitchCase(SkinwalkerStateMachine.EnemyState.Idle);
        }     
    }
}
