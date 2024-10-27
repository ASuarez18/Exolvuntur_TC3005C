using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/*
    Script que se encarga de llamar el encendido y apagado de cada vela el cual ser� llamado por el jugador con un Raycast
*/
public class CandleBehavior : MonoBehaviour
{
    AudioSource audioVela;

    public void Start()
    {
        audioVela = GetComponent<AudioSource>();
    }
    public void encenderVela() //Funcion de encendido de vela
    {
        Debug.Log("Vela " + this.gameObject.name + " Encendida");

        gameObject.transform.GetChild(0).gameObject.SetActive(true); //Tomamos al hijo de la vela, que en este caso es la flama y lo activamos
        //audioVela.PlayOneShot(audioVela.clip);
        //gameObject.transform.GetComponentInParent<CandleManager>().candles.Add(gameObject.transform.GetChild(0).gameObject); //A�adimos esta flama como gameObject a un arreglo dentro del controlador del puzzle de velas para verificar si el jugador realiza el encendido de las velas por completo
        gameObject.transform.GetComponentInParent<CandleManager>().VelaEncendida(gameObject); //A�adimos esta flama como gameObject a un arreglo dentro del controlador del puzzle de velas para verificar si el jugador realiza el encendido de las velas por completo
    }

    public void apagarVela() //Funcion de apagado de vela
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false); //Se llama a la desactivaci�n de la flama de la vela ya que el jugador cometio un error
    }
}
