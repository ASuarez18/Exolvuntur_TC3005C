using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces{

    public interface IMovement
    {
        //Atributos
        public CharacterController controller { get; set; }
        //Metodos
        public void Move(Vector3 position);
        public void CameraView(Vector2 position, Transform camera, Transform POVPosition);

        public void CameraPosUpdate(Transform cameraPlayer, Transform POVPosition);
        public bool AreYouOnTheGround();
        public void setPlayerSpeed(float speed);
    }
}


