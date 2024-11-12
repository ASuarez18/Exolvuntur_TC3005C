using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enemy.Behaviour;
using Enemy.Stats;

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

        //Atrivutos de sensores
        public SphereCollider areaAlerta;
        // public bool areaAtaque;
        // public RaycastHit hit;
        // public LayerMask layerMask;

        //Atrivutos de estadisticas
        [SerializeField] public EnemyScriptableObject enemyStats;
        [SerializeField] public List<Transform> waypoints;

        //Eventos
        public TriggerChilds triggerEvent;

         public void OnEnable()
        {
            //Suscribimos los Trigger al evento principal
            triggerEvent.TriggerEventEnter += enemyMachine.OnTriggerEnter;
            triggerEvent.TriggerEventStay += enemyMachine.OnTriggerStay;
            triggerEvent.TriggerEventExit += enemyMachine.OnTriggerExit;
        }

        public void OnDisable()
        {
            triggerEvent.TriggerEventEnter -= enemyMachine.OnTriggerEnter;
            triggerEvent.TriggerEventStay -= enemyMachine.OnTriggerStay;
            triggerEvent.TriggerEventExit -= enemyMachine.OnTriggerExit;
        }

        //Generamos un constructor de la instancia de la clase
        public EnemyKormosManager(NavMeshAgent myAgent,EnemyScriptableObject myEnemyStats, KormosStateMachine myEnemyMachine)
        {
            //Asignamos los valores a los atributos
            agent = myAgent;
            enemyMachine = myEnemyMachine;
            enemyStats = myEnemyStats;
        }

        //Inicializamos la funcion

        void Start()
        {
            //Accedemos a los hijos del enemigo (Alerta y Ataque)
            areaAlerta = transform.Find("Alerta").GetComponent<SphereCollider>();
            areaAlerta.radius = enemyStats.ViewRange;

            //Ejecutamos el primer estado de nuestra maquina de estados
            enemyMachine.SwitchCase(KormosStateMachine.EnemyState.Idle);
        }

        // void Update()
        // {
        //     if(Physics.SphereCast(transform.position,enemyStats.AttackRange/2,transform.forward,out hit,0,layerMask))
        //     {
        //         Debug.Log(hit.collider.gameObject.name);
        //     }
        // }

        // void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawSphere(transform.position,enemyStats.AttackRange/2);
        // }
        

    }
}