using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Pool;
using Photon.Pun;

namespace GamePlay.IA
{
    /// <summary>
    /// Este script se encarga de inicializar y crear objetos de tipo enemigo en la escena.
    /// Utilizaremos un singleton que controle en toda la escena la instanción de enemigos.
    /// A su vez usaremos la tecnica de un pool de objetos para un mejor rendimiento a traves de la red.
    /// </summary>
    
    public class PUNEnemiesPools : MonoBehaviour
    {
        //Atributos de inicializacion
        [SerializeField] private int KormosEnemies;
        [SerializeField] private int SkinwalkerEnemies;
        [SerializeField] private int DybbukEnemies;
        [SerializeField] private List<GameObject> enemies;

        //Pool de objetos para los enemigos
        private PhotonView photonView;

        //Creamos un pool para cada enemigo
        private ObjectPool<GameObject> KormosPool;
        private ObjectPool<GameObject> SkinwalkerPool;
        private ObjectPool<GameObject> DybbukPool;

        //Atributos del metodo o patron fabrica
        [SerializeField] private EnemiesInterfaceFactory _enemyFactory;

        //Creamos un diccionario que guarde la llave de cada enemigo (Enmun) y su valor (Funcion de creacion)
        private Dictionary<EnemiesTypes.EnemyClass, Func<GameObject>> _enemiesFactoryInstance;

        //Creamos un singleton
        private static PUNEnemiesPools _instance;
        public static PUNEnemiesPools Instance
        {
            get
            {
                //Verificamos que este en la escena
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PUNEnemiesPools>();
                    //Si no esta en la escena , creamos un nuevo EnemiesPools
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("EnemiesPools");
                        _instance = go.AddComponent<PUNEnemiesPools>();
                    }
                }
                return _instance;
            }
        }

        void Awake()
        {
            //Incializamos el diccionario
            _enemiesFactoryInstance = new Dictionary<EnemiesTypes.EnemyClass, Func<GameObject>>()
            {
                { EnemiesTypes.EnemyClass.Kormos, () => _enemyFactory.InstantiateEnemies(EnemiesTypes.EnemyClass.Kormos, this.transform.position, Quaternion.identity)},
                { EnemiesTypes.EnemyClass.Skinwalker, () => _enemyFactory.InstantiateEnemies(EnemiesTypes.EnemyClass.Skinwalker, this.transform.position, Quaternion.identity)},
                { EnemiesTypes.EnemyClass.Dybbuk, () => _enemyFactory.InstantiateEnemies(EnemiesTypes.EnemyClass.Dybbuk, this.transform.position, Quaternion.identity)}
            };

            //Obtenemos el photon view de nuesto pool de enemigos (singleton)
            photonView = GetComponent<PhotonView>();
            //El master Cliente inicializa cada uno de los pool de enemigos con el pool generico
            if(PhotonNetwork.IsMasterClient)
            {
                //Inicializamos el pool de enemigos
                KormosPool = InitializePool(EnemiesTypes.EnemyClass.Kormos, KormosEnemies);
                SkinwalkerPool = InitializePool(EnemiesTypes.EnemyClass.Skinwalker, SkinwalkerEnemies);
                DybbukPool = InitializePool(EnemiesTypes.EnemyClass.Dybbuk, DybbukEnemies);

                //Sincronizamos los pool de enemigos en la red
                photonView.RPC(nameof(SyncronizedPools), RpcTarget.OthersBuffered);

            }
        }


        //Creamos una funcion para iniicializar el pool generico de enemigos (RPC)
        private ObjectPool<GameObject> InitializePool(EnemiesTypes.EnemyClass enemyClass, int size)
        {
            Debug.Log(enemyClass + " pool " + size );
            //Inicializamos el pool de objetos
            return new ObjectPool<GameObject>(
                //Funcion para crear un nuevo objeto
                createFunc: () => _enemiesFactoryInstance[enemyClass](),
                //Funcion para activar un objeto
                actionOnGet: enemy => enemy.SetActive(true),
                //Funcion para desactivar un objeto
                actionOnRelease: enemy => 
                {
                    enemy.SetActive(false);
                    enemy.transform.position = this.transform.position;
                },
                //Funcion para destruir un objeto
                actionOnDestroy: enemy => Destroy(enemy),
                //Tamaño maximo del pool
                maxSize: size
                
                );
        }

        [PunRPC]
        //Creamos una funcion para sincronizar la creacion de enemigos en la red (RPC)
        public void SyncronizedPools()
        {
            //Inicializamos el pool de enemigos
            KormosPool = InitializePool(EnemiesTypes.EnemyClass.Kormos, KormosEnemies);
            SkinwalkerPool = InitializePool(EnemiesTypes.EnemyClass.Skinwalker, SkinwalkerEnemies);
            DybbukPool = InitializePool(EnemiesTypes.EnemyClass.Dybbuk, DybbukEnemies);
        }

        //Creamos una funcion que se encarga de crear instancias de enemigos (RPC)
        [PunRPC]
        public GameObject GetEnemies(EnemiesTypes.EnemyClass enemyClass)
        {
            switch (enemyClass)
            {
                case EnemiesTypes.EnemyClass.Kormos:
                    GameObject kormos = KormosPool.Get();
                    kormos.transform.SetParent(transform);
                    return kormos;
                case EnemiesTypes.EnemyClass.Skinwalker:
                    GameObject skinwalker = SkinwalkerPool.Get();
                    skinwalker.transform.SetParent(transform);
                    return skinwalker;
                case EnemiesTypes.EnemyClass.Dybbuk:
                    GameObject dybbuk = DybbukPool.Get();
                    dybbuk.transform.SetParent(transform);
                    return dybbuk;
                default:
                    return null;
            }
        }
        

        //Creamos una funcion que se encarga de liberar instancias de enemigos (RPC)
        [PunRPC]
        public void ReleaseEnemies(string enemyName , GameObject enemy)
        {
            switch (enemyName)
            {
                case "EnemyKormos":
                    KormosPool.Release(enemy);
                    break;
                case "EnemySkinwalker":
                    SkinwalkerPool.Release(enemy);
                    break;
                case "EnemyDybbuk":
                    DybbukPool.Release(enemy);
                    break;
            }
        }
    }
}