using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Creamos una clase abstracta que defina metodos abstractos y metodos fijos para cada uno de los enemigos
/// </summary>
public abstract class EnemyMovement 
{
    //Atributos de AI
    private NavMeshAgent agent;

    /// <summary>
    /// Este metodo crea un movimento definido de manera pseudo aleatoria de cada agente.
    /// Recibe una lista de puntos (target) que son los puntos de patrullaje.
    /// Finalmente elige un punto de la lista al azar y se mueve
    /// </summary>
    /// <param name="targets"></param>
    public void enemyDeafaultMovementation(List<Transform> targets)
    {
        //Elegimos un punto al azar de la lista y nos movemos
        var actualTarget = targets[Random.Range(0,targets.Count-1)];
        agent.SetDestination(actualTarget.position);
    }
}
