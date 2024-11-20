using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace GamePlay.IA
{
    /// <summary>
    /// Este script se encarga de inicializar y crear objetos de tipo enemigo en la escena.
    /// Utilizaremos un singleton que controle en toda la escena la instanción de enemigos.
    /// A su vex usaremos la tecnica de un pool de objetos para un mejor rendimiento
    /// </summary>
    
    public class EnemiesPools : MonoBehaviour
    {
        //Atributos de inicializacion
        [SerializeField] private int KormosEnemies;
        [SerializeField] private int SkinwalkerEnemies;
        [SerializeField] private int DybbukEnemies;
        [SerializeField] private List<GameObject> enemies;

        //Atributos del metodo o patron fabrica
        [SerializeField] private EnemiesInterfaceFactory _enemyFactory;

        //Creamos un singleton
        private static EnemiesPools _instance;
        public static EnemiesPools Instance
        {
            get
            {
                //Verificamos que este en la escena
                if (_instance == null)
                {
                    _instance = FindObjectOfType<EnemiesPools>();
                    //Si no esta en la escena , creamos un nuevo EnemiesPools
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("EnemiesPools");
                        _instance = go.AddComponent<EnemiesPools>();
                    }
                }
                return _instance;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //Inicializamos la lista de enemigos
            enemies = new List<GameObject>();
            //Agregamos los enemigos a la lista
            AddEnemies(KormosEnemies, SkinwalkerEnemies, DybbukEnemies);
        }


        //Creamos una funcion que se encarga de crear instancias de enemigos
        private void AddEnemies(int kAmount, int sAmount, int dAmount)
        {
            for(int i = 0; i < kAmount; i++)
            {
                //Instanciamos un enemigo Kormos a traves de la fabrica de enemigos
                GameObject kormos = _enemyFactory.InstantiateEnemies(EnemiesTypes.EnemyClass.Kormos, this.transform.position, Quaternion.identity);
                kormos.SetActive(false);
                //Agregamos el enemigo a la lista
                enemies.Add(kormos);
                //Agregamos el enemigo al padre en jerarquia
                kormos.transform.parent = this.transform;
            }
            for(int i = 0; i < sAmount; i++)
            {
                //Instanciamos un enemigo Skinwalker
                GameObject skinwalker = _enemyFactory.InstantiateEnemies(EnemiesTypes.EnemyClass.Skinwalker, this.transform.position, Quaternion.identity);
                skinwalker.SetActive(false);
                //Agregamos el enemigo a la lista
                enemies.Add(skinwalker);
                //Agregamos el enemigo al padre en jerarquia
                skinwalker.transform.parent = this.transform;
            }
            for(int i = 0; i < dAmount; i++)
            {
                //Instanciamos un enemigo Dybbuk
                GameObject dybbuk = _enemyFactory.InstantiateEnemies(EnemiesTypes.EnemyClass.Dybbuk, this.transform.position, Quaternion.identity);
                dybbuk.SetActive(false);
                //Agregamos el enemigo a la lista
                enemies.Add(dybbuk);
                //Agregamos el enemigo al padre en jerarquia
                dybbuk.transform.parent = this.transform;
            }
        }
        
        //Creamos un metodo que nos permita obtener un enemigo de la lista
        public GameObject GetEnemy(string enemyType)
        {
           //Recorremos la lista de enemigos
            foreach(GameObject enemy in enemies)
            {
                if(!enemy.activeSelf && enemy.CompareTag(enemyType))
                {
                    //Si encontramos un enemigo inactivo, lo retornamos
                    enemy.SetActive(true);
                    //Devolvemos el enemigo
                    return enemy;
                }
            }

            //Si no encontramos un enemigo en la lista, creamos un nuevo enemigo
            switch(enemyType)
            {
                case "EnemyKormos":
                    AddEnemies(1,0,0);
                    break;
                case "EnemySkinwalker":
                    AddEnemies(0,1,0);
                    break;
                case "EnemyDybbuk":
                    AddEnemies(0,0,1);
                    break;
            }

            enemies[enemies.Count - 1].SetActive(true);
            //Si no encontramos un enemigo en la lista, retornamos null
            return enemies[enemies.Count - 1];
        }
    }
}