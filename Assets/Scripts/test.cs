using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class test : MonoBehaviour
{
    //slider en el editor
    [Range(0,3)]
    public float forward;
    public bool task= false;
    
    public bool muerte = false;
    public bool agresividad = false;
    public bool alerta = false;
    public bool ifstun = false;
    public bool stun = false;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("forward", forward);


        if (task)
        {
            anim.SetTrigger("ataque");
            task = false;
        }

        if (ifstun)
        {
            anim.SetTrigger("stuninter");
            stun = !stun;
            anim.SetBool("stun", stun);
            ifstun = false;
        }

        if (agresividad)
        {
            anim.SetTrigger("agresividad");
            agresividad = false;
        }
        if (alerta)
        {
            anim.SetTrigger("alerta");
            alerta = false;
        }
        if (muerte)
        {
            anim.SetTrigger("muerte");
            muerte = false;
        }
        
    }
}
