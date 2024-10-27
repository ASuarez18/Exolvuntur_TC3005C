using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPuzzleCandle : MonoBehaviour
{
    public GameObject candle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            candle.GetComponent<CandleBehavior>().encenderVela();
        }
        
    }
}
