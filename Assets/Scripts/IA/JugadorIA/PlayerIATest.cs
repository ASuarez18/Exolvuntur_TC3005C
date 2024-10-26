using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIATest : MonoBehaviour
{
    //Atributos de moviento
    [SerializeField] private float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Revisamos que el jugador tenga un rigidbody , si no le creamos uno
        if (!TryGetComponent<Rigidbody>(out Rigidbody body))
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Obtenemos el input del teclado en w,a,s,d
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

    }

    public void Move(float x, float y)
    {
        //Calculamos la dirección del movimiento
        Vector3 movement = new Vector3(x, 0, y);
        //Aplicamos la fuerza de movimiento al rigidbody
        transform.position += movement * speed * Time.deltaTime;
    }
}
