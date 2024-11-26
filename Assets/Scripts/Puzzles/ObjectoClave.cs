using Interfaces;
using PlayerController.Inventory;
using UnityEngine;
using LocalPhoton.Gameplay;
using Photon.Pun;

public class ObjectoClave : MonoBehaviourPunCallbacks, IInteraction
{

    public void InteractObject(GameObject player)
    {
        
        player.GetComponent<SlotSelector>().CollectObject(this.gameObject);
        // Update count in total key objects that players have;
        //FindObjectOfType<PUNMatchManager>().TotalObjects++;
        PUNMatchManager.Instance.UpdateStatSent(0, PUNEventCodes.PlayerStats.TotalObjects ,1);

    }

    public void InteractObject()
    {
        
    }
}
