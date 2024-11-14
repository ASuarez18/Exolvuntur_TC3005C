// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// /// <summary>
// /// El enemigo hereda de la clase abstracta de EnemyBlueprint.
// /// Define su propia maquina de estados mientras utiliza metodos abstractos de la interfaz.
// /// </summary>
// public class EnemyDybbuk : EnemyBlueprint
// {
//     //Atributos de AI
//     private NavMeshAgent agent;
//     private Vector3 destination;

//     //Atributos de patrullaje
//     [SerializeField] List<Transform> _patrullaje;


//     // Start is called before the first frame update
//     void Start()
//     {
//         //Llamamos a la funcion de movimiento
//         enemyDefaultMovement(_patrullaje);
//         destination = agent.destination;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //Si el enemigo llega al destino, elegimos otro destino al azar
//         if (agent.remainingDistance < 0.5f)
//         {
//             //Cuando este cerca de llegar elegimos un nuevo punto
//             enemyDefaultMovement(_patrullaje);
//             //Verifcamos que el destino que eligio no sea igual al pasado
//             while (agent.destination == destination)
//             {
//                 enemyDefaultMovement(_patrullaje);
//             }
//         }
//     }
// }

