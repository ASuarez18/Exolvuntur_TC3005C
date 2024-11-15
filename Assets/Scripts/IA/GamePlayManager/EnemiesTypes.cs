using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Este codigo contiene n cantidad de fabricas por la cantidad de enemigos que existen.
/// Cada fabrica herdad de la interfaz y sus metodos permitiendo definirlos de la manera que quieran.
/// </summary>
public static class EnemiesTypes
{
   public enum EnemyClass : byte
   {
     Kormos,
     Skinwalker,
     Dybbuk
   }
}
