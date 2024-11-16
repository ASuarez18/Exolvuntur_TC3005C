using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIAViewTest : MonoBehaviour
{
    //Atributos
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        //Inicializamos el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  

        //Encontramos la camara principal
        Camera cam = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        //Obtenemos el movimiento del mouse en el eje X y Y
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * 100f;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * 100f;
        
        //Actualizamos la rotación del jugador en el eje Y
        transform.Rotate(Vector3.up * mouseX);

        // //Actualizamos la rotación de la cámara en el eje y
        // cam.transform.Rotate(Vector3.up * mouseX);

        //Actualizamos la rotacion de la camara en el eje x
        cam.transform.Rotate(Vector3.left * mouseY);
        
    }
}
