using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastManager : MonoBehaviour
{
    public LayerMask layerMask;
    public RaycastHit hit;
    public float radius;
    public float maxDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // // 
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //    if(Physics.SphereCast(transform.position,radius,transform.forward,out hit, maxDistance, layerMask))
        //    {
        //      Debug.Log("Hit object: " + hit.transform.name);
        //      // Perform actions on hit object here
        //    } 
        // }   
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entre al trigger");
    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("Entre al collision");
    }
}
