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
                TriggerEventEnter?.Invoke(other);
            }
            else if (tag == "AtaqueKormos")
            {
                TriggerEventEnter?.Invoke(other);
            }
        }

    //     void OnTriggerStay(Collider other)
    //     {
    //         //Verificamos con que hijo esta colisionando
    //         if (tag == "AlertaKormos")
    //         {
    //             //Invocamos el evento
    //             TriggerEvent?.Invoke(other);
    //         }
    //         else if (tag == "AtaqueKormos")
    //         {
    //             TriggerEvent?.Invoke(other);
    //         }
    //     }

    //     void OnTriggerExit(Collider other)
    //     {
    //         //Verificamos con que hijo esta colisionando
    //         if (tag == "AlertaKormos")
    //         {
    //             //Invocamos el evento
    //             TriggerEvent?.Invoke(other);
    //         }
    //         else if (tag == "AtaqueKormos")
    //         {
    //             TriggerEvent?.Invoke(other);
    //         }
    //     }
    }

}