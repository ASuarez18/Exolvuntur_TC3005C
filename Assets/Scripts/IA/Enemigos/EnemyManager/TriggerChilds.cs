using UnityEngine;

namespace Enemy.Behaviour
{
    public class TriggerChilds : MonoBehaviour
    {
        //Creamos un evento de C#
        public delegate void OnArea(Collider other);
        public event OnArea TriggerEventEnter;
        public event OnArea TriggerEventStay;
        public event OnArea TriggerEventExit;

        void OnTriggerEnter(Collider other)
        {
            //Verificamos con que hijo esta colisionando
            if (tag == "AlertaKormos")
            {
                //Invocamos el evento
                Debug.Log("Evento de entrada invocado");
                TriggerEventEnter?.Invoke(other);
            }
            else if (tag == "AtaqueKormos")
            {
                TriggerEventEnter?.Invoke(other);
            }
        }

        void OnTriggerStay(Collider other)
        {
            //Verificamos con que hijo esta colisionando
            if (tag == "AlertaKormos")
            {
                //Invocamos el evento
                // Debug.Log("Evento de estado invocado");
                TriggerEventStay?.Invoke(other);
            }
            else if (tag == "AtaqueKormos")
            {
                TriggerEventStay?.Invoke(other);
            }
        }

        void OnTriggerExit(Collider other)
        {
            //Verificamos con que hijo esta colisionando
            if (tag == "AlertaKormos")
            {
                Debug.Log("Evento de salida invocado");
                TriggerEventExit?.Invoke(other);
            }
            else if (tag == "AtaqueKormos")
            {
                TriggerEventExit?.Invoke(other);
            }
        }
    }

}