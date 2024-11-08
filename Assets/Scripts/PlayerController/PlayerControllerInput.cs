using UnityEngine;
using Interfaces;

namespace PlayerController
{
    public class PlayerControllerInput : MonoBehaviour
    {

        //Atributos
        //Transform de la camara del jugador
        [SerializeField] Transform cameraPlayer;
        //Vector de la vista
        Vector2 CameraView;
        //Vector del movimento
        Vector3 movementDirection;
        //Mascara 
        public LayerMask groundMask;
        //Creamos un objeto de tipo interfaz
        private IMovement character;
        //Controlador de audio
        //private AudioController audioController ;

        void Start()
        {
            //Instanciamos un nuevo objeto de tipo Player y su constructor
            character = new PlayerMovement(GetComponent<CharacterController>(), groundMask);
            // Obtén el primer hijo y su componente AudioController
            // Verificar si el objeto tiene al menos un hijo
             // Verificar si el objeto tiene al menos un hijo
        
                
            // Intentar obtener el componente AudioController
            //audioController = GetComponentInChildren<AudioController>();

        
            //Desaparecemos el curso de la pantalla
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            character.setPlayerSpeed(30.0f);
        }

        // Update is called once per frame
        void Update()
        {
            //Input que regresa la posicion que se debe mover
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Input que regresa la posicion que se debe rotar la camara
            Vector2 cameraView = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            //Movimiento y vista del personaje

            character.CameraView(cameraView,cameraPlayer);
            if (movementDirection != Vector3.zero)
            {
                character.Move(movementDirection);
            }

            if(Input.GetKey(KeyCode.LeftShift))
            {
                character.setPlayerSpeed(15f);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                character.setPlayerSpeed(30f);
            }

            // Ground character
            character.AreYouOnTheGround();
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Stone")){
                //audioController.groundType = 0;
            }
            if(other.CompareTag("Water")){
                //audioController.groundType = 1;
            }
            if(other.CompareTag("Monster"))
            {
                 //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }

    }
}

