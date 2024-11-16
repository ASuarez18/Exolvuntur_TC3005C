using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Creamos una interfaz que permita crear distintas clases de enemigos.
/// Devuelve un GameObject (un enemigo)
/// </summary>
public class EnemiesInterfaceFactory : MonoBehaviour
{
   //We create a list with the enemies prefabs
   [SerializeField] private GameObject _kormosPrefab;
   [SerializeField] private GameObject _skinwalkerPrefab;
   [SerializeField] private GameObject _dybbukPrefab;

   // Diccionario para almacenar los prefabs de los enemigos según su tipo
   private Dictionary<EnemiesTypes.EnemyClass, GameObject> _enemyPrefabs;
   
   private void Awake()
   {
      // Inicializamos el diccionario aquí, ya que necesitamos que las referencias de los prefabs estén listas
      _enemyPrefabs = new Dictionary<EnemiesTypes.EnemyClass, GameObject>
      {
         { EnemiesTypes.EnemyClass.Kormos, _kormosPrefab },
         { EnemiesTypes.EnemyClass.Skinwalker, _skinwalkerPrefab },
         { EnemiesTypes.EnemyClass.Dybbuk, _dybbukPrefab }
      };
   }

   //We create a method to instantiate the enmies based on their type
   public void InstantiateEnemies(EnemiesTypes.EnemyClass _enemyClass,Vector3 enemies, Quaternion rotation)
   {
      switch(_enemyClass)
      {
         case EnemiesTypes.EnemyClass.Kormos:
            Instantiate(_enemyPrefabs[_enemyClass], enemies, rotation);
            break;
         case EnemiesTypes.EnemyClass.Skinwalker:
            Instantiate(_enemyPrefabs[_enemyClass], enemies, rotation);
            break;
         case EnemiesTypes.EnemyClass.Dybbuk:
            Instantiate(_enemyPrefabs[_enemyClass], enemies, rotation);
            break;
         default:
            Debug.LogError("Enemy class not found");
            break;  //In case the enemy type is not defined in the enum, we log an error and break the switch case.
      }
   }
}
