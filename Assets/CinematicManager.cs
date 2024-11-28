using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> Cinematicas;
    public bool CinematicaFinalizada = false;
    AsyncOperation asyncLoad;
    float progress;


    private void OnEnable()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        asyncLoad = SceneManager.LoadSceneAsync("Gameplay"); 
        Debug.Log(asyncLoad.ToString());
        asyncLoad.allowSceneActivation = false; // No activar la escena de inmediato
        StartCoroutine(WaitScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (asyncLoad.progress >= 0.9f && CinematicaFinalizada)
        {
            
                asyncLoad.allowSceneActivation = true;
                
            
            
        }
    }

    IEnumerator WaitScene()
    {
        
        for (int i = 0; i < Cinematicas.Count; i++)
        {
            foreach (GameObject image in Cinematicas)
            {
                image.SetActive(false);
            }
            Cinematicas[i].SetActive(true);
            Debug.Log("Antes de espera");
            yield return new WaitForSeconds(5f);
            
            Debug.Log("Despues de espera");
        }
        CinematicaFinalizada = true;
        
    }
}
