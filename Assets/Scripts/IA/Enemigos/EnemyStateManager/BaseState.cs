using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


/// <summary>
/// Esta clase abstracta percibe las diferentes partes de un estado
/// Es encargado de comunicar como esta compuesto un estado y como maneja su flujo interno
/// Es importante recordar que nada mas interactua con UN estado
/// </summary>

//Creamos un namespace que guarde comportamientos, estados o acciones
namespace Enemy.Behaviour
{
    //Clase abstracta que recibe un ENUM
    public abstract class BaseState<EState> where EState : Enum
    {
        //Constructor que recibe la llave del estado actual
        public BaseState(EState key)
        {
            StateKey = key; 
        }

        //Identificador o llave de la casa
        public EState StateKey { get; private set; }

        //Creamos metodos abstractos encargados manejar cada etapa del estado desde su inicio al fin
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
        public abstract EState GetNextState();

        //Creamos metodos abstractos encargados de cambiar el flujo del estado actual
        public abstract void OnAreaEnter(Collider other);
        public abstract void OnAreaStay(Collider other);
        public abstract void OnAreaExit(Collider other);
    }
}