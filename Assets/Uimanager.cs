using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uimanager : MonoBehaviour
{
    public GameObject PauseMenu;
    private bool paused;
   
    GameObject globalPauseMenuinstance;
    GameObject globalRestartinstance;

    RectTransform pauseMenuTransform;

    void Start()
    {
        paused = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            globalPauseMenuinstance.SetActive(paused);
            float RightpauseMenuTransform = -pauseMenuTransform.offsetMax.x;
            RightpauseMenuTransform = 0;

            foreach (Transform t in globalPauseMenuinstance.transform)
            {
                t.gameObject.SetActive(paused);
            }

        }
    }

    private void OnLevelWasLoaded(int level)
    {
       
        if (level > 0) 
        {
            GameObject PausemenuInstance = GameObject.Find("PauseMenu");

            PausemenuInstance.SetActive(false);
    
            globalPauseMenuinstance = PausemenuInstance;
          
            pauseMenuTransform = globalPauseMenuinstance.GetComponent<RectTransform>();
           
            PausemenuInstance.gameObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
           
        }
    }

    
}
