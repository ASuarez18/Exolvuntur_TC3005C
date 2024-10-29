using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CandleManager : MonoBehaviour
{
    public Animator doorAnimator;
    private string _sequence ="";
    private int _currentPos = 0;
    private GameObject[] _velas;
    public TMP_Text sequenceText;
    public GameObject sequenceTextObject;

    public List<GameObject> spawnPoints = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        //Inicializacion de arreglo de objetos velas para apagarlas y verificarlas
        _velas = new GameObject[5];

        //Inicializacion de secuencia de comparacion de manera aleatoria
        for(int i = 0; i < 5; i++) 
        {
            string newNumber = UnityEngine.Random.Range(1, 6).ToString();
            if (!_sequence.Contains(newNumber))
            {
                _sequence = _sequence + newNumber;
            }
            
        }

        //Verificacion de asignacion completa de numeros para secuencia 
        if(_sequence.Length < 5)
        {
            for(int i = 1;i< 6;i++) 
            {
                if (!_sequence.Contains(i.ToString()))
                {
                    _sequence = _sequence + i.ToString();           
                }
            }
        }

        Debug.Log(_sequence);
        sequenceText.text = _sequence;

        int indexPosition = UnityEngine.Random.Range(0, spawnPoints.Count +1);

        Debug.Log(indexPosition);

        sequenceTextObject.transform.position = spawnPoints[indexPosition].transform.position;
        sequenceTextObject.transform.rotation = spawnPoints[indexPosition].transform.rotation;

    }

    public void VelaEncendida(GameObject vela)
    {
        Debug.Log("Vela encendida:" + vela.name);
        _velas[_currentPos] = vela;
        if (_sequence[_currentPos] == vela.name[vela.name.Length - 1])
        {
            Debug.Log("Acierto de vela");
            _currentPos++;
        }
        else
        {
            Debug.Log("Equivocacion de vela");
            foreach(GameObject objeto in _velas)
            {
                if(objeto != null)
                {
                    objeto.GetComponent<CandleBehavior>().apagarVela();
                }
                
            }
            _currentPos = 0;
            Array.Clear(_velas,0,_velas.Length);
            
        }
    }
}
