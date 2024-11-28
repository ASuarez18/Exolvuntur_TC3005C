using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public GameObject Araña;
    public GameObject pataFrontalIzq, pataFrontalDer, pataMediaIzq, pataMediaDer, pataTraseraIzq, pataTraseraDer;
    public float anguloParesY, anguloMediaY, AnguloDerechaParX, AnguloIzquierdaParX, AnguloDerechaMediaX, AnguloIzquierdaMediaX; 

    public float deltaPar = 0.5f, deltaMedia = 0.3f;

    public int dirParY=1, dirMediaY=1, dirDerechaParX=1, dirIzquierdaParX=1, dirDerechaMediaX=1, dirIzquierdaMediaX=1;


    // Update is called once per frame
    void Update()
    {
        anguloParesY += dirParY * deltaPar;
        anguloMediaY += dirMediaY * deltaMedia;
        AnguloDerechaParX += dirDerechaParX * deltaPar;
        AnguloIzquierdaParX += dirIzquierdaParX * deltaPar;
        AnguloDerechaMediaX += dirDerechaMediaX * deltaMedia;
        AnguloIzquierdaMediaX += dirIzquierdaMediaX * deltaMedia;

        if (anguloParesY > 10 || anguloParesY < -10) dirParY *= -1;
        if (anguloMediaY > 10 || anguloMediaY < -10) dirMediaY *= -1;
        if (AnguloDerechaParX > 10|| AnguloDerechaParX < -7) dirDerechaParX *= -1;
        if (AnguloIzquierdaParX > -170 || AnguloIzquierdaParX < -190) dirIzquierdaParX *= -1;
        if (AnguloDerechaMediaX > 10 || AnguloDerechaMediaX < -7) dirDerechaMediaX *= -1;
        if (AnguloIzquierdaMediaX > -170 || AnguloIzquierdaMediaX < -190) dirIzquierdaMediaX *= -1;

        pataFrontalIzq.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle", anguloParesY);
        pataFrontalIzq.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle2",-1* AnguloIzquierdaParX);

        pataFrontalDer.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle", anguloParesY);
        pataFrontalDer.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle2", AnguloDerechaParX);

        pataMediaIzq.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle", -1*anguloMediaY);
        pataMediaIzq.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle2", AnguloIzquierdaMediaX);

        pataMediaDer.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle", anguloMediaY);
        pataMediaDer.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle2", AnguloDerechaMediaX);

        pataTraseraIzq.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle",  anguloParesY);
        pataTraseraIzq.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle2", AnguloIzquierdaParX);

        pataTraseraDer.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle", anguloParesY);
        pataTraseraDer.GetComponentInChildren<Renderer>().sharedMaterial.SetFloat("_angle2", -1*AnguloDerechaParX);
 
    }
}