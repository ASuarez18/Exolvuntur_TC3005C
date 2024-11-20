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
        [SerializeField] private List<Transform> _spawnPoints;
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

        public void SpawnEnemy(string nameEnemy = null)
        {
            //Si observamos que el parametro manuel (nameEnemy) esta nulo creamos los enemigos al inicio
            if(nameEnemy == null)
            {
                //TODO: Refactorizar , se repite el mismo codigo varias veces
                for (int i = 0; i < Kormos; i++)
                {
                    //Elegimos un punto aleatorio
                    Transform spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)];
                    GameObject enemy = EnemiesPools.Instance.GetEnemy("EnemyKormos");
                    enemy.transform.position = spawnPoint.position;
                    enemy.transform.rotation = spawnPoint.rotation;
                }
                for (int i = 0; i < Skinwalker; i++)
                {
                    //Elegimos un punto aleatorio
                    Transform spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)];
                    GameObject enemy = EnemiesPools.Instance.GetEnemy("EnemySkinwalker");
                    enemy.transform.position = spawnPoint.position;
                    enemy.transform.rotation = spawnPoint.rotation;
                }
                for (int i = 0; i < Dybbuk; i++)
                {
                    //Elegimos un punto aleatorio
                    Transform spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)];
                    GameObject enemy = EnemiesPools.Instance.GetEnemy("EnemyDybbuk");
                    enemy.transform.position = spawnPoint.position;
                    enemy.transform.rotation = spawnPoint.rotation;
                }
            }
            //Si observamos que el parametro manuel (nameEnemy) no esta nulo creamos el enemigo especificado
            else
            {
                //Creamos el enemigo segun el nombre recibido
                Transform spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)];
                GameObject enemy = EnemiesPools.Instance.GetEnemy(nameEnemy);
                enemy.transform.position = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;   
            }
        }

    }
}
