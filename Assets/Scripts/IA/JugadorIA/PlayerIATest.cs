using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlay.IA;

public class PlayerIATest : MonoBehaviour
{
    //Atributos de moviento
    [SerializeField] private float speed = 10.0f;

    //Atributos de sonido
    [SerializeField] private AudioClip footsteps;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float soundRage = 10.0f; //Distancia en metros en la que el jugador escucha el sonido de pasos

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
        //Llamamos al metodo Move para realizar el movimiento
        Move(horizontal, vertical);

    }

    public void Move(float x, float y)
    {
        //Calculamos la dirección del movimiento
        Vector3 movement = new Vector3(x, 0, y);
        //Si la dirección es diferente de cero, reproducimos el sonido de pasos
        if (movement!= Vector3.zero)
        {
            footsteps.PlayOneShot();
            var soundDetector = new SoundGame(transform.position, soundRage);
            soundDetector.soundType = SoundGame.SoundType.Interesting;
            Sounds.MakeSound(soundDetector);
            //Rotamos al jugador según la dirección del movimiento
            Debug.Log(soundDetector.pos);
        }
        //Si la direecion es cero detenemos el sonido
        else
        {
            audioSource.Stop();
        }
        //Aplicamos la fuerza de movimiento al rigidbody
        transform.position += Vector3.Normalize(movement) * speed * Time.deltaTime;
    }
}
