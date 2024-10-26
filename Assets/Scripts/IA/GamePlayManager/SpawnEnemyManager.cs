using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GamePlay.IA
{
    //TODO: Para la creacion de enemigos se puede implementar un Object Pool para crear al inicio.
    public class SpawnEnemyManager : MonoBehaviour
    {
        //Atributos
        [SerializeField] private List<Transform> _targetPoints;

        //Atributos de generacion de enemigos
        [Serializable]
        public struct EnemyData
        {
            public GameObject enemyPrefab;
            public int spawnAmount;
        }

        //Creamos una lista de los datos
        [SerializeField] private List<EnemyData> _enemyDataList;

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
            InstantiateEnemy();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        //TODO: refactorizar algormito de creacion
        //Instanciar enemigo
        public void InstantiateEnemy()
        {
            //?Metodo1
            //Recorremos la lista de enemigos
            foreach (var dataEnemy in _enemyDataList)
            {
                for(int i = 0; i < dataEnemy.spawnAmount; i++)
                {
                    //Generamos un punto aleatorio
                    int index = UnityEngine.Random.Range(0, _targetPoints.Count-1);
                    //Instanciamos un enemigo en el punto
                    GameObject enemy = Instantiate(dataEnemy.enemyPrefab, _targetPoints[index].position, Quaternion.identity);
                    enemy.transform.parent = this.transform; //Añadimos el enemigo al objeto SpawnEnemyManager para mantenerlo en la escena
                }
            }

        }

    }
}
