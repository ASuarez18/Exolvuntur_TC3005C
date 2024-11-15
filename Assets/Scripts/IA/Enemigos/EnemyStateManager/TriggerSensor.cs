using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSensor : MonoBehaviour
{
    //Creamos un evento que recibe un collider
    public delegate void OnAreaEnter(Collider other);
    public delegate void OnAreaStay(Collider other);
    public delegate void OnAreaExit(Collider other);

    //Declaramos eventos para los áreas de interés
    public event OnAreaEnter TriggerEventEnter;
    public event OnAreaStay TriggerEventStay;
    public event OnAreaExit TriggerEventExit;

    public void OnTriggerEnter(Collider other)
    {
        TriggerEventEnter?.Invoke(other);
    }

    public void OnTriggerStay(Collider other)
    {
        TriggerEventStay?.Invoke(other);
    }

    public void OnTriggerExit(Collider other)
    {
        TriggerEventExit?.Invoke(other);
    }
}