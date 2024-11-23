using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffect : MonoBehaviour
{
    [Header("Configuración del shaking")]
    public float cantidadSacudida = 0.1f;  
    public float velocidadSacudida = 10f;  

    [Header("Configuración de Intensidad")]
    public float intensidadMinima = 0.5f;  
    public float intensidadMaxima = 1.5f;  
    public float velocidadOscilacionIntensidad = 2f;  


    private Light luzAntorcha;
    private Vector3 posicionInicial;

    void Start()
    {

        luzAntorcha = GetComponent<Light>();
        if (luzAntorcha == null)
        {
            Debug.LogError("No se encontró un componente Light en este GameObject");
            return;
        }
        
        posicionInicial = transform.localPosition;
    }

    void Update()
    {

        SacudirLuz();
        OscilarIntensidad();
    }

    void SacudirLuz()
    {
        // Generar desplazamientos aleatorios suves  basados en el ruido de Perlin 
        float offsetX = (Mathf.PerlinNoise(Time.time * velocidadSacudida, 0.0f) - 0.5f) * 2 * cantidadSacudida;
        float offsetY = (Mathf.PerlinNoise(Time.time * velocidadSacudida, 1.0f) - 0.5f) * 2 * cantidadSacudida;
        float offsetZ = (Mathf.PerlinNoise(Time.time * velocidadSacudida, 2.0f) - 0.5f) * 2 * cantidadSacudida;
        
        transform.localPosition = posicionInicial + new Vector3(offsetX, offsetY, offsetZ);
    }

    void OscilarIntensidad()
    {
        // Crear un efecto de parpadeo variando la intensidad a lo largo del tiempo
        float nuevaIntensidad = Mathf.Lerp(intensidadMinima, intensidadMaxima, Mathf.PerlinNoise(Time.time * velocidadOscilacionIntensidad, 0.0f));
        luzAntorcha.intensity = nuevaIntensidad;
    }
}
