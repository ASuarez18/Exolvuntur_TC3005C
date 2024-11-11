using UnityEngine;

public class test : MonoBehaviour
{
    //slider en el editor
    [Range(-1,1)]
    public float forward,side;
    public int task;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Forward", forward);
        anim.SetFloat("Side", side);
        anim.SetInteger("task", task);

        
    }
}
