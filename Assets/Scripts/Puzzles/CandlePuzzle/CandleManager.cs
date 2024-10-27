using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleManager : MonoBehaviour
{
    public Animator doorAnimator;
    private string sequence ="";
    private int currentPos = 0;
    private GameObject[] velas;


    // Start is called before the first frame update
    void Start()
    {
        //Inicializacion de arreglo de objetos velas para apagarlas y verificarlas
        velas = new GameObject[5];

        //Inicializacion de secuencia de comparacion de manera aleatoria
        for(int i = 0; i < 5; i++) 
        {
            string newNumber = UnityEngine.Random.Range(1, 6).ToString();
            if (!sequence.Contains(newNumber))
            {
                sequence = sequence + newNumber;
            }
            
        }

        //Verificacion de asignacion completa de numeros para secuencia 
        if(sequence.Length < 5)
        {
            for(int i = 1;i< 6;i++) 
            {
                if (!sequence.Contains(i.ToString()))
                {
                    sequence = sequence + i.ToString();           
                }
            }
        }

        Debug.Log(sequence);
    }

    public void VelaEncendida(GameObject vela)
    {
        Debug.Log("Vela encendida:" + vela.name);
        velas[currentPos] = vela;
        if (sequence[currentPos] == vela.name[vela.name.Length - 1])
        {
            Debug.Log("Acierto de vela");
            currentPos++;
        }
        else
        {
            Debug.Log("Equivocacion de vela");
            foreach(GameObject objeto in velas)
            {
                objeto.GetComponent<CandleBehavior>().apagarVela();
            }
            currentPos = 0;
            Array.Clear(velas,0,velas.Length);
        }
    }
}
