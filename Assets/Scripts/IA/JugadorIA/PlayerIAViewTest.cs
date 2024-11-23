using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;

public class PlayerIAViewTest : MonoBehaviour
{
    // Atributos
    public Camera cam;
    public LayerMask layerMask;
    public float viewRange = 500f;

    // Start is called before the first frame update
    void Start()
    {
        // Encontramos la cámara principal
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Llamamos al método de raycasting
        EnemyOnView();
    }

    private bool EnemyOnView()
    {
        // Encontramos todos los enemigos activos en la escena con el tag "EnemyDybbuk"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyDybbuk");
        bool enemyInView = false;

        // Recorremos cada enemigo
        foreach (GameObject enemy in enemies)
        {
            // Verificamos si tiene el componente EnemyDybbukManager
            if (enemy.TryGetComponent<EnemyDybbukManager>(out var enemyManager))
            {
                // Calculamos la posición del enemigo en el espacio del viewport
                Vector3 enemyPos = cam.WorldToViewportPoint(enemy.transform.position);

                // Validamos si está dentro del rango del viewport y al frente de la cámara
                if (enemyPos.x > 0f && enemyPos.x < 1f && enemyPos.y > 0f && enemyPos.y < 1f && enemyPos.z > 0f)
                {
                    
                    Vector3 enemyDir = enemy.transform.position - cam.transform.position;
                    Ray ray = new Ray(cam.transform.position, enemyDir);

                    // Dibujamos el rayo para depuración
                    Debug.DrawRay(ray.origin, ray.direction * viewRange, Color.yellow);

                    // Lanza el raycast con un límite de alcance
                    if (Physics.Raycast(ray, out RaycastHit hit, viewRange, layerMask))
                    {
                        // Verifica si el raycast impacta al enemigo
                        if (hit.transform == enemy.transform)
                        {
                            enemyManager.enemyMachine.OnView = true;
                            enemyInView = true;
                        }
                        else
                        {
                            enemyManager.enemyMachine.OnView = false;
                        }
                    }
                }
                else
                {
                    // Si no está en el viewport, desactivamos "OnView"
                    enemyManager.enemyMachine.OnView = false;
                }
            }
        }

        return enemyInView;
    }
}
