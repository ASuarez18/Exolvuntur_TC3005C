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

        //Pool de objetos para los enemigos
        private PhotonView photonView;

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

            photonView = GetComponent<PhotonView>();

            // Buscamos el componente PUNEnemiesPools en la escena
            _enemiesPunPools = FindObjectOfType<PUNEnemiesPools>();

            // Solo el MasterClient inicializa el spawn de enemigos
            // photonView.RPC(nameof(SpawnEnemies), RpcTarget.AllBuffered);



            SpawnEnemies();
            
        }

        /// <summary>
        /// Spawnea enemigos al inicio según las configuraciones.
        /// </summary>
        
        public void SpawnEnemies()
        {
            //El master client ejecuta e inicializa el lugar de los enemigos al inicio según las configuraciones
            if(!PhotonNetwork.IsMasterClient)return; 
        
            // Spawnea enemigos de cada clase
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
                Vector3 spawnPoint = _patrolPoints[Random.Range(0, _patrolPoints.Count)].position;
                photonView.RPC(nameof(SyncronizedLocationEnemy), RpcTarget.AllBuffered, spawnPoint, enemyClass);
            }
        }

        [PunRPC]
        private void SyncronizedLocationEnemy(Vector3 pointEnemy,EnemiesTypes.EnemyClass enemyClass)
        {
            GameObject enemy = _enemiesPunPools.GetEnemies(enemyClass); // Llama al pool para obtener el enemigo
            Debug.LogWarning("Spawning " + enemyClass + " at " + pointEnemy);
            DoSpawnNavMeshAgent(enemy, pointEnemy);
            AssignWaypoints(enemy, enemyClass);
        }

        /// <summary>
        /// Creamos una RPC que sincronice el lugar de spawner para cada uno de los enemigos.
        /// Mandamos a llamar la funcion de remote procedural calls cada vez que un enemigo es spawneado por el master.
        /// </summary>
        /// <param name="enemyClass">Clase de enemigo.</param>
        // [PunRPC]
        // private void SynchronizeEnemySpawn(int enemyClass, Vector3 position)
        // {
        //     // Instanciar un enemigo con la posición sincronizada
        //     GameObject enemy = _enemiesPunPools.GetEnemies((EnemiesTypes.EnemyClass)enemyClass);
        //     Debug.Log("Spawning " + (EnemiesTypes.EnemyClass)enemyClass + " at " + position);
        //     DoSpawnNavMeshAgent(enemy, position);
        //     AssignWaypoints(enemy, (EnemiesTypes.EnemyClass)enemyClass);
        // }

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
                 PhotonView enemyPhotonView = enemy.GetComponent<PhotonView>();
                if (enemyPhotonView != null && PhotonNetwork.IsMasterClient)
                {
                    enemyPhotonView.TransferOwnership(PhotonNetwork.MasterClient);
                }
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