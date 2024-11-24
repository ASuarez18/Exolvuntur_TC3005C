using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Enemy.Manager;

namespace GamePlay.IA
{
    public class PUNSpawnEnemyManager : MonoBehaviour
    {
        // Atributos de inicialización
        [SerializeField] private List<Transform> _patrolPoints;
        [SerializeField] private int Kormos, Skinwalker, Dybbuk;
        private UnityEngine.AI.NavMeshTriangulation triangulation;
        public List<Transform> spawnPoints;
        [SerializeField] private PUNEnemiesPools _enemiesPunPools;

        // Singleton
        private static PUNSpawnEnemyManager _instance;
        public static PUNSpawnEnemyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PUNSpawnEnemyManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("SpawnEnemyManager");
                        _instance = go.AddComponent<PUNSpawnEnemyManager>();
                    }
                }
                return _instance;
            }
        }

        private void Start()
        {
            // Obtenemos el triangulado de la malla de navegación
            triangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();

            // Buscamos el componente PUNEnemiesPools en la escena
            _enemiesPunPools = FindObjectOfType<PUNEnemiesPools>();

            // Solo el MasterClient inicializa el spawn de enemigos
            if (PhotonNetwork.IsMasterClient)
            {
                SpawnEnemies();
            }
        }

        /// <summary>
        /// Spawnea enemigos al inicio según las configuraciones.
        /// </summary>
        public void SpawnEnemies()
        {
            SpawnEnemyBatch(Kormos, EnemiesTypes.EnemyClass.Kormos);
            SpawnEnemyBatch(Skinwalker, EnemiesTypes.EnemyClass.Skinwalker);
            SpawnEnemyBatch(Dybbuk, EnemiesTypes.EnemyClass.Dybbuk);
        }

        /// <summary>
        /// Spawnea una cantidad específica de enemigos de una clase.
        /// </summary>
        /// <param name="count">Cantidad de enemigos.</param>
        /// <param name="enemyClass">Clase de enemigo.</param>
        private void SpawnEnemyBatch(int count, EnemiesTypes.EnemyClass enemyClass)
        {
            for (int i = 0; i < count; i++)
            {
                Transform spawnPoint = _patrolPoints[Random.Range(0, _patrolPoints.Count)];
                GameObject enemy = _enemiesPunPools.GetEnemies(enemyClass); // Llama al pool para obtener el enemigo
                DoSpawnNavMeshAgent(enemy, spawnPoint.position);
                AssignWaypoints(enemy, enemyClass);
            }
        }

        /// <summary>
        /// Spawnea un enemigo específico en un punto de navegación aleatorio.
        /// </summary>
        /// <param name="enemy">Enemigo a spawnear.</param>
        /// <param name="position">Posición donde spawnear.</param>
        private void DoSpawnNavMeshAgent(GameObject enemy, Vector3 position)
        {
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(position, out hit, 2.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(hit.position);
                enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            }
        }

        /// <summary>
        /// Asigna waypoints al enemigo dependiendo de su clase.
        /// </summary>
        /// <param name="enemy">El enemigo al cual asignar waypoints.</param>
        /// <param name="enemyClass">Clase del enemigo.</param>
        private void AssignWaypoints(GameObject enemy, EnemiesTypes.EnemyClass enemyClass)
        {
            switch (enemyClass)
            {
                case EnemiesTypes.EnemyClass.Kormos:
                    enemy.GetComponent<EnemyKormosManager>().waypoints = _patrolPoints;
                    break;
                case EnemiesTypes.EnemyClass.Skinwalker:
                    enemy.GetComponent<EnemySkinWalkerManager>().waypoints = _patrolPoints;
                    break;
                case EnemiesTypes.EnemyClass.Dybbuk:
                    enemy.GetComponent<EnemyDybbukManager>().waypoints = _patrolPoints;
                    break;
            }
        }
    }

}