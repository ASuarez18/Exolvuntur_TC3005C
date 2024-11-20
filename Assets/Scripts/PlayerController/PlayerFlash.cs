using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlash : MonoBehaviourPunCallbacks
{
   [SerializeField] private GameObject flashLight;

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashLight.SetActive(!flashLight.activeSelf);
        }
        
    }
}
