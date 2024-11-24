using Interfaces;
using PlayerController.Inventory;
using UnityEngine;
using LocalPhoton.Gameplay;

public class ObjectoClave : MonoBehaviour, IInteraction
{

    public void InteractObject()
    {
        FindObjectOfType<SlotSelector>().CollectObject(this.gameObject);
        // Update count in total key objects that players have;
        FindObjectOfType<PUNMatchManager>().TotalObjects++;

    }
}
