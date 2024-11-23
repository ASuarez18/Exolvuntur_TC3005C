using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enemy.Behaviour;
using Enemy.Stats;

namespace Enemy.Manager
{

    /// <summary>
    /// El enemigo es el Dybbuk (Acechador).
    /// Define su propia maquina de estados mientras utiliza metodos abstractos de la interfaz.
    /// </summary>
    
    public class EnemyDybbukManager : MonoBehaviour
    {
        //Atributos de AI
        [SerializeField] public NavMeshAgent agent;
        [SerializeField] public DybbukStateMachine enemyMachine;
        [SerializeField] public Renderer enemyRender;
        [SerializeField] public Animator animator;

        //Atrivutos de sensores
        public SphereCollider areaAlerta;

        //Atrivutos de estadisticas
        [SerializeField] public EnemyScriptableObject enemyStats;
        [SerializeField] public List<Transform> waypoints;

        //Generamos un constructor de la instancia de la clase
        public EnemyDybbukManager(NavMeshAgent myAgent,EnemyScriptableObject myEnemyStats, DybbukStateMachine myEnemyMachine, Animator myEnemyAnimation)
        {
            //Asignamos los valores a los atributos
            agent = myAgent;
            enemyMachine = myEnemyMachine;
            enemyStats = myEnemyStats;
            animator = myEnemyAnimation;
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
            enemyMachine.SwitchCase(DybbukStateMachine.EnemyState.Idle);
        }     
    }
}

