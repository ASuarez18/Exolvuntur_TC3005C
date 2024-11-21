using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorcontroler : MonoBehaviour
{
    public Animator animator;


    public GameObject player;
    public GameObject player2;

    public bool Attack,Death,Search,Stun;
    public float Move;

    public bool step2, bandera = false;


    public void Update()
    {

        if(step2 && bandera == false)
        {
            player.SetActive(false);
            player2.SetActive(true);
            animator.SetBool("walk2", true);
            animator.SetTrigger("Step");
            bandera = true;
        }
        else
        {
            if(!step2)bandera = false;
            if(bandera) return;
            animator.SetBool("walk2", false);
            player.SetActive(true);
            player2.SetActive(false);
        }

        animator.SetFloat("Move", Move);

        if (Attack)
        {
            animator.SetTrigger("Attack");
            Attack = false;
        }
        if (Death)
        {
            animator.SetTrigger("Death");
            Death = false;
        }
        if (Search)
        {
            animator.SetTrigger("Search");
            Search = false;
        }
        if (Stun)
        {
            animator.SetTrigger("Stunning");
            animator.SetBool("Stun", true);
            Stun = false;
        }
        else
        {
            animator.SetBool("Stun", false);
        }

    }


}
