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
            photonView.RPC(nameof(SetFlashActive), RpcTarget.AllBuffered, !flashLight.activeSelf);
        }

    }

    [PunRPC]
    private void SetFlashActive(bool active)
    {
        flashLight.SetActive(active);
    }
}
