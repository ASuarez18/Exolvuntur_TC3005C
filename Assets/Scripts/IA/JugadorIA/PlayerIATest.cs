using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlay.IA;
using Enemy.Manager;
using Enemy.Behaviour;

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

        //Creamos una seria de raycast que ocupan el frustum de la camar
    }

    // Update is called once per frame
    void Update()
    {
        //Obtenemos el input del teclado en w,a,s,d
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Llamamos al metodo Move para realizar el movimiento
        Move(horizontal, vertical);

        #region DamageTest
        Collider[] col = Physics.OverlapSphere(transform.position , 3);

        for(int i=0; i<col.Length; i++)
        {
            if(col[i].TryGetComponent(out EnemyKormosManager enemyStateKormos))
            {
                // Debug.LogWarning("Encontro enemigo");
                if(Input.GetKeyDown(KeyCode.P))
                {
                    // Simulacion de ataque para stunnear
                    enemyStateKormos.enemyMachine.ApplyStun();
                    Debug.Log("Aplico stun");
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    // Simulacion de ataque para daniar
                    enemyStateKormos.enemyMachine.ApplyDamage(20);
                    Debug.LogError($"Aplico danio: {enemyStateKormos.enemyMachine.currentHealth}");
                    
                }
            }

            if(col[i].TryGetComponent(out EnemyDybbukManager enemyStateDybbuk))
            {
                // Debug.LogWarning("Encontro enemigo");
                if(Input.GetKeyDown(KeyCode.P))
                {
                    // Simulacion de ataque para stunnear
                    enemyStateDybbuk.enemyMachine.ApplyStun();
                    Debug.Log("Aplico stun");
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    // Simulacion de ataque para daniar
                    enemyStateDybbuk.enemyMachine.ApplyDamage(20);
                    Debug.LogError($"Aplico danio: {enemyStateDybbuk.enemyMachine.currentHealth}");
                    
                }
            }

            if(col[i].TryGetComponent(out EnemySkinWalkerManager enemyStateSkinWalker))
            {
                // Debug.LogWarning("Encontro enemigo");
                if(Input.GetKeyDown(KeyCode.P))
                {
                    // Simulacion de ataque para stunnear
                    enemyStateSkinWalker.enemyMachine.ApplyStun();
                    Debug.Log("Aplico stun");
                }
                if (Input.GetKeyDown(KeyCode.O))
                {
                    // Simulacion de ataque para daniar
                    enemyStateSkinWalker.enemyMachine.ApplyDamage(20);
                    Debug.LogError($"Aplico danio: {enemyStateSkinWalker.enemyMachine.currentHealth}");
                    
                }
            }
        }
        #endregion

    }

    // public void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, 3);
    // }

    public void Move(float x, float y)
    {
        //Calculamos la dirección del movimiento en posicion local
        Vector3 movement = new Vector3(x, 0, y);
        movement = transform.TransformDirection(movement);
        //Si la dirección es diferente de cero, reproducimos el sonido de pasos
        if (movement!= Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            audioSource.Play(0);
            var soundDetector = new SoundGame(transform.position, soundRage);
            soundDetector.soundType = SoundGame.SoundType.Interesting;
            Sounds.MakeSound(soundDetector);
            //Rotamos al jugador según la dirección del movimiento
        }
        //Si la direecion es cero detenemos el sonido
        else
        {
            audioSource.Stop();
        }
        //Aplicamos la fuerza de movimiento al rigidbody y con rotacion en espacio local
        transform.position += Vector3.Normalize(movement) * speed * Time.deltaTime;
    }
}
