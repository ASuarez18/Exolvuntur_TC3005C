using UnityEngine;
using Interfaces;
using GamePlay.IA;
using Photon.Pun;
using Unity.VisualScripting;
using PlayerController.PUN;
using PlayerController.Inventory;


namespace PlayerController
{
    public class PlayerControllerInput : MonoBehaviourPunCallbacks
    {

        //Atributos
        //Transform de la camara del jugador
        private Transform cameraPlayer;

        //POV De la camara
        [SerializeField] private Transform _cameraPOV;

        [SerializeField] private Transform _inventoryPOV;


        //Vector de la vista
        Vector2 CameraView;
        //Vector del movimento
        Vector3 movementDirection;
        //Mascara 
        public LayerMask groundMask;
        //Creamos un objeto de tipo interfaz
        private IMovement character;
        [SerializeField] private float soundRage = 20.0f; //Distancia en metros en la que el jugador escucha el sonido de pasos
        //Controlador de audio
        //private AudioController audioController ;
        [SerializeField] private Avatar _playerAvatar;

        // Heal values
        private float _healCooldown = 5f;
        private float _timeSinceHeal = 5f;
        private PUNPlayerSanity _playerSanity;
        [SerializeField] private int _healValue = 20;

        private SlotSelector _slotSelector;

        private void Awake()
        {
            _playerSanity = GetComponent<PUNPlayerSanity>();
            _slotSelector = GetComponent<SlotSelector>();
        }

        void Start()
        {

            enabled = photonView.IsMine;
            

            if (enabled)
            {
                /*_playerModel.SetActive(false)*/;
                
               
            }else{
                this.transform.GetChild(0).gameObject.SetActive(true);
                this.transform.GetChild(1).gameObject.SetActive(false);
                GetComponent<CharacterController>().GetComponent<Animator>().avatar = _playerAvatar;
            }
            //Instanciamos un nuevo objeto de tipo Player y su constructor
            character = new PlayerMovement(GetComponent<CharacterController>(), groundMask);
            // Obtén el primer hijo y su componente AudioController
            // Verificar si el objeto tiene al menos un hijo
            // Verificar si el objeto tiene al menos un 

            // Intentar obtener el componente AudioController
            //audioController = GetComponentInChildren<AudioController>();

            //Desaparecemos el curso de la pantalla
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            character.setPlayerSpeed(30.0f);
            cameraPlayer = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            
            //Input que regresa la posicion que se debe mover
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Input que regresa la posicion que se debe rotar la camara
            Vector2 cameraView = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            //Movimiento y vista del personaje

            character.CameraView(cameraView,cameraPlayer, _cameraPOV);
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
            else if (movementDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                var soundDetector = new SoundGame(transform.position, soundRage);
                soundDetector.soundType = SoundGame.SoundType.Interesting;
                Sounds.MakeSound(soundDetector);
                // Debug.Log("Haciendo ruido");
                //Rotamos al jugador según la dirección del movimiento
            }

            // Ground character
            character.AreYouOnTheGround();

            // Update heal cooldown
            _timeSinceHeal += Time.deltaTime;
            //Debug.Log($"TimeSinceHeal: {_timeSinceHeal} | HealCooldown: {_healCooldown}");
            if (Input.GetKeyDown(KeyCode.C) && _timeSinceHeal >= _healCooldown)
            {
                //Debug.Log($"EnterHeal: {_playerSanity.Sanity}");
                _timeSinceHeal = 0f;
                _playerSanity.HealItself(_healValue);
                _slotSelector.SetHealBottleAlpha(30f, _healCooldown);
            }
            if (Input.GetKeyDown(KeyCode.G))
                _playerSanity.TakeDamage(30, "test");
        }

        private void LateUpdate()
        {
            character.CameraPosUpdate(cameraPlayer, _cameraPOV);
            //_inventoryPOV.transform.position = cameraPlayer.rotation;
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

        //Dibujamos una wire esfera
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, soundRage);
        }

        // void OnControllerColliderHit(ControllerColliderHit hit)
        // {
        //      InterfaceAttacking enemy = hit.transform.GetComponent<InterfaceAttacking>();
        //      if(enemy != null)
        //      {
        //          enemy.Attack(gameObject);
        //      }
        // }

    }
}

