using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

public class PlayerIAViewTest : MonoBehaviour
{
    //Atributos
    public Camera cam;
    public Transform enemy;
    public LayerMask layerMask;

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
        // //Llamamos al metodo de raycasting
        EnemyOnView();
        
        

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

    // private void RayCastingView()
    // {
    //     //Creamos un loop for que dibuje varios raycast desde la vista de la camara
    //     for (float i = 0f; i <= 1f; i = i + 0.05f)
    //     {
    //         Ray ray = cam.ViewportPointToRay(new Vector3(i, 0.5f, 0f));
    //         RaycastHit hit;
    //         if (Physics.Raycast(ray, out hit, 5f))
    //         {
    //             //Si colisionamos con algo, imprimimos su nombre
    //             print("I'm looking at " + hit.transform.name);
    //         }
    //         //Dibujamos el rayo
    //         Debug.DrawRay(ray.origin, ray.direction * 5f, Color.yellow);
    //     }
    // }

    private bool EnemyOnView()
    {
        // 1. Determina la posición del enemigo en el espacio del viewport
        Vector3 enemyPos = cam.WorldToViewportPoint(enemy.position);
        // Debug.Log(enemyPos);

        // 2. Comprueba si el enemigo está dentro del rango del viewport
        // Validamos tanto en el eje horizontal (x), vertical (y), y si está al frente de la cámara (z > 0)
        if (enemyPos.x > 0f && enemyPos.x < 1f && enemyPos.y > 0f && enemyPos.y < 1f && enemyPos.z > 0f)
        {
            // Debug.Log("Enemy in view");
            // 3. Crea el rayo desde la cámara hacia el enemigo
            Vector3 enemyDir = enemy.position - cam.transform.position;
            Ray ray = new Ray(cam.transform.position, enemyDir);

            // 4. Dibujamos el rayo para depuración (hasta la distancia al enemigo)
            Debug.DrawRay(ray.origin, ray.direction * 15f, Color.yellow);

            // 5. Lanza el raycast con un límite de alcance (5f en este caso)
            if (Physics.Raycast(ray, out RaycastHit hit, 15f, layerMask))
            {
                // 6. Verifica si el raycast impacta al enemigo
                if (hit.transform.CompareTag("EnemyDybbuk"))
                {
                    // Activa el estado "OnView" en el enemigo
                    hit.transform.gameObject.GetComponent<EnemyDybbukManager>().enemyMachine.OnView = true;
                    return true;
                }
                else
                {
                    // 7. Si no está en el rango de visión, desactiva "OnView" y devuelve false
                    enemy.gameObject.GetComponent<EnemyDybbukManager>().enemyMachine.OnView = false;
                    return false;
                }
            }
        }

        // 7. Si no está en el rango de visión, desactiva "OnView" y devuelve false
        enemy.gameObject.GetComponent<EnemyDybbukManager>().enemyMachine.OnView = false;
        return false;
    }

}
