using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Enemy.Behaviour;
using Enemy.Stats;

namespace Enemy.Manager
{
    /// <summary>
    /// Creamos una clase abstracta genrica que controla el flujo del enemigo.
    /// Los metodos abstractos los define cada clase que derive de esta
    /// La clase generica es 
    /// </summary>

    public abstract class EnemyBlueprintManager<T> : MonoBehaviour where T : System.Enum
    {

        #region Variables
        //Atributos de AI
        public NavMeshAgent agent {get; set;}
        public StateManager<T> enemyMachine {get; set;}

        //Atrivutos de estadisticas
        private EnemyScriptableObject enemyStats {get; set;}

        //Atributos de navegacion
        [SerializeField] public List<Transform> waypoints;

        #endregion

        #region Comportamientos
        public abstract void StartMachine();
        #endregion

        // #region Constructor

        // protected EnemyBlueprintManager(NavMeshAgent myAgent, StateManager<T> myEnemyMachine, EnemyScriptableObject myEnemyStats)
        // {
        //     this.agent = myAgent;
        //     this.enemyMachine = myEnemyMachine;
        //     this.enemyStats = myEnemyStats;
        // }

        // #endregion

        /// <summary>
        /// Este metodo crea un movimento definido de manera pseudo aleatoria de cada agente.
        /// Recibe una lista de puntos (target) que son los puntos de patrullaje.
        /// Finalmente elige un punto de la lista al azar y se mueve
        /// </summary>
        /// <param name="targets"></param>
        // public void IDLE(List<Transform> targets)
        // {
        //     //Elegimos un punto al azar de la lista y nos movemos
        //     var actualTarget = targets[Random.Range(0,targets.Count-1)];
        //     agent.SetDestination(actualTarget.position);
        // }

        // public void Dead()
        // {
        //     //El enemigo muere y desaparece
        //     Destroy(gameObject);
        // }

        // public void Search()
        // {
        //     //Definimos un area apartir de la ubicacion actual del agente
        //     var area = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);
        //     var searchRadius = 10.0f;
        //     //El enemigo busca en la area definida durante un tiempo
        //     agent.SetDestination(area + Random.insideUnitSphere * searchRadius);
        // }

        // public void Fuzzle(GameObject objeto)
        // {
        //     //Logica que se aturde dependiendo del objeto y se detiene el enemigo por un cierto tiempo
        //     agent.isStopped = false;
        // }

        // //Metodos abstractos que varian por enemigo
        // public abstract void Aggresive();
        // public abstract void Hunt();
        // public abstract void Attack();
        
    }
}