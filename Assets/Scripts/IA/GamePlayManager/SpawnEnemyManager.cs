using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GamePlay.IA
{
    //TODO: Para la creacion de enemigos se puede implementar un Object Pool para crear al inicio.
    public class SpawnEnemyManager : MonoBehaviour
    {
        //Atributos de inicializacion
        [SerializeField] private List<Transform> _targetPoints;
        [SerializeField] private EnemiesInterfaceFactory _enemyFactory;
        [SerializeField] private int Kormos, Skinwalker, Dybbuk;

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
            SpawnEnemy();
        }

        //TODO: refactorizar algormito de creacion
        public void SpawnEnemy()
        {
            for (int i = 0; i < Kormos; i++)
            {
                //Elegimos un punto aleatorio
                Transform spawnPoint = _targetPoints[UnityEngine.Random.Range(0, _targetPoints.Count-1)];
                _enemyFactory.InstantiateEnemies(EnemiesTypes.EnemyClass.Kormos, spawnPoint.position, spawnPoint.rotation);
            }
            for (int i = 0; i < Skinwalker; i++)
            {
                //Elegimos un punto aleatorio
                Transform spawnPoint = _targetPoints[UnityEngine.Random.Range(0, _targetPoints.Count-1)];
                _enemyFactory.InstantiateEnemies(EnemiesTypes.EnemyClass.Skinwalker, spawnPoint.position, spawnPoint.rotation);
            }
            for (int i = 0; i < Dybbuk; i++)
            {
                //Elegimos un punto aleatorio
                Transform spawnPoint = _targetPoints[UnityEngine.Random.Range(0, _targetPoints.Count-1)];
                _enemyFactory.InstantiateEnemies(EnemiesTypes.EnemyClass.Dybbuk, spawnPoint.position, spawnPoint.rotation);
            } 
        }

    }
}
