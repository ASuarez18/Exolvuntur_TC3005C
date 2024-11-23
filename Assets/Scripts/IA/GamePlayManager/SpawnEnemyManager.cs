using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Enemy.Manager;

namespace GamePlay.IA
{
    //TODO: Para la creacion de enemigos se puede implementar un Object Pool para crear al inicio.
    public class SpawnEnemyManager : MonoBehaviour
    {
        //Atributos de inicializacion
        [SerializeField] private List<Transform> _patrolPoints;
        [SerializeField] private int Kormos, Skinwalker, Dybbuk;
        private UnityEngine.AI.NavMeshTriangulation triangulation;

        //Creamos un singleton
        private static SpawnEnemyManager _instance;
        public static SpawnEnemyManager Instance
        {
            get
            {
                //Verificamos que este en la escena
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SpawnEnemyManager>();
                    //Si no esta en la escena , creamos un nuevo SpawnEnemyManager
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("SpawnEnemyManager");
                        _instance = go.AddComponent<SpawnEnemyManager>();
                    }
                }
                return _instance;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            triangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
            SpawnEnemy();
        }

        public void SpawnEnemy(string nameEnemy = null)
        {
            //Si observamos que el parametro manuel (nameEnemy) esta nulo creamos los enemigos al inicio
            if(nameEnemy == null)
            {
                //TODO: Refactorizar , se repite el mismo codigo varias veces
                for (int i = 0; i < Kormos; i++)
                {
                    //Elegimos un punto aleatorio
                    Transform spawnPoint = _patrolPoints[UnityEngine.Random.Range(0, _patrolPoints.Count)];
                    GameObject enemy = EnemiesPools.Instance.GetEnemy("EnemyKormos");
                    DoSpawnNavMeshAgent(enemy);
                    enemy.GetComponent<EnemyKormosManager>().waypoints = _patrolPoints;
                }
                for (int i = 0; i < Skinwalker; i++)
                {
                    //Elegimos un punto aleatorio
                    Transform spawnPoint = _patrolPoints[UnityEngine.Random.Range(0, _patrolPoints.Count)];
                    GameObject enemy = EnemiesPools.Instance.GetEnemy("EnemySkinwalker");
                    DoSpawnNavMeshAgent(enemy);
                    enemy.GetComponent<EnemySkinWalkerManager>().waypoints = _patrolPoints;
                }
                for (int i = 0; i < Dybbuk; i++)
                {
                    //Elegimos un punto aleatorio
                    Transform spawnPoint = _patrolPoints[UnityEngine.Random.Range(0, _patrolPoints.Count)];
                    GameObject enemy = EnemiesPools.Instance.GetEnemy("EnemyDybbuk");
                    DoSpawnNavMeshAgent(enemy);
                    enemy.GetComponent<EnemyDybbukManager>().waypoints = _patrolPoints;
                }
            }
            //Si observamos que el parametro manuel (nameEnemy) no esta nulo creamos el enemigo especificado
            else
            {
                //Creamos el enemigo segun el nombre recibido
                Transform spawnPoint = _patrolPoints[UnityEngine.Random.Range(0, _patrolPoints.Count)];
                GameObject enemy = EnemiesPools.Instance.GetEnemy(nameEnemy);
                DoSpawnNavMeshAgent(enemy);
                switch(nameEnemy)
                {
                    case "EnemyKormos":
                        enemy.GetComponent<EnemyKormosManager>().waypoints = _patrolPoints;
                        break;
                    case "EnemySkinwalker":
                        enemy.GetComponent<EnemySkinWalkerManager>().waypoints = _patrolPoints;
                        break;
                    case "EnemyDybbuk":
                        enemy.GetComponent<EnemyDybbukManager>().waypoints = _patrolPoints;
                        break;
                }
            }
        }

        private void DoSpawnNavMeshAgent(GameObject enemy)
        {
            UnityEngine.AI.NavMeshHit Hit;
            int VerticesIndex = UnityEngine.Random.Range(0, triangulation.vertices.Length);

            //Obtenemos un punto aleatorio en la malla de navegacion
            if(UnityEngine.AI.NavMesh.SamplePosition(triangulation.vertices[VerticesIndex], out Hit, 2.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                //Asignamos la posicion al NavMeshAgent
                enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(Hit.position);
                enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            }
        }

    }
}
