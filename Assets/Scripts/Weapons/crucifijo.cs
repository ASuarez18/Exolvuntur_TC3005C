using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crucifijo : MonoBehaviour
{
    public GameObject cubo;


    public Transform point;

    [SerializeField] private float rango = 2f;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && cubo.activeSelf) 
        {
       
            Collider[] colliders = Physics.OverlapCapsule(point.position, point.position + point.forward * rango, 2f);



            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy");
                }
            }
            
        }
        
        
    }
}
